using System.Windows.Input;
using Community.PowerToys.Run.Plugin.Twitch.Models;
using LazyCache;
using ManagedCommon;
using Wox.Infrastructure;
using Wox.Infrastructure.Storage;
using Wox.Plugin;
using Wox.Plugin.Common;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Twitch
{
    /// <summary>
    /// Main class of this plugin that implement all used interfaces.
    /// </summary>
    public class Main : IPlugin, IDelayedExecutionPlugin, IContextMenu, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            Log.Info($"Ctor", GetType());

            Storage = new PluginJsonStorage<TwitchSettings>();
            Settings = Storage.Load();
            TwitchClient = new TwitchClient(Settings, new CachingService());
        }

        internal Main(TwitchSettings settings, ITwitchClient twitchClient)
        {
            Settings = settings;
            TwitchClient = twitchClient;
        }

        /// <summary>
        /// ID of the plugin.
        /// </summary>
        public static string PluginID => "69340CBAA6A54D73861135D4AB7A5276";

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "Twitch";

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Browse, search and view streams on Twitch";

        private PluginJsonStorage<TwitchSettings>? Storage { get; }

        private TwitchSettings Settings { get; }

        private ITwitchClient TwitchClient { get; }

        private PluginInitContext? Context { get; set; }

        private string? IconPath { get; set; }

        private bool Disposed { get; set; }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query)
        {
            Log.Info($"Query: {query?.RawQuery}", GetType());

            return new List<Result>(0);
        }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <param name="delayedExecution">False if this is the first pass through plugins, true otherwise. Slow plugins should run delayed.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query, bool delayedExecution)
        {
            Log.Info($"Query: {query?.RawQuery}, {delayedExecution}", GetType());

            if (query?.Search is null || !delayedExecution)
            {
                return new List<Result>(0);
            }

            var args = query.Search;

            if (string.IsNullOrEmpty(args))
            {
                return new List<Result>(0);
            }

            // TODO: config
            var first = 100;
            var language = "en";
            var live_only = true;

            var tokens = args.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var command = tokens.First();
            var after = Param("after", tokens);
            var before = Param("before", tokens);
            var game_id = Param("game_id", tokens);
            var q = Query(tokens);

            Log.Info($"{tokens.Length} tokens: after={after}, before={before}, game_id={game_id}, q={q}", GetType());

#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
            switch (command)
            {
                case "game":
                    var response = q == null ?
                        TwitchClient.GetGamesAsync(after: after, before: before, first: first).Result :
                        TwitchClient.SearchGamesAsync(query: q, after: after, first: first).Result;
                    return GetResultsFromGamesResponse(response);
                case "channel": return GetResultsFromChannelsResponse(TwitchClient.SearchChannelsAsync(query: q!, live_only: live_only, after: after, first: first).Result);
                case "stream": return GetResultsFromStreamsResponse(TwitchClient.GetStreamsAsync(game_id: game_id, language: language, after: after, before: before, first: first).Result);
                default: return new List<Result>(0);
            }
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits

            string? Param(string name, string[] tokens)
            {
                var param = tokens.FirstOrDefault(x => x.StartsWith(name, StringComparison.InvariantCulture));

                if (param != null)
                {
                    return param.Split('=', 2).Last();
                }

                return null;
            }

            string? Query(string[] tokens)
            {
                var results = tokens.Skip(1).Where(x => !x.Contains('=', StringComparison.InvariantCulture));

                if (results.Any())
                {
                    return string.Join(' ', results);
                }

                return null;
            }

            List<Result> GetResultsFromGamesResponse(GamesResponse? response)
            {
                var results = new List<Result>();

                if (response?.data == null)
                {
                    return results;
                }

                foreach (var game in response.data)
                {
                    results.Add(new Result
                    {
                        QueryTextDisplay = "stream game_id=" + game.id,
                        IcoPath = IconPath,
                        Title = game.name,
                        SubTitle = game.name,
                        ToolTipData = new ToolTipData("Game", game.name),
                        Action = _ =>
                        {
                            Log.Info("Get streams (Enter): " + game.name, GetType());

                            Context?.API.ChangeQuery("twitch stream game_id=" + game.id, true);

                            return false;
                        },
                        ContextData = game,
                    });
                }

                results.Add(GetResultFromPagination(response.pagination));

                return results;
            }

            List<Result> GetResultsFromChannelsResponse(ChannelsResponse? response)
            {
                var results = new List<Result>();

                if (response?.data == null)
                {
                    return results;
                }

                foreach (var channel in response.data)
                {
                    var arguments = $"https://www.twitch.tv/{UrlEncode(channel.broadcaster_login)}";

                    results.Add(new Result
                    {
                        IcoPath = IconPath,
                        Title = channel.title,
                        SubTitle = channel.display_name,
                        ToolTipData = new ToolTipData("Channel", channel.title),
                        ProgramArguments = arguments,
                        Action = _ =>
                        {
                            Log.Info($"Channel: {channel.title}, {channel.id}", GetType());

                            if (!Helper.OpenCommandInShell(DefaultBrowserInfo.Path, DefaultBrowserInfo.ArgumentsPattern, arguments))
                            {
                                Log.Error("Open default browser failed.", GetType());
                                Context?.API.ShowMsg($"Plugin: {Name}", "Open default browser failed.");
                                return false;
                            }

                            return true;
                        },
                    });
                }

                results.Add(GetResultFromPagination(response.pagination));

                return results;
            }

            List<Result> GetResultsFromStreamsResponse(StreamsResponse? response)
            {
                var results = new List<Result>();

                if (response?.data == null)
                {
                    return results;
                }

                foreach (var stream in response.data)
                {
                    var arguments = $"https://www.twitch.tv/{UrlEncode(stream.user_login)}";

                    results.Add(new Result
                    {
                        IcoPath = IconPath,
                        Title = stream.title,
                        SubTitle = stream.type,
                        ToolTipData = new ToolTipData("Stream", stream.title),
                        ProgramArguments = arguments,
                        Action = _ =>
                        {
                            Log.Info($"Stream: {stream.title}, {stream.id}", GetType());

                            if (!Helper.OpenCommandInShell(DefaultBrowserInfo.Path, DefaultBrowserInfo.ArgumentsPattern, arguments))
                            {
                                Log.Error("Open default browser failed.", GetType());
                                Context?.API.ShowMsg($"Plugin: {Name}", "Open default browser failed.");
                                return false;
                            }

                            return true;
                        },
                    });
                }

                results.Add(GetResultFromPagination(response.pagination));

                return results;
            }

            Result GetResultFromPagination(Pagination pagination)
            {
                return new Result
                {
                    QueryTextDisplay = command + " " + q,
                    IcoPath = IconPath,
                    Title = "Pagination",
                    SubTitle = pagination.cursor,
                    ToolTipData = new ToolTipData("Pagination", pagination.cursor),
                    ContextData = pagination,
                    Score = int.MaxValue,
                };
            }
        }

        /// <summary>
        /// Initialize the plugin with the given <see cref="PluginInitContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="PluginInitContext"/> for this plugin.</param>
        public void Init(PluginInitContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(Context.API.GetCurrentTheme());
        }

        /// <summary>
        /// Return a list context menu entries for a given <see cref="Result"/> (shown at the right side of the result).
        /// </summary>
        /// <param name="selectedResult">The <see cref="Result"/> for the list with context menu entries.</param>
        /// <returns>A list context menu entries.</returns>
        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            if (selectedResult?.ContextData is GameData game)
            {
                return new List<ContextMenuResult>
                {
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Get streams (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xEC05", // EC05 => Symbol: NetworkTower
                        /*AcceleratorKey = Key.Enter,*/
                        Action = _ =>
                        {
                            Log.Info("Get streams (Enter): " + game.name, GetType());

                            Context?.API.ChangeQuery("twitch stream game_id=" + game.id, true);

                            return false;
                        },
                    },
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Open website (Ctrl+Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xF6FA", // F6FA => Symbol: WebSearch
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ =>
                        {
                            Log.Info("Open website (Ctrl+Enter): " + game.name, GetType());

                            var arguments = $"https://www.twitch.tv/directory/game/{UrlEncode(game.name)}";

                            if (!Helper.OpenCommandInShell(DefaultBrowserInfo.Path, DefaultBrowserInfo.ArgumentsPattern, arguments))
                            {
                                Log.Error("Open default browser failed.", GetType());
                                Context?.API.ShowMsg($"Plugin: {Name}", "Open default browser failed.");
                                return false;
                            }

                            return true;
                        },
                    },
                };
            }

            if (selectedResult?.ContextData is Pagination pagination)
            {
                return new List<ContextMenuResult>
                {
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Previous (Left)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE892", // E892 => Symbol: Previous
                        AcceleratorKey = Key.Left,
                        Action = _ =>
                        {
                            Log.Info("Previous (Left): " + pagination.cursor, GetType());

                            Context?.API.ChangeQuery(selectedResult.QueryTextDisplay + " before=" + pagination.cursor, true);

                            return false;
                        },
                    },
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Next (Right)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE893", // E893 => Symbol: Next
                        AcceleratorKey = Key.Right,
                        Action = _ =>
                        {
                            Log.Info("Next (Right): " + pagination.cursor, GetType());

                            Context?.API.ChangeQuery(selectedResult.QueryTextDisplay + " after=" + pagination.cursor, true);

                            return false;
                        },
                    },
                };
            }

            return new List<ContextMenuResult>(0);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Wrapper method for <see cref="Dispose()"/> that dispose additional objects and events form the plugin itself.
        /// </summary>
        /// <param name="disposing">Indicate that the plugin is disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed || !disposing)
            {
                return;
            }

            if (Context != null && Context.API != null)
            {
                Context.API.ThemeChanged -= OnThemeChanged;
            }

            Disposed = true;
        }

        private static string UrlEncode(string q)
        {
            return Uri.EscapeDataString(q);
        }

        private void UpdateIconPath(Theme theme) => IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite ? "Images/twitch.light.png" : "Images/twitch.dark.png";

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);
    }
}

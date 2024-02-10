using System.Net;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Input;
using Community.PowerToys.Run.Plugin.Twitch.Models;
using LazyCache;
using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
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
    public class Main : IPlugin, IDelayedExecutionPlugin, IContextMenu, ISettingProvider, ISavable, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            Log.Info("Ctor", GetType());

            Storage = new PluginJsonStorage<TwitchSettings>();
            Settings = Storage.Load();
            TwitchClient = new TwitchClient(Settings, new CachingService());
        }

        internal Main(TwitchSettings settings, ITwitchClient twitchClient)
        {
            Storage = new PluginJsonStorage<TwitchSettings>();
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

        /// <summary>
        /// Additional options for the plugin.
        /// </summary>
        public IEnumerable<PluginAdditionalOption> AdditionalOptions => Settings.GetAdditionalOptions();

        private PluginInitContext? Context { get; set; }

        private string? IconPath { get; set; }

        private bool Disposed { get; set; }

        private PluginJsonStorage<TwitchSettings> Storage { get; }

        private TwitchSettings Settings { get; }

        private ITwitchClient TwitchClient { get; }

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

            if (!Settings.HasValidTwitchApiCredentials())
            {
                return GetResultsFromInvalidTwitchApiCredentials();
            }

            var args = query.Search;

            if (string.IsNullOrEmpty(args))
            {
                return GetResultsFromEmptyQuery();
            }

            try
            {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                return args switch
                {
                    var x when x.StartsWith(Command.Games, StringComparison.OrdinalIgnoreCase) =>
                        GetResultsFromCategoriesResponse(TwitchClient.GetTopGamesAsync(Pagination(args)).Result),
                    var x when x.StartsWith(Command.Categories, StringComparison.OrdinalIgnoreCase) =>
                        GetResultsFromCategoriesResponse(TwitchClient.SearchCategoriesAsync(Query(args), Pagination(args)).Result),
                    var x when x.StartsWith(Command.Channels, StringComparison.OrdinalIgnoreCase) =>
                        GetResultsFromChannelsResponse(TwitchClient.SearchChannelsAsync(Query(args), Pagination(args)).Result),
                    var x when x.StartsWith(Command.Streams, StringComparison.OrdinalIgnoreCase) =>
                        GetResultsFromStreamsResponse(TwitchClient.GetStreamsAsync(gameId: Query(args), Pagination(args)).Result),
                    _ =>
                        GetResultsFromQuery(args),
                };
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
            }
            catch (AggregateException ex) when (ex.InnerException is HttpRequestException hre && hre.StatusCode == HttpStatusCode.BadRequest)
            {
                Log.Exception("Twitch API 400 Bad Request.", ex, GetType());
            }
            catch (AggregateException ex) when (ex.InnerException is HttpRequestException hre && hre.StatusCode == HttpStatusCode.Unauthorized)
            {
                Log.Exception("Twitch API 401 Unauthorized.", ex, GetType());
                Context?.API.ShowMsg($"Plugin: {Name}", "The Twitch API returned 401 Unauthorized. Make sure the plugin settings has the correct Client ID and Client Secret and then restart PowerToys.");
            }

            return new List<Result>(0);

            string Query(string args)
            {
                var index = args.IndexOf(' ', StringComparison.InvariantCulture);

                if (index < 0)
                {
                    return string.Empty;
                }

                return args.Substring(index).Clean();
            }

            Models.Page Pagination(string args) => args switch
            {
                null => Models.Page.None,
                var x when x.Contains(Command.Previous, StringComparison.Ordinal) => Models.Page.Previous,
                var x when x.Contains(Command.Next, StringComparison.Ordinal) => Models.Page.Next,
                _ => Models.Page.None,
            };

            List<Result> GetResultsFromInvalidTwitchApiCredentials()
            {
                const string twitch = "https://dev.twitch.tv/console/apps/create";
                const string github = "https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins";

                return [
                    new Result
                    {
                        IcoPath = IconPath,
                        Title = "1. Register Your Application",
                        SubTitle = "Register a new application that uses the Twitch API to interact with Twitch",
                        ToolTipData = new ToolTipData("Help", "1. Enter a Name\n2. Enter an dummy OAuth Redirect URL, for example http://localhost\n3. Select Category, for example Application Integration\n4. Select Confidential Client Type\n5. Click the Create button"),
                        Score = 3,
                        ProgramArguments = twitch,
                        Action = _ => OpenInBrowser(twitch),
                    },
                    new Result
                    {
                        IcoPath = IconPath,
                        Title = "2. Update Plugin Settings",
                        SubTitle = "Open the plugin settings and enter the Client ID and Client Secret",
                        ToolTipData = new ToolTipData("Help", "1. Open PowerToys Settings\n2. Click PowerToys Run\n3. Scroll down and expand the Twitch plugin\n4. Paste the Twitch API Client ID\n5. Paste the Twitch API Client Secret"),
                        Score = 2,
                    },
                    new Result
                    {
                        IcoPath = IconPath,
                        Title = "3. Documentation",
                        SubTitle = "Read the plugin documentation in the GitHub repo",
                        ToolTipData = new ToolTipData("Help", "1. Requirements\n2. Installation\n3. Usage\n4. Configuration"),
                        Score = 1,
                        ProgramArguments = github,
                        Action = _ => OpenInBrowser(github),
                    },
                ];
            }

            List<Result> GetResultsFromEmptyQuery() => [
                new Result
                {
                    QueryTextDisplay = Command.Games,
                    IcoPath = IconPath,
                    Title = "Top games",
                    SubTitle = "Gets information about all broadcasts on Twitch.",
                    ToolTipData = new ToolTipData("Games", "Live streams of all your favorite games, from shooters to platformers and everything in between."),
                    Action = _ => ChangeQuery(Command.Games),
                },
                new Result
                {
                    QueryTextDisplay = Command.Channels + " ",
                    IcoPath = IconPath,
                    Title = "Search channels",
                    SubTitle = "Gets the channels that match the specified query and have streamed content within the past 6 months.",
                    ToolTipData = new ToolTipData("Channels", "The fields that the API uses for comparison depends on the value that the live_only query parameter is set to. If live_only is false, the API matches on the broadcaster’s login name. However, if live_only is true, the API matches on the broadcaster’s name and category name.\r\nTo match, the beginning of the broadcaster’s name or category must match the query string. The comparison is case insensitive. If the query string is angel_of_death, it matches all names that begin with angel_of_death. However, if the query string is a phrase like angel of death, it matches to names starting with angelofdeath or names starting with angel_of_death.\r\nBy default, the results include both live and offline channels. To get only live channels set the live_only query parameter to true."),
                    Action = _ => ChangeQuery(Command.Channels + " "),
                },
                new Result
                {
                    QueryTextDisplay = Command.Categories + " ",
                    IcoPath = IconPath,
                    Title = "Search categories",
                    SubTitle = "Gets the games or categories that match the specified query.",
                    ToolTipData = new ToolTipData("Categories", "To match, the category’s name must contain all parts of the query string. For example, if the query string is 42, the response includes any category name that contains 42 in the title. If the query string is a phrase like love computer, the response includes any category name that contains the words love and computer anywhere in the name. The comparison is case insensitive."),
                    Action = _ => ChangeQuery(Command.Categories + " "),
                },
            ];

            List<Result> GetResultsFromQuery(string q) => [
                new Result
                {
                    QueryTextDisplay = Command.Channels + " " + q,
                    IcoPath = IconPath,
                    Title = "Search channels: " + q,
                    SubTitle = "Gets the channels that match the specified query and have streamed content within the past 6 months.",
                    ToolTipData = new ToolTipData("Channels", "The fields that the API uses for comparison depends on the value that the live_only query parameter is set to. If live_only is false, the API matches on the broadcaster’s login name. However, if live_only is true, the API matches on the broadcaster’s name and category name.\r\nTo match, the beginning of the broadcaster’s name or category must match the query string. The comparison is case insensitive. If the query string is angel_of_death, it matches all names that begin with angel_of_death. However, if the query string is a phrase like angel of death, it matches to names starting with angelofdeath or names starting with angel_of_death.\r\nBy default, the results include both live and offline channels. To get only live channels set the live_only query parameter to true."),
                    Action = _ => ChangeQuery(Command.Channels + " " + q),
                },
                new Result
                {
                    QueryTextDisplay = Command.Categories + " " + q,
                    IcoPath = IconPath,
                    Title = "Search categories: " + q,
                    SubTitle = "Gets the games or categories that match the specified query.",
                    ToolTipData = new ToolTipData("Categories", "To match, the category’s name must contain all parts of the query string. For example, if the query string is 42, the response includes any category name that contains 42 in the title. If the query string is a phrase like love computer, the response includes any category name that contains the words love and computer anywhere in the name. The comparison is case insensitive."),
                    Action = _ => ChangeQuery(Command.Categories + " " + q),
                },
            ];

            List<Result> GetResultsFromCategoriesResponse(CategoriesResponse? response)
            {
                var results = new List<Result>();

                if (response?.data == null)
                {
                    return results;
                }

                foreach (var category in response.data)
                {
                    results.Add(new Result
                    {
                        QueryTextDisplay = Command.Streams + " " + category.id,
                        IcoPath = IconPath,
                        Title = category.name,
                        SubTitle = "ID: " + category.id,
                        ToolTipData = new ToolTipData("Category", $"Name: {category.name}\nID: {category.id}\nIGDB ID: {category.igdb_id}"),
                        Action = _ => ChangeQuery(Command.Streams + " " + category.id),
                        ContextData = category,
                    });
                }

                if (response.pagination?.cursor != null)
                {
                    results.Add(GetResultFromPagination(response.pagination));
                }

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
                    var arguments = TwitchClient.GetUrl(channel);

                    results.Add(new Result
                    {
                        QueryTextDisplay = args,
                        IcoPath = IconPath,
                        Title = channel.title,
                        SubTitle = channel.display_name.Pipe(channel.game_name, channel.broadcaster_language, channel.is_live ? "📺 " + channel.started_at.Elapsed() : string.Empty),
                        ToolTipData = new ToolTipData("Channel", $"Title: {channel.title}\nID: {channel.id}\nBroadcaster: {channel.display_name}\nGame: {channel.game_name}\nLanguage: {channel.broadcaster_language}\nLive: {channel.is_live}\nStarted: {channel.started_at}\nTags: {string.Join(", ", channel.tags ?? [])}"),
                        ContextData = channel,
                        ProgramArguments = arguments,
                        Action = _ => OpenInBrowser(arguments),
                    });
                }

                if (response.pagination?.cursor != null)
                {
                    results.Add(GetResultFromPagination(response.pagination));
                }

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
                    var arguments = TwitchClient.GetUrl(stream);

                    results.Add(new Result
                    {
                        QueryTextDisplay = args,
                        IcoPath = IconPath,
                        Title = stream.title,
                        SubTitle = stream.user_name.Pipe(stream.game_name, stream.language, stream.type == "live" ? "📺 " + stream.started_at.Elapsed() : string.Empty, stream.viewer_count > 0 ? "👤 " + stream.viewer_count.Format() : string.Empty, stream.is_mature ? "🔞" : string.Empty),
                        ToolTipData = new ToolTipData("Channel", $"Title: {stream.title}\nID: {stream.id}\nBroadcaster: {stream.user_name}\nGame: {stream.game_name}\nLanguage: {stream.language}\nLive: {stream.type == "live"}\nStarted: {stream.started_at}\nViewers: {stream.viewer_count}\nMature: {stream.is_mature}\nTags: {string.Join(", ", stream.tags ?? [])}"),
                        ContextData = stream,
                        ProgramArguments = arguments,
                        Action = _ => OpenInBrowser(arguments),
                    });
                }

                if (response.pagination?.cursor != null)
                {
                    results.Add(GetResultFromPagination(response.pagination));
                }

                return results;
            }

            Result GetResultFromPagination(Pagination pagination)
            {
                return new Result
                {
                    QueryTextDisplay = args,
                    IcoPath = IconPath,
                    Title = "Pagination: Next",
                    SubTitle = $"Get next {Settings.TwitchApiParameterFirst} results",
                    ToolTipData = new ToolTipData("Pagination", $"Cursor: {pagination.cursor}"),
                    ContextData = pagination,
                    Score = -100,
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
            if (selectedResult?.ContextData is CategoryData category)
            {
                return
                [
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Get streams (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xEC05", // EC05 => Symbol: NetworkTower
                        /* AcceleratorKey = Key.Enter, */
                        Action = _ => ChangeQuery(Command.Streams + " " + category.id),
                    },
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Open website (Ctrl+Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xF6FA", // F6FA => Symbol: WebSearch
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ => OpenInBrowser(TwitchClient.GetUrl(category)),
                    },
                ];
            }

            if (selectedResult?.ContextData is ChannelData channel)
            {
                return
                [
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Open website (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE774", // E774 => Symbol: Globe
                        /* AcceleratorKey = Key.Enter, */
                        Action = _ => OpenInBrowser(TwitchClient.GetUrl(channel)),
                    },
                ];
            }

            if (selectedResult?.ContextData is StreamData stream)
            {
                return
                [
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Open website (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE774", // E774 => Symbol: Globe
                        /* AcceleratorKey = Key.Enter, */
                        Action = _ => OpenInBrowser(TwitchClient.GetUrl(stream)),
                    },
                ];
            }

            if (selectedResult?.ContextData is Pagination pagination)
            {
                var q = selectedResult.QueryTextDisplay.Clean();

                return
                [
                    /*
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Previous (Alt+Left)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE892", // E892 => Symbol: Previous
                        AcceleratorKey = Key.Left,
                        AcceleratorModifiers = ModifierKeys.Alt,
                        Action = _ => ChangeQuery(q + " " + Command.Previous, false),
                    },
                    */
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Next (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE893", // E893 => Symbol: Next
                        AcceleratorKey = Key.Enter,
                        Action = _ => ChangeQuery(q + " " + Command.Next, false),
                    },
                ];
            }

            return new List<ContextMenuResult>(0);
        }

        /// <summary>
        /// Creates setting panel.
        /// </summary>
        /// <returns>The control.</returns>
        /// <exception cref="NotImplementedException">method is not implemented.</exception>
        public Control CreateSettingPanel() => throw new NotImplementedException();

        /// <summary>
        /// Updates settings.
        /// </summary>
        /// <param name="settings">The plugin settings.</param>
        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            Settings.SetAdditionalOptions(settings.AdditionalOptions);
            Save();
        }

        /// <summary>
        /// Saves settings.
        /// </summary>
        public void Save()
        {
            Storage.Save();
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

            if (Context?.API != null)
            {
                Context.API.ThemeChanged -= OnThemeChanged;
            }

            Disposed = true;
        }

        private void UpdateIconPath(Theme theme) => IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite ? "Images/twitch.light.png" : "Images/twitch.dark.png";

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);

        private bool OpenInBrowser(string url)
        {
            if (!Helper.OpenCommandInShell(DefaultBrowserInfo.Path, DefaultBrowserInfo.ArgumentsPattern, url))
            {
                Log.Error("Open default browser failed.", GetType());
                Context?.API.ShowMsg($"Plugin: {Name}", "Open default browser failed.");
                return false;
            }

            return true;
        }

        private bool ChangeQuery(string command, bool appendActionKeyword = true)
        {
            if (appendActionKeyword)
            {
                Context?.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword + " " + command, true);
            }
            else
            {
                Context?.API.ChangeQuery(command, true);
            }

            return false;
        }
    }

    internal static class Command
    {
        public const string Games = "games";
        public const string Categories = "categories";
        public const string Channels = "channels";
        public const string Streams = "streams";

        // Pagination
        public const string Previous = "⏮️";
        public const string Next = "⏭️";
    }
}

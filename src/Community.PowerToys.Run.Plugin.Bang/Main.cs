using System.Windows.Input;
using Community.PowerToys.Run.Plugin.Bang.Models;
using LazyCache;
using ManagedCommon;
using Wox.Infrastructure;
using Wox.Plugin;
using Wox.Plugin.Common;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Bang
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
            DuckDuckGoClient = new DuckDuckGoClient(new CachingService());
        }

        internal Main(IDuckDuckGoClient duckDuckGoClient)
        {
            DuckDuckGoClient = duckDuckGoClient;
        }

        /// <summary>
        /// ID of the plugin.
        /// </summary>
        public static string PluginID => "888A6F53700D4F16841160FC0E8AAC2A";

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "Bang";

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Search websites with DuckDuckGo !Bangs";

        private PluginInitContext? Context { get; set; }

        private string? IconPath { get; set; }

        private bool Disposed { get; set; }

        private IDuckDuckGoClient DuckDuckGoClient { get; }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query)
        {
            return [];
        }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <param name="delayedExecution">False if this is the first pass through plugins, true otherwise. Slow plugins should run delayed.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query, bool delayedExecution)
        {
            if (query?.Search is null || !delayedExecution)
            {
                return [];
            }

            var q = query.Search;

            return GetResultsFromQuery(Bangify(q)).ToList();

            string Bangify(string q) => q.StartsWith('!') ? q : "!" + q;

            IEnumerable<Result> GetResultsFromQuery(string q)
            {
                var suggestions = AutoComplete(q);

                if (suggestions?.Any() != true)
                {
                    yield break;
                }

                foreach (var suggestion in suggestions)
                {
                    if (suggestion.Snippet != null)
                    {
                        yield return GetResultFromSnippet(suggestion);
                    }
                    else
                    {
                        yield return GetResultFromPhrase(suggestion);
                    }
                }

                if (suggestions.All(x => x.Snippet == null) && suggestions.All(x => x.Phrase != q))
                {
                    yield return GetResultFromQuery(q);
                }
            }

            Result GetResultFromSnippet(Suggestion suggestion) => new()
            {
                QueryTextDisplay = suggestion.Phrase + " ",
                IcoPath = IconPath,
                Title = suggestion.Snippet,
                SubTitle = suggestion.Phrase,
                ToolTipData = new ToolTipData("Bang", $"Search {suggestion.Snippet}"),
                ContextData = suggestion,
            };

            Result GetResultFromPhrase(Suggestion suggestion)
            {
                var website = GetSnippet(suggestion.Phrase);
                var terms = DuckDuckGoClient.GetSearchTerms(suggestion.Phrase);

                return new()
                {
                    QueryTextDisplay = suggestion.Phrase,
                    IcoPath = IconPath,
                    Title = $"{website?.Snippet}: {terms}",
                    SubTitle = suggestion.Phrase,
                    ToolTipData = new ToolTipData("Bang", $"Search {website?.Snippet}"),
                    ContextData = suggestion,
                    Score = suggestion.Phrase == q ? 100 : 0,
                };
            }

            Result GetResultFromQuery(string q)
            {
                var website = GetSnippet(q);
                var terms = DuckDuckGoClient.GetSearchTerms(q);

                return new()
                {
                    QueryTextDisplay = q,
                    IcoPath = IconPath,
                    Title = $"{website?.Snippet}: {terms}",
                    SubTitle = q,
                    ToolTipData = new ToolTipData("Bang", $"Search {website?.Snippet}"),
                    ContextData = q,
                    Score = 100,
                };
            }

            IEnumerable<Suggestion>? AutoComplete(string q)
            {
                try
                {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                    return DuckDuckGoClient.AutoCompleteAsync(q).Result;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
                }
                catch (Exception ex)
                {
                    Log.Exception("AutoComplete failed.", ex, GetType());
                }

                return null;
            }

            Suggestion? GetSnippet(string q)
            {
                try
                {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                    return DuckDuckGoClient.GetSnippetAsync(q).Result;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
                }
                catch (Exception ex)
                {
                    Log.Exception("GetSnippet failed.", ex, GetType());
                }

                return null;
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
            if (selectedResult?.ContextData is Suggestion suggestion)
            {
                var arguments = DuckDuckGoClient.GetSearchUrl(suggestion.Phrase);

                if (suggestion.Snippet != null)
                {
                    return
                    [
                        new ContextMenuResult
                        {
                            PluginName = Name,
                            Title = "Open website (Ctrl+Enter)",
                            FontFamily = "Segoe MDL2 Assets",
                            Glyph = "\xE774", // E774 => Symbol: Globe
                            AcceleratorKey = Key.Enter,
                            AcceleratorModifiers = ModifierKeys.Control,
                            Action = _ => OpenInBrowser(arguments),
                        },
                    ];
                }
                else
                {
                    return
                    [
                        new ContextMenuResult
                        {
                            PluginName = Name,
                            Title = "Open website (Enter)",
                            FontFamily = "Segoe MDL2 Assets",
                            Glyph = "\xF6FA", // F6FA => Symbol: WebSearch
                            AcceleratorKey = Key.Enter,
                            Action = _ => OpenInBrowser(arguments),
                        },
                    ];
                }
            }

            if (selectedResult?.ContextData is string q)
            {
                var arguments = DuckDuckGoClient.GetSearchUrl(q);

                return
                [
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Open website (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xF6FA", // F6FA => Symbol: WebSearch
                        AcceleratorKey = Key.Enter,
                        Action = _ => OpenInBrowser(arguments),
                    },
                ];
            }

            return [];
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

        private void UpdateIconPath(Theme theme) => IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite ? "Images/bang.light.png" : "Images/bang.dark.png";

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
    }
}

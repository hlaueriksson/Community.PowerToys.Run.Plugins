using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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
    public class Main : IPlugin, IDelayedExecutionPlugin, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            Log.Info($"Ctor", GetType());

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://duckduckgo.com"),
                Timeout = TimeSpan.FromSeconds(5),
            };
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Community.PowerToys.Run.Plugin.Bang");
        }

        internal Main(HttpClient httpClient)
        {
            HttpClient = httpClient;
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
        public string Description => "Search websites with DuckDuckGo !Bang";

        private HttpClient HttpClient { get; }

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

            var q = query.Search;

            if (IsPhraseOnly(q))
            {
                var suggestions = AutoComplete(Bangify(q));

                return suggestions?.Select(GetResultFromSuggestion).ToList() ?? new List<Result>(0);
            }

            var result = GetResultFromQuery(Bangify(q));

            if (result != null)
            {
                return new List<Result> { result };
            }

            return new List<Result>(0);

            bool IsPhraseOnly(string q) => !q.Contains(' ', StringComparison.InvariantCulture);

            string Bangify(string q) => q.StartsWith('!') ? q : "!" + q;

            Result GetResultFromSuggestion(Suggestion suggestion) => new()
            {
                QueryTextDisplay = suggestion.Phrase + " ",
                IcoPath = IconPath,
                Title = suggestion.Snippet,
                SubTitle = suggestion.Phrase,
                ToolTipData = new ToolTipData("Bang", $"Search {suggestion.Snippet}"),
            };

            Result? GetResultFromQuery(string q)
            {
                var index = q.IndexOf(' ', StringComparison.InvariantCulture);
                var phrase = q.Substring(0, index);
                var terms = q.Substring(index + 1);

                var suggestions = AutoComplete(phrase);
                var suggestion = suggestions?.FirstOrDefault(x => x.Phrase == phrase);

                if (suggestion == null)
                {
                    return null;
                }

                var arguments = $"https://duckduckgo.com/?va=j&t=hc&q={UrlEncode(q)}";

                return new()
                {
                    QueryTextDisplay = q,
                    IcoPath = IconPath,
                    Title = $"{suggestion.Snippet}: {terms}",
                    SubTitle = q,
                    ToolTipData = new ToolTipData("Bang", $"Search {suggestion.Snippet}"),
                    ProgramArguments = arguments,
                    Action = _ =>
                    {
                        Log.Info("Query: " + q, GetType());

                        if (!Helper.OpenCommandInShell(DefaultBrowserInfo.Path, DefaultBrowserInfo.ArgumentsPattern, arguments))
                        {
                            Log.Error("Open default browser failed.", GetType());
                            Context?.API.ShowMsg($"Plugin: {Name}", "Open default browser failed.");
                            return false;
                        }

                        return true;
                    },
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
            return WebUtility.UrlEncode(q);
        }

        private void UpdateIconPath(Theme theme) => IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite ? "Images/bang.light.png" : "Images/bang.dark.png";

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);

        private IEnumerable<Suggestion>? AutoComplete(string q)
        {
            try
            {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                return HttpClient.GetFromJsonAsync<IEnumerable<Suggestion>>($"/ac/?q={UrlEncode(q)}&kl=wt-wt").Result;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
            }
            catch (Exception ex)
            {
                Log.Exception("AutoComplete failed.", ex, GetType());
            }

            return null;
        }
    }
}

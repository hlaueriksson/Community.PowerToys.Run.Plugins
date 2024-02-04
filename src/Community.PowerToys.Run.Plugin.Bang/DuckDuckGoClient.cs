using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Community.PowerToys.Run.Plugin.Bang.Models;
using LazyCache;

namespace Community.PowerToys.Run.Plugin.Bang
{
    /// <summary>
    /// DuckDuckGo API.
    /// </summary>
    public interface IDuckDuckGoClient
    {
        /// <summary>
        /// Gets suggestions.
        /// </summary>
        /// <param name="q">Search query.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IEnumerable<Suggestion>?> AutoCompleteAsync(string q);

        /// <summary>
        /// Gets snippet.
        /// </summary>
        /// <param name="q">Search query.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<Suggestion?> GetSnippetAsync(string q);

        /// <summary>
        /// Gets the search terms.
        /// </summary>
        /// <param name="q">Search query.</param>
        /// <returns>The terms.</returns>
        string GetSearchTerms(string q);

        /// <summary>
        /// Gets the search URL.
        /// </summary>
        /// <param name="q">Search query.</param>
        /// <returns>The URL.</returns>
        string GetSearchUrl(string q);
    }

    /// <inheritdoc/>
    public class DuckDuckGoClient : IDuckDuckGoClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuckDuckGoClient"/> class.
        /// </summary>
        /// <param name="cache">Client cache.</param>
        public DuckDuckGoClient(IAppCache cache)
        {
            Cache = cache;

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://duckduckgo.com"),
                Timeout = TimeSpan.FromSeconds(5),
            };
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Community.PowerToys.Run.Plugin.Bang");
        }

        internal DuckDuckGoClient(IAppCache cache, HttpClient httpClient)
        {
            Cache = cache;
            HttpClient = httpClient;
        }

        private IAppCache Cache { get; }

        private HttpClient HttpClient { get; }

        /// <inheritdoc/>
        public async Task<IEnumerable<Suggestion>?> AutoCompleteAsync(string q)
        {
            var result = await HttpClient.GetFromJsonAsync<IEnumerable<Suggestion>>($"/ac/?q={UrlEncode(q)}&kl=wt-wt").ConfigureAwait(false);

            foreach (var suggestion in result?.Where(x => x.Snippet != null) ?? [])
            {
                var bang = suggestion.Phrase;
                Cache.Add(bang, suggestion, DateTimeOffset.Now.AddDays(1));
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<Suggestion?> GetSnippetAsync(string q)
        {
            var bang = GetBang(q);
            return await Cache.GetAsync<Suggestion?>(bang).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public string GetSearchTerms(string q)
        {
            if (q == null)
            {
                return string.Empty;
            }

            var index = q.IndexOf(' ', StringComparison.Ordinal);

            return index != -1 ? q.Substring(index + 1) : string.Empty;
        }

        /// <inheritdoc/>
        public string GetSearchUrl(string q)
        {
            return $"https://duckduckgo.com/?t=h_&q={UrlEncode(q)}";
        }

        private static string UrlEncode(string q)
        {
            return WebUtility.UrlEncode(q);
        }

        private static string? GetBang(string? q)
        {
            if (q == null)
            {
                return q;
            }

            var index = q.IndexOf(' ', StringComparison.Ordinal);

            return index != -1 ? q.Substring(0, index) : q;
        }
    }
}

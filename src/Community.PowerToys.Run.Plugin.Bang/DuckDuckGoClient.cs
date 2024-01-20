using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Community.PowerToys.Run.Plugin.Bang.Models;

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
        public DuckDuckGoClient()
        {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://duckduckgo.com"),
                Timeout = TimeSpan.FromSeconds(5),
            };
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Community.PowerToys.Run.Plugin.Bang");
        }

        internal DuckDuckGoClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        private HttpClient HttpClient { get; }

        /// <inheritdoc/>
        public async Task<IEnumerable<Suggestion>?> AutoCompleteAsync(string q)
        {
            return await HttpClient.GetFromJsonAsync<IEnumerable<Suggestion>>($"/ac/?q={UrlEncode(q)}&kl=wt-wt").ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public string GetSearchUrl(string q)
        {
            return $"https://duckduckgo.com/?va=j&t=hc&q={UrlEncode(q)}";
        }

        private static string UrlEncode(string q)
        {
            return WebUtility.UrlEncode(q);
        }
    }
}

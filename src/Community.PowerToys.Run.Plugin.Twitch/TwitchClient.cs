using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Community.PowerToys.Run.Plugin.Twitch.Models;
using LazyCache;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Twitch
{
    /// <summary>
    /// Twitch API.
    /// </summary>
    public interface ITwitchClient
    {
        /// <summary>
        /// Gets auth token.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <seealso href="https://dev.twitch.tv/docs/authentication/getting-tokens-oauth#client-credentials-grant-flow"/>
        Task<TokenResponse?> GetAuthTokenAsync();

        /// <summary>
        /// Gets top games.
        /// </summary>
        /// <param name="page">The previous or next page of results.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <seealso href="https://dev.twitch.tv/docs/api/reference#get-top-games"/>
        Task<CategoriesResponse?> GetTopGamesAsync(Page page = Page.None);

        /// <summary>
        /// Search categories or games.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <param name="page">The previous or next page of results.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <seealso href="https://dev.twitch.tv/docs/api/reference#search-categories"/>
        Task<CategoriesResponse?> SearchCategoriesAsync(string query, Page page = Page.None);

        /// <summary>
        /// Search channels.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <param name="page">The previous or next page of results.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <seealso href="https://dev.twitch.tv/docs/api/reference#search-channels"/>
        Task<ChannelsResponse?> SearchChannelsAsync(string query, Page page = Page.None);

        /// <summary>
        /// Gets streams.
        /// </summary>
        /// <param name="gameId">Returns streams broadcasting a specified game ID.</param>
        /// <param name="page">The previous or next page of results.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <seealso href="https://dev.twitch.tv/docs/api/reference#get-streams"/>
        Task<StreamsResponse?> GetStreamsAsync(string gameId, Page page = Page.None);

        /// <summary>
        /// Gets category or game URL.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>The URL.</returns>
        string GetUrl(CategoryData category);

        /// <summary>
        /// Gets channel URL.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The URL.</returns>
        string GetUrl(ChannelData channel);

        /// <summary>
        /// Gets stream URL.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The URL.</returns>
        string GetUrl(StreamData stream);
    }

    /// <inheritdoc/>
    public class TwitchClient : ITwitchClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwitchClient"/> class.
        /// </summary>
        /// <param name="settings">Plugin settings.</param>
        /// <param name="cache">Client cache.</param>
        public TwitchClient(TwitchSettings settings, IAppCache cache)
        {
            Settings = settings;
            Cache = cache;

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.twitch.tv"),
                Timeout = TimeSpan.FromSeconds(5),
            };
            HttpClient.DefaultRequestHeaders.Add("Client-Id", Settings.TwitchApiClientId);
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Community.PowerToys.Run.Plugin.Twitch");
        }

        internal TwitchClient(TwitchSettings settings, IAppCache cache, HttpClient httpClient)
        {
            Settings = settings;
            Cache = cache;
            HttpClient = httpClient;
        }

        private TwitchSettings Settings { get; }

        private IAppCache Cache { get; }

        private HttpClient HttpClient { get; }

        /// <inheritdoc/>
        public async Task<TokenResponse?> GetAuthTokenAsync()
        {
            const string uri = "https://id.twitch.tv/oauth2/token";

            var result = await Cache.GetAsync<TokenResponse?>(uri).ConfigureAwait(false);

            if (result != null)
            {
                return result;
            }

            if (string.IsNullOrEmpty(Settings.TwitchApiClientId))
            {
                Log.Error("TwitchApiClientId is invalid", GetType());
                return null;
            }

            if (string.IsNullOrEmpty(Settings.TwitchApiClientSecret))
            {
                Log.Error("TwitchApiClientSecret is invalid", GetType());
                return null;
            }

            var data = new Dictionary<string, string>
            {
                { "client_id", Settings.TwitchApiClientId },
                { "client_secret", Settings.TwitchApiClientSecret },
                { "grant_type", "client_credentials" },
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(data) };
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            result = await response.Content.ReadFromJsonAsync<TokenResponse>().ConfigureAwait(false);

            if (result != null)
            {
                Cache.Add(uri, result, DateTimeOffset.Now.AddDays(1));
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<CategoriesResponse?> GetTopGamesAsync(Page page = Page.None)
        {
            await SetAuthorizationAsync().ConfigureAwait(false);
            var uri = "/helix/games/top" + Query(Pagination(page), Parameter(Settings.TwitchApiParameterFirst));
            Log.Debug($"GetTopGamesAsync: {uri}", GetType());
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return CachePagination(await response.Content.ReadFromJsonAsync<CategoriesResponse>().ConfigureAwait(false));
        }

        /// <inheritdoc/>
        public async Task<CategoriesResponse?> SearchCategoriesAsync(string query, Page page = Page.None)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            // NOTE: not previous
            await SetAuthorizationAsync().ConfigureAwait(false);
            var uri = "/helix/search/categories" + Query(Parameter(query), Pagination(page), Parameter(Settings.TwitchApiParameterFirst));
            Log.Debug($"SearchCategoriesAsync: {uri}", GetType());
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return CachePagination(await response.Content.ReadFromJsonAsync<CategoriesResponse>().ConfigureAwait(false));
        }

        /// <inheritdoc/>
        public async Task<ChannelsResponse?> SearchChannelsAsync(string query, Page page = Page.None)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            // NOTE: not previous
            await SetAuthorizationAsync().ConfigureAwait(false);
            var uri = "helix/search/channels" + Query(Parameter(query), Pagination(page), Parameter(Settings.TwitchApiParameterFirst), Parameter(Settings.TwitchApiParameterLiveOnly));
            Log.Debug($"SearchChannelsAsync: {uri}", GetType());
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return CachePagination(await response.Content.ReadFromJsonAsync<ChannelsResponse>().ConfigureAwait(false));
        }

        /// <inheritdoc/>
        public async Task<StreamsResponse?> GetStreamsAsync(string gameId, Page page = Page.None)
        {
            if (string.IsNullOrWhiteSpace(gameId))
            {
                return null;
            }

            await SetAuthorizationAsync().ConfigureAwait(false);
            var uri = "/helix/streams" + Query(Parameter(gameId), Pagination(page), Parameter(Settings.TwitchApiParameterFirst), Parameter(Settings.TwitchApiParameterLiveOnly ? "live" : "all", "type"), Parameter(Settings.TwitchApiParameterLanguage));
            Log.Debug($"GetStreamsAsync: {uri}", GetType());
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return CachePagination(await response.Content.ReadFromJsonAsync<StreamsResponse>().ConfigureAwait(false));
        }

        /// <inheritdoc/>
        public string GetUrl(CategoryData category)
        {
            ArgumentNullException.ThrowIfNull(category);

            return $"https://www.twitch.tv/directory/category/{KebabCase(category.name)}";
        }

        /// <inheritdoc/>
        public string GetUrl(ChannelData channel)
        {
            ArgumentNullException.ThrowIfNull(channel);

            return $"https://www.twitch.tv/{channel.broadcaster_login}";
        }

        /// <inheritdoc/>
        public string GetUrl(StreamData stream)
        {
            ArgumentNullException.ThrowIfNull(stream);

            return $"https://www.twitch.tv/{stream.user_login}";
        }

        private static string? Query(params string?[] parameters)
        {
            if (parameters == null || parameters.Length == 0 || parameters.All(x => x == null))
            {
                return null;
            }

            return "?" + string.Join("&", parameters.Where(x => x != null));
        }

        private static string? Parameter(object? value, [CallerArgumentExpression(nameof(value))] string name = "")
        {
            if (value == null)
            {
                return null;
            }

            return $"{GetParameterName(name)}={UrlEncode(value)}";

            static string GetParameterName(string name) => name switch
            {
                var x when x.EndsWith(nameof(Settings.TwitchApiParameterFirst), StringComparison.Ordinal) => "first",
                var x when x.EndsWith(nameof(Settings.TwitchApiParameterLiveOnly), StringComparison.Ordinal) => "live_only",
                var x when x.EndsWith(nameof(Settings.TwitchApiParameterLanguage), StringComparison.Ordinal) => "language",
                "gameId" => "game_id",
                _ => name,
            };
        }

        private static string? UrlEncode(object value)
        {
            return WebUtility.UrlEncode(value.ToString());
        }

        private static string KebabCase(string value)
        {
            return value.ToLowerInvariant().Replace(" ", "-", StringComparison.Ordinal);
        }

        private T? CachePagination<T>(T? response)
            where T : IPaginationResponse
        {
            Cache.Add(nameof(Pagination), response?.pagination, DateTimeOffset.Now.AddDays(1));

            return response;
        }

        private string? Pagination(Page page)
        {
            var result = Cache.Get<Pagination>(nameof(Pagination));
            var cursor = result?.cursor;

            if (cursor == null)
            {
                return null;
            }

            return page switch
            {
                Page.Previous => $"before={cursor}",
                Page.Next => $"after={cursor}",
                _ => null,
            };
        }

        private async Task SetAuthorizationAsync()
        {
            var token = await GetAuthTokenAsync().ConfigureAwait(false);
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.access_token);
        }
    }
}

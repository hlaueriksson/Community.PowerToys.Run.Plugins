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
    // TODO:
    // https://dev.twitch.tv/docs/api/reference#get-followed-streams
    //
    // https://dev.twitch.tv/docs/api/guide#pagination
    // https://dev.twitch.tv/docs/api/guide#twitch-rate-limits
    // https://devstatus.twitch.tv/

    /// <summary>
    /// Twitch API.
    /// </summary>
    public interface ITwitchClient
    {
        /// <summary>
        /// Gets OAuth token.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <seealso href="https://dev.twitch.tv/docs/authentication/getting-tokens-oauth#client-credentials-grant-flow"/>
        Task<TokenResponse?> GetTokenAsync();

        /// <summary>
        /// Gets top games.
        /// </summary>
        /// <param name="after">Cursor for forward pagination.</param>
        /// <param name="before">Cursor for backward pagination.</param>
        /// <param name="first">Maximum number of objects to return.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <seealso href="https://dev.twitch.tv/docs/api/reference#get-top-games"/>
        Task<GamesResponse?> GetGamesAsync(string? after = null, string? before = null, int? first = null);

        /// <summary>
        /// Search games or categories.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <param name="after">Cursor for forward pagination.</param>
        /// <param name="first">Maximum number of objects to return.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <seealso href="https://dev.twitch.tv/docs/api/reference#search-categories"/>
        Task<GamesResponse?> SearchGamesAsync(string query, string? after = null, int? first = null);

        /// <summary>
        /// Search channels.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <param name="after">Cursor for forward pagination.</param>
        /// <param name="first">Maximum number of objects to return.</param>
        /// <param name="live_only">Filter results for live streams only.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <seealso href="https://dev.twitch.tv/docs/api/reference#search-channels"/>
#pragma warning disable CA1707 // Identifiers should not contain underscores
        Task<ChannelsResponse?> SearchChannelsAsync(string query, string? after = null, int? first = null, bool? live_only = null);
#pragma warning restore CA1707 // Identifiers should not contain underscores

        /// <summary>
        /// Gets top streams.
        /// </summary>
        /// <param name="after">Cursor for forward pagination.</param>
        /// <param name="before">Cursor for backward pagination.</param>
        /// <param name="first">Maximum number of objects to return.</param>
        /// <param name="game_id">Returns streams broadcasting a specified game ID.</param>
        /// <param name="language">Stream language.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <seealso href="https://dev.twitch.tv/docs/api/reference#get-streams"/>
#pragma warning disable CA1707 // Identifiers should not contain underscores
        Task<StreamsResponse?> GetStreamsAsync(string? after = null, string? before = null, int? first = null, string? game_id = null, string? language = null);
#pragma warning restore CA1707 // Identifiers should not contain underscores

        /// <summary>
        /// Gets game URL.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns>The URL.</returns>
        string GetUrl(GameData game);

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
        /// <param name="cache">Plugin cache.</param>
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
        public async Task<TokenResponse?> GetTokenAsync()
        {
            const string uri = "https://id.twitch.tv/oauth2/token";

            var result = await Cache.GetAsync<TokenResponse?>(uri).ConfigureAwait(false);

            if (result != null)
            {
                return result;
            }

            Log.Info("GetTokenAsync not cached", GetType());

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
        public async Task<GamesResponse?> GetGamesAsync(string? after = null, string? before = null, int? first = null)
        {
            var uri = "/helix/games/top" + Query(Parameter(after), Parameter(before), Parameter(first));

            Log.Info($"GetGamesAsync: {uri}", GetType());

            var result = await Cache.GetAsync<GamesResponse?>(uri).ConfigureAwait(false);

            if (result != null)
            {
                return result;
            }

            Log.Info("GetGamesAsync not cached", GetType());

            await SetAuthorizationAsync().ConfigureAwait(false);
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            result = await response.Content.ReadFromJsonAsync<GamesResponse>().ConfigureAwait(false);

            if (result != null)
            {
                Cache.Add(uri, result, DateTimeOffset.Now.AddMinutes(1));
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<GamesResponse?> SearchGamesAsync(string query, string? after = null, int? first = null)
        {
            await SetAuthorizationAsync().ConfigureAwait(false);
            var uri = "/helix/search/categories" + Query(Parameter(query), Parameter(after), Parameter(first));
            Log.Info($"SearchGamesAsync: {uri}", GetType());
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<GamesResponse>().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ChannelsResponse?> SearchChannelsAsync(string query, string? after = null, int? first = null, bool? live_only = null)
        {
            await SetAuthorizationAsync().ConfigureAwait(false);
            var uri = "helix/search/channels" + Query(Parameter(query), Parameter(after), Parameter(first), Parameter(live_only));
            Log.Info($"SearchChannelsAsync: {uri}", GetType());
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<ChannelsResponse>().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<StreamsResponse?> GetStreamsAsync(string? after = null, string? before = null, int? first = null, string? game_id = null, string? language = null)
        {
            await SetAuthorizationAsync().ConfigureAwait(false);
            var uri = "/helix/streams" + Query(Parameter(after), Parameter(before), Parameter(first), Parameter(game_id), Parameter(language));
            Log.Info($"GetStreamsAsync: {uri}", GetType());
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<StreamsResponse>().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public string GetUrl(GameData game)
        {
            ArgumentNullException.ThrowIfNull(game);

            return $"https://www.twitch.tv/directory/game/{UrlEncode(game.name)}";
        }

        /// <inheritdoc/>
        public string GetUrl(ChannelData channel)
        {
            ArgumentNullException.ThrowIfNull(channel);

            return $"https://www.twitch.tv/{UrlEncode(channel.broadcaster_login)}";
        }

        /// <inheritdoc/>
        public string GetUrl(StreamData stream)
        {
            ArgumentNullException.ThrowIfNull(stream);

            return $"https://www.twitch.tv/{UrlEncode(stream.user_login)}";
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

            return $"{name}={UrlEncode(value)}";
        }

        private static string? UrlEncode(object value)
        {
            return WebUtility.UrlEncode(value.ToString());
        }

        private async Task SetAuthorizationAsync()
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.access_token);
        }
    }
}

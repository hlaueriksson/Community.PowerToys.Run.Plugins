using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Community.PowerToys.Run.Plugin.DenCode.Models;

namespace Community.PowerToys.Run.Plugin.DenCode
{
    /// <summary>
    /// DenCode API.
    /// </summary>
    public interface IDenCodeClient
    {
        /// <summary>
        /// Encodes/decodes a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Encoding/decoding results.</returns>
        Task<DenCodeResponse?> DenCodeAsync(string value);

        /// <summary>
        /// Encodes/decodes a value.
        /// </summary>
        /// <param name="method">The encoding/decoding method.</param>
        /// <param name="value">The value.</param>
        /// <returns>Encoding/decoding results.</returns>
        Task<DenCodeResponse?> DenCodeAsync(DenCodeMethod method, string value);

        /// <summary>
        /// Gets DenCode URL.
        /// </summary>
        /// <param name="method">The encoding/decoding method.</param>
        /// <returns>The URL.</returns>
        string GetUrl(DenCodeMethod method);

        /// <summary>
        /// Gets DenCode URL.
        /// </summary>
        /// <param name="data">The context data.</param>
        /// <returns>The URL.</returns>
        string GetUrl(DenCodeContextData data);
    }

    /// <inheritdoc/>
    public class DenCodeClient : IDenCodeClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DenCodeClient"/> class.
        /// </summary>
        public DenCodeClient()
        {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://dencode.com"),
                Timeout = TimeSpan.FromSeconds(5),
            };
            HttpClient.DefaultRequestHeaders.Add("X-Application-Id", "Community.PowerToys.Run.Plugin.DenCode");
        }

        internal DenCodeClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        private HttpClient HttpClient { get; }

        /// <inheritdoc/>
        public async Task<DenCodeResponse?> DenCodeAsync(string value)
        {
            var request = JsonSerializer.Deserialize<DenCodeRequest>(Constants.AllRequest);
            request!.value = value;
            request!.tz = GetIanaTimeZoneId(TimeZoneInfo.Local, "UTC");
            var response = await HttpClient.PostAsJsonAsync("dencode", request).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<DenCodeResponse>().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<DenCodeResponse?> DenCodeAsync(DenCodeMethod method, string value)
        {
            ArgumentNullException.ThrowIfNull(method);

            var request = JsonSerializer.Deserialize<DenCodeRequest>(Constants.AllRequest);
            request!.type = method.GetRequestType();
            request!.method = method.Key;
            request!.value = value;
            request!.tz = GetIanaTimeZoneId(TimeZoneInfo.Local, "UTC");
            var response = await HttpClient.PostAsJsonAsync("dencode", request).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<DenCodeResponse>().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public string GetUrl(DenCodeMethod method)
        {
            ArgumentNullException.ThrowIfNull(method);

            if (method.Method == Constants.AllMethod)
            {
                return "https://dencode.com";
            }

            var slug = method.Key.Replace('.', '/') ?? string.Empty;
            return $"https://dencode.com/{slug}";
        }

        /// <inheritdoc/>
        public string GetUrl(DenCodeContextData data)
        {
            ArgumentNullException.ThrowIfNull(data);

            var slug = data.Method?.Key.Replace('.', '/') ?? string.Empty;
            return $"https://dencode.com/{slug}?v={UrlEncode(data.Value)}";
        }

        private static string UrlEncode(string q)
        {
            return Uri.EscapeDataString(q);
        }

        /// <summary>
        /// Gets IANA ID from TimeZoneInfo.
        /// </summary>
        /// <param name="timeZoneInfo">The TimeZoneInfo instance.</param>
        /// <param name="defaultIanaId">The default IANA ID.</param>
        /// <returns>IANA ID.</returns>
        private static string GetIanaTimeZoneId(TimeZoneInfo timeZoneInfo, string defaultIanaId)
        {
            if (timeZoneInfo.HasIanaId)
            {
                return timeZoneInfo.Id;
            }

            if (TimeZoneInfo.TryConvertWindowsIdToIanaId(timeZoneInfo.Id, out string? ianaId))
            {
                return ianaId;
            }

            return defaultIanaId;
        }
    }
}

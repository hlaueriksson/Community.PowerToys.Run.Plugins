using System.Net.Http;
using System.Text.Json;
using Community.PowerToys.Run.Plugin.Dice.Models;

namespace Community.PowerToys.Run.Plugin.Dice
{
    /// <summary>
    /// Rolz API.
    /// </summary>
    public interface IRolzClient
    {
        /// <summary>
        /// Rolls dice.
        /// </summary>
        /// <param name="expression">The dice expression.</param>
        /// <returns>Roll results.</returns>
        Task<Roll?> RollAsync(string expression);
    }

    /// <inheritdoc/>
    public class RolzClient : IRolzClient
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Initializes a new instance of the <see cref="RolzClient"/> class.
        /// </summary>
        public RolzClient()
        {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://rolz.org/api/"),
                Timeout = TimeSpan.FromSeconds(5),
            };
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Community.PowerToys.Run.Plugin.Dice");
        }

        internal RolzClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        private HttpClient HttpClient { get; }

        /// <inheritdoc/>
        public async Task<Roll?> RollAsync(string expression)
        {
            var content = await HttpClient.GetStringAsync($"?{expression.Clean()}.json").ConfigureAwait(false);

            if (string.IsNullOrEmpty(content) || content.Contains("dice code error", StringComparison.InvariantCulture))
            {
                return null;
            }

            return JsonSerializer.Deserialize<Roll>(content, _jsonSerializerOptions);
        }
    }
}

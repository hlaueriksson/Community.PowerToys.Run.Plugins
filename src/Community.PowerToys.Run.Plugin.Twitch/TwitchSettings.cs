using Microsoft.PowerToys.Settings.UI.Library;

namespace Community.PowerToys.Run.Plugin.Twitch
{
    /// <summary>
    /// Plugin settings.
    /// </summary>
    public class TwitchSettings
    {
        /// <summary>
        /// Application Client ID.
        /// </summary>
        public string? TwitchApiClientId { get; set; }

        /// <summary>
        /// Application Client Secret.
        /// </summary>
        public string? TwitchApiClientSecret { get; set; }

        internal IEnumerable<PluginAdditionalOption> GetAdditionalOptions()
        {
            return new List<PluginAdditionalOption>()
            {
                new()
                {
                    Key = nameof(TwitchApiClientId),
                    DisplayLabel = "Twitch API Client ID",
                    DisplayDescription = "Passed to authorization endpoints to identify your application.",
                    PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                    TextValue = TwitchApiClientId,
                },
                new()
                {
                    Key = nameof(TwitchApiClientSecret),
                    DisplayLabel = "Twitch API Client Secret",
                    DisplayDescription = "Passed to the token exchange endpoints to obtain a token.",
                    PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                    TextValue = TwitchApiClientSecret,
                },
            };
        }

        internal void SetAdditionalOptions(IEnumerable<PluginAdditionalOption> additionalOptions)
        {
            ArgumentNullException.ThrowIfNull(additionalOptions);

            var options = additionalOptions.ToList();
            TwitchApiClientId = options.Find(x => x.Key == nameof(TwitchApiClientId))?.TextValue;
            TwitchApiClientSecret = options.Find(x => x.Key == nameof(TwitchApiClientSecret))?.TextValue;
        }
    }
}

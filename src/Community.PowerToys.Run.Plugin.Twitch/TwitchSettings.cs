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

        /// <summary>
        /// The maximum number of items to return per page in the response. The minimum page size is 1 item per page and the maximum is 100 items per page. The default is 20.
        /// </summary>
        public int TwitchApiParameterFirst { get; set; } = 20;

        /// <summary>
        /// A language code used to filter the list of streams. Specify the language using an ISO 639-1 two-letter language code.
        /// </summary>
        public string TwitchApiParameterLanguage { get; set; } = "en";

        /// <summary>
        /// A Boolean value that determines whether the response includes only channels that are currently streaming live.
        /// </summary>
        public bool TwitchApiParameterLiveOnly { get; set; } = true;

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
                new()
                {
                    Key = nameof(TwitchApiParameterFirst),
                    DisplayLabel = "Items per page",
                    DisplayDescription = "Number of items to return per page.",
                    PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Numberbox,
                    NumberValue = TwitchApiParameterFirst,
                    NumberBoxMin = 1,
                    NumberBoxMax = 100,
                },
                new()
                {
                    Key = nameof(TwitchApiParameterLanguage),
                    DisplayLabel = "Language code",
                    DisplayDescription = "ISO 639-1 two-letter language code to filter streams.",
                    PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                    TextValue = TwitchApiParameterLanguage,
                },
                new()
                {
                    Key = nameof(TwitchApiParameterLiveOnly),
                    DisplayLabel = "Live",
                    DisplayDescription = "Only include channels that are currently streaming live.",
                    PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Checkbox,
                    Value = TwitchApiParameterLiveOnly,
                },
            };
        }

        internal void SetAdditionalOptions(IEnumerable<PluginAdditionalOption> additionalOptions)
        {
            ArgumentNullException.ThrowIfNull(additionalOptions);

            var options = additionalOptions.ToList();
            TwitchApiClientId = options.Find(x => x.Key == nameof(TwitchApiClientId))?.TextValue;
            TwitchApiClientSecret = options.Find(x => x.Key == nameof(TwitchApiClientSecret))?.TextValue;
            TwitchApiParameterFirst = (int)(options.Find(x => x.Key == nameof(TwitchApiParameterFirst))?.NumberValue ?? 20);
            TwitchApiParameterLanguage = options.Find(x => x.Key == nameof(TwitchApiParameterLanguage))?.TextValue ?? "en";
            TwitchApiParameterLiveOnly = options.Find(x => x.Key == nameof(TwitchApiParameterLiveOnly))?.Value ?? true;
        }

        internal bool HasValidTwitchApiCredentials()
        {
            return !string.IsNullOrWhiteSpace(TwitchApiClientId) && !string.IsNullOrWhiteSpace(TwitchApiClientSecret);
        }
    }
}

using Community.PowerToys.Run.Plugin.Dice.Models;
using Microsoft.PowerToys.Settings.UI.Library;

namespace Community.PowerToys.Run.Plugin.Dice
{
    /// <summary>
    /// Plugin settings.
    /// </summary>
    public class DiceSettings
    {
        /// <summary>
        /// Roll options.
        /// </summary>
        public List<RollOption> RollOptions
        {
            get;
            set => field = value?.Count > 0 ? value : RollOption.Defaults;
        }

        = RollOption.Defaults;

        internal IEnumerable<PluginAdditionalOption> GetAdditionalOptions()
        {
            foreach (var (option, index) in RollOptions.Select((option, index) => (option, index)))
            {
                yield return new()
                {
                    Key = nameof(RollOptions) + index,
                    DisplayLabel = $"Roll Option: {index}",
                    DisplayDescription = "Predefined roll expression",
                    PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                    TextValue = (string)option,
                };
            }

            yield return new()
            {
                Key = nameof(RollOptions) + RollOptions.Count,
                DisplayLabel = $"Roll Option: {RollOptions.Count}",
                DisplayDescription = "Predefined roll expression",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = string.Empty,
            };
        }

        internal void SetAdditionalOptions(IEnumerable<PluginAdditionalOption> additionalOptions)
        {
            ArgumentNullException.ThrowIfNull(additionalOptions);

            var options = additionalOptions
                .Where(x => x.Key.StartsWith(nameof(RollOptions), StringComparison.Ordinal))
                .Select(x => (RollOption)x.TextValue)
                .Where(x => x != RollOption.Empty)
                .ToList();
            RollOptions = options.Count != 0 ? options : RollOption.Defaults;
        }
    }
}

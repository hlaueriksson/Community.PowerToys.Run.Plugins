namespace Community.PowerToys.Run.Plugin.Dice.Models
{
    /// <summary>
    /// Preconfigured roll expression.
    /// </summary>
    public class RollOption
    {
        public static readonly List<RollOption> Defaults =
        [
            new RollOption { Expression = "d20", Description = "Twenty sided die" },
            new RollOption { Expression = "d12", Description = "Twelve sided die" },
            new RollOption { Expression = "d10", Description = "Ten sided die" },
            new RollOption { Expression = "d6", Description = "Six sided die" },
            new RollOption { Expression = "d4", Description = "Four sided die" },
        ];

        public static readonly RollOption Empty = new();

        private const string Separator = ";";

        /// <summary>
        /// Roll expression.
        /// </summary>
        public string? Expression { get; set; }

        /// <summary>
        /// Roll description.
        /// </summary>
        public string? Description { get; set; }

        public static implicit operator string(RollOption value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.Expression?.Trim() + Separator + value.Description?.Trim();
        }

        public static implicit operator RollOption(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return RollOption.Empty;
            }

            var tokens = value.Split(Separator);
            return new RollOption { Expression = tokens?.FirstOrDefault()?.Trim(), Description = tokens?.LastOrDefault()?.Trim() };
        }
    }
}

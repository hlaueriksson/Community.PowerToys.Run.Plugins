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
        private const string QuotationMark = "\"";

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

            if (value.Expression?.Contains(Separator, StringComparison.Ordinal) == true || value.Description?.Contains(Separator, StringComparison.Ordinal) == true)
            {
                return QuotationMark + value.Expression?.Trim() + QuotationMark + Separator + QuotationMark + value.Description?.Trim() + QuotationMark;
            }

            return value.Expression?.Trim() + Separator + value.Description?.Trim();
        }

        public static implicit operator RollOption(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return RollOption.Empty;
            }

            if (value.Contains(QuotationMark, StringComparison.Ordinal))
            {
                var quotes = value.Split(QuotationMark, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                return new RollOption { Expression = quotes?.FirstOrDefault(), Description = quotes?.LastOrDefault() };
            }

            var tokens = value.Split(Separator, StringSplitOptions.TrimEntries);
            return new RollOption { Expression = tokens?.FirstOrDefault(), Description = tokens?.LastOrDefault() };
        }
    }
}

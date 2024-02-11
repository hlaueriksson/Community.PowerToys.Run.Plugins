using System.Text.RegularExpressions;

namespace Community.PowerToys.Run.Plugin.Dice
{
    internal static partial class Extensions
    {
        public static string? Clean(this string? value)
        {
            if (value != null)
            {
                return WhiteSpaceRegex().Replace(value, string.Empty);
            }

            return value;
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhiteSpaceRegex();
    }
}

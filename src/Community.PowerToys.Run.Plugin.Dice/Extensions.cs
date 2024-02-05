using System.Text.RegularExpressions;
using System.Windows;

namespace Community.PowerToys.Run.Plugin.Dice
{
    internal static partial class Extensions
    {
        public static bool CopyToClipboard(this string? value)
        {
            if (value != null)
            {
                Clipboard.SetText(value);
            }

            return true;
        }

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

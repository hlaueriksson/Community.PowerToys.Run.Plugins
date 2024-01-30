using System.Windows;

namespace Community.PowerToys.Run.Plugin.Dice
{
    internal static class Extensions
    {
        public static bool CopyToClipboard(this string? value)
        {
            if (value != null)
            {
                Clipboard.SetText(value);
            }

            return true;
        }
    }
}
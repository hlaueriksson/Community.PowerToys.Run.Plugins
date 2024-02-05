using System.Text.Json;
using System.Windows;
using Community.PowerToys.Run.Plugin.Need.Models;

namespace Community.PowerToys.Run.Plugin.Need
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

        public static string ToJson(this Record record)
        {
            return JsonSerializer.Serialize(record);
        }
    }
}

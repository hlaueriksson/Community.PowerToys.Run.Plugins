using System.Text.Json;
using System.Windows;
using Community.PowerToys.Run.Plugin.DenCode.Models;

namespace Community.PowerToys.Run.Plugin.DenCode
{
    internal static class Extensions
    {
        public static Dictionary<string, DenCodeMethod> GetDenCodeMethods(this string json)
        {
            return JsonSerializer.Deserialize<Dictionary<string, DenCodeMethod>>(json) ?? [];
        }

        public static Dictionary<string, DenCodeMethod> GetDenCodeLabels(this Dictionary<string, DenCodeMethod> methods)
        {
            var result = new Dictionary<string, DenCodeMethod>();

            foreach (var method in methods.Values)
            {
                foreach (var label in method.label.Keys)
                {
                    result[label] = method;
                }
            }

            return result;
        }

        public static string GetRequestType(this DenCodeMethod method)
        {
            return method.Key.Substring(0, method.Key.IndexOf('.', StringComparison.Ordinal));
        }

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

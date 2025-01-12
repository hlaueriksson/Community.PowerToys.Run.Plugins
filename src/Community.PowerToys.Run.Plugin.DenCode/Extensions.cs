using System.Text.Json;
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
                foreach (var label in method.Label.Keys)
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

        public static bool IsRoot(this DenCodeMethod method)
        {
            return method.Key.Equals("all.all", StringComparison.Ordinal);
        }

        public static bool IsBranch(this DenCodeMethod method)
        {
            return method.Key.Contains(".all", StringComparison.Ordinal);
        }
    }
}

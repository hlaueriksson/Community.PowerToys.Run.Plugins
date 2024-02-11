using System.Text.Json;
using Community.PowerToys.Run.Plugin.Need.Models;

namespace Community.PowerToys.Run.Plugin.Need
{
    internal static class Extensions
    {
        public static string ToJson(this Record record)
        {
            return JsonSerializer.Serialize(record);
        }
    }
}

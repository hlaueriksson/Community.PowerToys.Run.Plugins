using System.Text.Json;

namespace Community.PowerToys.Run.Plugin.DenCode.Models
{
    public class DenCodeContextData
    {
        public string Value { get; set; }

        public KeyValuePair<string, JsonElement> Result { get; set; }

        public DenCodeMethod? Method { get; set; }
    }
}

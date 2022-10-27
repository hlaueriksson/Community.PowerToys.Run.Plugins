using System.Text.Json;

namespace Community.PowerToys.Run.Plugin.DenCode.Models
{
    public class DenCodeResponse
    {
        public int statusCode { get; set; }

        public string[] messages { get; set; }

        public string redirectUrl { get; set; }

        public Dictionary<string, JsonElement> response { get; set; }
    }
}

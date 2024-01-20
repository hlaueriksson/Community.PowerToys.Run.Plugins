namespace Community.PowerToys.Run.Plugin.DenCode.Models
{
    public class DenCodeRequest
    {
        public string type { get; set; }

        public string method { get; set; }

        public string value { get; set; }

        public string oe { get; set; }

        public string nl { get; set; }

        public string tz { get; set; }

        public Dictionary<string, string> options { get; set; } = [];
    }
}

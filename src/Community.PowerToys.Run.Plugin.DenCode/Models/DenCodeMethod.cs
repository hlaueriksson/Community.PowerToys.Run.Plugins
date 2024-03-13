namespace Community.PowerToys.Run.Plugin.DenCode.Models
{
    public class DenCodeMethod
    {
        public string Key { get; set; }

        public string Method { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Tooltip { get; set; }

        public Dictionary<string, string> Label { get; set; } = [];
    }
}

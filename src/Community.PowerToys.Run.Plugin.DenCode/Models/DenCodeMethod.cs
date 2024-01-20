namespace Community.PowerToys.Run.Plugin.DenCode.Models
{
    public class DenCodeMethod
    {
        public string Key { get; set; }

        // config
        public bool useOe { get; set; }

        public bool useNl { get; set; }

        public bool useTz { get; set; }

        public bool hasEncoded { get; set; }

        public bool hasDecoded { get; set; }

        // messages
        public string method { get; set; }

        public string title { get; set; }

        public string desc { get; set; }

        public string tooltip { get; set; }

        public Dictionary<string, string> label { get; set; } = [];
    }
}

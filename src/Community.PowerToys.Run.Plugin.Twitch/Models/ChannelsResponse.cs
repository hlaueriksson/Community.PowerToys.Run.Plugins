namespace Community.PowerToys.Run.Plugin.Twitch.Models
{
    public class ChannelsResponse : IPaginationResponse
    {
        public ChannelData[] data { get; set; }

        public Pagination pagination { get; set; }
    }

    public class ChannelData
    {
        public string broadcaster_language { get; set; }

        public string broadcaster_login { get; set; }

        public string display_name { get; set; }

        public string game_id { get; set; }

        public string game_name { get; set; }

        public string id { get; set; }

        public bool is_live { get; set; }

        public string[] tags { get; set; }

        public string title { get; set; }

        public string started_at { get; set; }
    }
}

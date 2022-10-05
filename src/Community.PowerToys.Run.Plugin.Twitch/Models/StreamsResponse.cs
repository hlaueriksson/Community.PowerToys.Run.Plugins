namespace Community.PowerToys.Run.Plugin.Twitch.Models
{
    public class StreamsResponse
    {
        public StreamData[] data { get; set; }

        public Pagination pagination { get; set; }
    }

    public class StreamData
    {
        public string id { get; set; }

        public string user_id { get; set; }

        public string user_login { get; set; }

        public string user_name { get; set; }

        public string game_id { get; set; }

        public string game_name { get; set; }

        public string type { get; set; }

        public string title { get; set; }

        public int viewer_count { get; set; }

        public DateTime started_at { get; set; }

        public string language { get; set; }

        public string thumbnail_url { get; set; }

        public string[] tag_ids { get; set; }

        public bool is_mature { get; set; }
    }
}

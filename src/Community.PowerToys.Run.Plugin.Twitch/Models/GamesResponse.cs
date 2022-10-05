namespace Community.PowerToys.Run.Plugin.Twitch.Models
{
    public class GamesResponse
    {
        public GameData[] data { get; set; }

        public Pagination pagination { get; set; }
    }

    public class GameData
    {
        public string id { get; set; }

        public string name { get; set; }

        public string box_art_url { get; set; }
    }
}

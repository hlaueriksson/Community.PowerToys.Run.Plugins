namespace Community.PowerToys.Run.Plugin.Twitch.Models
{
    public class CategoriesResponse : IPaginationResponse
    {
        public CategoryData[] data { get; set; }

        public Pagination pagination { get; set; }
    }

    public class CategoryData
    {
        public string id { get; set; }

        public string name { get; set; }

        public string igdb_id { get; set; }
    }
}

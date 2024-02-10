namespace Community.PowerToys.Run.Plugin.Twitch.Models
{
    public enum Page
    {
        None,
        Previous,
        Next,
    }

    public interface IPaginationResponse
    {
        public Pagination pagination { get; }
    }

    public class Pagination
    {
        public string cursor { get; set; }
    }
}

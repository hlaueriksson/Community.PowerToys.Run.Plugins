namespace Community.PowerToys.Run.Plugin.Twitch.Models
{
    public class TokenResponse
    {
        public string access_token { get; set; }

        public int expires_in { get; set; } // ~58 days

        public string token_type { get; set; }
    }
}

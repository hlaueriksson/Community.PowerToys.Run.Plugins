using FluentAssertions;
using LazyCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;

namespace Community.PowerToys.Run.Plugin.Twitch.UnitTests
{
    [TestClass]
    public class TwitchClientTests
    {
        private TwitchClient subject;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "https://id.twitch.tv/oauth2/token")
                .Respond("application/json", "{ \"access_token\": \"i610c3a6q5v8ikm7u6ezdhpa7otcwo\", \"expires_in\": 5332108, \"token_type\": \"bearer\" }");
            mockHttp.When("http://localhost/helix/games/top")
                .Respond("application/json", "{ \"data\": [ { \"id\": \"509658\", \"name\": \"Just Chatting\", \"box_art_url\": \"https://static-cdn.jtvnw.net/ttv-boxart/509658-{width}x{height}.jpg\" } ], \"pagination\": { \"cursor\": \"eyJzIjoyMCwiZCI6ZmFsc2UsInQiOnRydWV9\" } }");
            mockHttp.When("http://localhost/helix/search/categories?query=cs")
                .Respond("application/json", "{ \"data\": [ { \"box_art_url\": \"https://static-cdn.jtvnw.net/ttv-boxart/10710_IGDB-52x72.jpg\", \"id\": \"10710\", \"name\": \"Counter-Strike\" } ], \"pagination\": { \"cursor\": \"MjA=\" } }");
            mockHttp.When("http://localhost/helix/search/channels?query=cs")
                .Respond("application/json", "{ \"data\": [ { \"broadcaster_language\": \"en\", \"broadcaster_login\": \"csanford\", \"display_name\": \"csanford\", \"game_id\": \"0\", \"game_name\": \"\", \"id\": \"37815253\", \"is_live\": false, \"tag_ids\": [], \"thumbnail_url\": \"https://static-cdn.jtvnw.net/jtv_user_pictures/a28f9231-640b-4eae-bc7a-4d7fa203b20d-profile_image-300x300.png\", \"title\": \"u003c3\", \"started_at\": \"\" } ], \"pagination\": { \"cursor\": \"MjA=\" } }");
            mockHttp.When("http://localhost/helix/streams")
                .Respond("application/json", "{ \"data\": [ { \"id\": \"39754856567\", \"user_id\": \"156037856\", \"user_login\": \"fextralife\", \"user_name\": \"Fextralife\", \"game_id\": \"515025\", \"game_name\": \"Overwatch 2\", \"type\": \"live\", \"title\": \"[!DROPS ENABLED] !OW2 STILL GOING with CAS!!\", \"viewer_count\": 49167, \"started_at\": \"2022-10-10T11:40:36Z\", \"language\": \"en\", \"thumbnail_url\": \"https://static-cdn.jtvnw.net/previews-ttv/live_user_fextralife-{width}x{height}.jpg\", \"tag_ids\": [ \"6ea6bca4-4712-4ab9-a906-e3336a9d8039\", \"c2542d6d-cd10-4532-919b-3d19f30a768b\" ], \"is_mature\": false } ], \"pagination\": { \"cursor\": \"eyJiIjp7IkN1cnNvciI6ImV5SnpJam94TWpNM056Y3VPREF6TWpJNE1qSTVNREVzSW1RaU9tWmhiSE5sTENKMElqcDBjblZsZlE9PSJ9LCJhIjp7IkN1cnNvciI6ImV5SnpJam94TXpRd05DNHdOVFE1TURRME9ESTBNRFFzSW1RaU9tWmhiSE5sTENKMElqcDBjblZsZlE9PSJ9fQ\" } }");
            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");

            var settings = new TwitchSettings { TwitchApiClientId = "foo", TwitchApiClientSecret = "bar" };
            subject = new TwitchClient(settings, new Mock<IAppCache>().Object, httpClient);
        }

        [TestMethod]
        public async Task GetTokenAsync()
        {
            var result = await subject.GetTokenAsync();
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task GetGamesAsync()
        {
            var result = await subject.GetGamesAsync();
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task SearchGamesAsync()
        {
            var result = await subject.SearchGamesAsync("cs");
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task SearchChannelsAsync()
        {
            var result = await subject.SearchChannelsAsync("cs");
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task GetStreamsAsync()
        {
            var result = await subject.GetStreamsAsync();
            result.Should().NotBeNull();
        }
    }
}

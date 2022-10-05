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
            mockHttp.When("https://id.twitch.tv/oauth2/token")
                .Respond("application/json", "{}");
            mockHttp.When("http://localhost/helix/games/top")
                .Respond("application/json", "{}");
            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");

            subject = new TwitchClient(new TwitchSettings(), new Mock<IAppCache>().Object, httpClient);
        }

        [TestMethod]
        public async Task GetGamesAsync()
        {
            var result = await subject.GetGamesAsync();
            result.Should().NotBeNull();

            result = await subject.GetGamesAsync(after: "foo", first: 100);
            result.Should().NotBeNull();
        }
    }
}

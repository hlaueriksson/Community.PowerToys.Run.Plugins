using FluentAssertions;
using LazyCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Community.PowerToys.Run.Plugin.Twitch.UnitTests
{
    [TestClass]
    [Ignore("Explicit")]
    public class TwitchClientIntegrationTests
    {
        private TwitchClient _subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var settings = new TwitchSettings { TwitchApiClientId = "SECRET", TwitchApiClientSecret = "SECRET" };
            _subject = new TwitchClient(settings, new CachingService());
        }

        [TestMethod]
        public async Task GetTokenAsync()
        {
            var result = await _subject.GetAuthTokenAsync();
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task GetGamesAsync()
        {
            var result = await _subject.GetTopGamesAsync();
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task SearchGamesAsync()
        {
            var result = await _subject.SearchCategoriesAsync("cs");
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task SearchChannelsAsync()
        {
            var result = await _subject.SearchChannelsAsync("cs");
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task GetStreamsAsync()
        {
            var result = await _subject.GetStreamsAsync("32399");
            result.Should().NotBeNull();
        }
    }
}

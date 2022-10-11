using Community.PowerToys.Run.Plugin.Twitch.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Twitch.UnitTests
{
    [TestClass]
    public class MainTests
    {
        private Main subject;

        [TestInitialize]
        public void TestInitialize()
        {
            var mock = new Mock<ITwitchClient>();
            mock.Setup(x => x.GetGamesAsync(null, null, 100)).ReturnsAsync(GamesResponse());
            mock.Setup(x => x.SearchGamesAsync("cs", null, 100)).ReturnsAsync(GamesResponse());
            mock.Setup(x => x.SearchChannelsAsync("cs", null, 100, true)).ReturnsAsync(ChannelsResponse());
            mock.Setup(x => x.GetStreamsAsync(null, null, 100, "1", "en")).ReturnsAsync(StreamsResponse());

            subject = new Main(new TwitchSettings(), mock.Object);

            GamesResponse GamesResponse() => new GamesResponse
            {
                data = new[] { new GameData { id = "1", name = "CS" } },
                pagination = new Pagination { cursor = "abc" }
            };

            ChannelsResponse ChannelsResponse() => new ChannelsResponse()
            {
                data = new[] { new ChannelData { id = "1", title = "CS", display_name = "CS", broadcaster_login = "cs" } },
                pagination = new Pagination { cursor = "abc" }
            };

            StreamsResponse StreamsResponse() => new StreamsResponse
            {
                data = new[] { new StreamData { id = "1", title = "CS", type = "live", user_login = "cs" } },
                pagination = new Pagination { cursor = "abc" }
            };
        }

        [TestMethod]
        public void Query_without_delayedExecution_should_return_empty_result()
        {
            subject.Query(new(""))
                .Should().BeEmpty();

            subject.Query(new(""), false)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Query_with_game_query_should_return_top_games()
        {
            subject.Query(new("game"), true)
                .Should().NotBeEmpty();
        }

        [TestMethod]
        public void Query_with_game_query_should_return_games_from_search()
        {
            subject.Query(new("game cs"), true)
                .Should().NotBeEmpty();
        }

        [TestMethod]
        public void Query_with_channel_query_should_return_channels_from_search()
        {
            subject.Query(new("channel cs"), true)
                .Should().NotBeEmpty();
        }

        [TestMethod]
        public void Query_with_stream_query_should_return_streams_for_game()
        {
            subject.Query(new("stream game_id=1"), true)
                .Should().NotBeEmpty();
        }

        [TestMethod]
        public void Query_properties()
        {
            Query.TermSeparator.Should().Be(" ");
            Query.ActionKeywordSeparator.Should().Be(";");
            Query.GlobalPluginWildcardSign.Should().Be("*");

            var subject = new Query("a b c d e f")
            {
                ActionKeyword = "a"
            };

            subject.RawQuery.Should().Be("a b c d e f");
            subject.Search.Should().Be("b c d e f");
            subject.Terms.Should().BeEquivalentTo(new[] { "b", "c", "d", "e", "f" });
            subject.FirstSearch.Should().Be("c");
            subject.SecondToEndSearch.Should().Be("d e f");
            subject.SecondSearch.Should().Be("d");
            subject.ThirdSearch.Should().Be("e");
        }
    }
}

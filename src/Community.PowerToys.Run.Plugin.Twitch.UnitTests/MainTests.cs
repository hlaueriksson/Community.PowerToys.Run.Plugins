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
        private Main _subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var mock = new Mock<ITwitchClient>();
            mock.Setup(x => x.GetTopGamesAsync(Page.None)).ReturnsAsync(GamesResponse());
            mock.Setup(x => x.SearchCategoriesAsync("cs", Page.None)).ReturnsAsync(GamesResponse());
            mock.Setup(x => x.SearchChannelsAsync("cs", Page.None)).ReturnsAsync(ChannelsResponse());
            mock.Setup(x => x.GetStreamsAsync("1", Page.None)).ReturnsAsync(StreamsResponse());

            _subject = new Main(new TwitchSettings { TwitchApiClientId = "foo", TwitchApiClientSecret = "bar" }, mock.Object);

            CategoriesResponse GamesResponse() => new()
            {
                data = [new CategoryData { id = "1", name = "CS" }],
                pagination = new Pagination { cursor = "abc" }
            };

            ChannelsResponse ChannelsResponse() => new()
            {
                data = [new ChannelData { id = "1", title = "CS", display_name = "CS", broadcaster_login = "cs" }],
                pagination = new Pagination { cursor = "abc" }
            };

            StreamsResponse StreamsResponse() => new()
            {
                data = [new StreamData { id = "1", title = "CS", type = "live", user_login = "cs" }],
                pagination = new Pagination { cursor = "abc" }
            };
        }

        [TestMethod]
        public void Query_without_delayedExecution_should_return_empty_result()
        {
            _subject.Query(new(""))
                .Should().BeEmpty();

            _subject.Query(new(""), false)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Empty_query_should_return_start_commands()
        {
            _subject.Query(new(""), true)
                .Should().BeEquivalentTo(new[]
                {
                    new Result { Title = "Top games", SubTitle = "Gets information about all broadcasts on Twitch.", IcoPath = @"Images\twitch.games.png" },
                    new Result { Title = "Search channels", SubTitle = "Gets the channels that match the specified query and have streamed content within the past 6 months.", IcoPath = @"Images\twitch.png" },
                    new Result { Title = "Search categories", SubTitle = "Gets the games or categories that match the specified query.", IcoPath = @"Images\twitch.png" },
                });
        }

        [TestMethod]
        public void Query_should_return_search_commands()
        {
            _subject.Query(new("foo"), true)
                .Should().BeEquivalentTo(new[]
                {
                    new Result { Title = "Search channels: foo", SubTitle = "Gets the channels that match the specified query and have streamed content within the past 6 months.", IcoPath = @"Images\twitch.png" },
                    new Result { Title = "Search categories: foo", SubTitle = "Gets the games or categories that match the specified query.", IcoPath = @"Images\twitch.png" },
                });
        }

        [TestMethod]
        public void Games_query_should_return_top_games()
        {
            _subject.Query(new("games"), true)
                .Should().NotBeEmpty();
        }

        [TestMethod]
        public void Categories_query_should_return_categories_from_search()
        {
            _subject.Query(new("categories cs"), true)
                .Should().NotBeEmpty();
        }

        [TestMethod]
        public void Channels_query_should_return_channels_from_search()
        {
            _subject.Query(new("channels cs"), true)
                .Should().NotBeEmpty();
        }

        [TestMethod]
        public void Streams_query_should_return_streams_for_game()
        {
            _subject.Query(new("streams 1"), true)
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
            subject.Terms.Should().BeEquivalentTo(["b", "c", "d", "e", "f"]);
            subject.FirstSearch.Should().Be("c");
            subject.SecondToEndSearch.Should().Be("d e f");
            subject.SecondSearch.Should().Be("d");
            subject.ThirdSearch.Should().Be("e");
        }
    }
}

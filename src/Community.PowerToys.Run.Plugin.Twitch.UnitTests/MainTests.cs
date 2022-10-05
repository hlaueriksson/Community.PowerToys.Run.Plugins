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
            subject = new Main(new TwitchSettings(), new Mock<ITwitchClient>().Object);
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
        public void Query_with_bang_query_should_return_default_Suggestion_result()
        {
            subject.Query(new("!"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "Wikipedia", SubTitle = "!w" } });
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

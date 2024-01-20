using Community.PowerToys.Run.Plugin.Bang.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Bang.UnitTests
{
    [TestClass]
    public class MainTests
    {
        private Main _subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var mock = new Mock<IDuckDuckGoClient>();
            mock.Setup(x => x.AutoCompleteAsync("!")).ReturnsAsync(new[] { new Suggestion { Phrase = "!w", Snippet = "Wikipedia" } });
            mock.Setup(x => x.AutoCompleteAsync("!gh")).ReturnsAsync(new[] { new Suggestion { Phrase = "!gh", Snippet = "GitHub" } });
            mock.Setup(x => x.AutoCompleteAsync("!unknown")).ReturnsAsync([]);
            mock.Setup(x => x.AutoCompleteAsync("!äx")).ReturnsAsync(new[] { new Suggestion { Phrase = "!äx", Snippet = "Levykauppa Äx" } });
            mock.Setup(x => x.GetSearchUrl("!gh PowerToys")).Returns("https://duckduckgo.com/?va=j&t=hc&q=!gh+PowerToys");

            _subject = new Main(mock.Object);
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
        public void Query_with_bang_query_should_return_default_Suggestion_result()
        {
            _subject.Query(new("!"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "Wikipedia", SubTitle = "!w" } });
        }

        [TestMethod]
        public void Query_with_bang_gh_query_should_return_Suggestion_result()
        {
            _subject.Query(new("!gh"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "GitHub", SubTitle = "!gh" } });
        }

        [TestMethod]
        public void Query_with_bang_unknown_query_should_return_empty_result()
        {
            _subject.Query(new("!unknown"), true)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Query_with_bang_gh_PowerToys_query_should_return_Query_result()
        {
            _subject.Query(new("!gh PowerToys"), true).Single()
                .Should().BeEquivalentTo(new Result { Title = "GitHub: PowerToys", SubTitle = "!gh PowerToys", ProgramArguments = "https://duckduckgo.com/?va=j&t=hc&q=!gh+PowerToys" });
        }

        [TestMethod]
        public void Query_should_URL_encode_q_parameter()
        {
            _subject.Query(new("!äx"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "Levykauppa Äx", SubTitle = "!äx" } });
        }

        [TestMethod]
        public void Query_should_add_bang_if_missing()
        {
            _subject.Query(new("gh"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "GitHub", SubTitle = "!gh" } });
        }
    }
}

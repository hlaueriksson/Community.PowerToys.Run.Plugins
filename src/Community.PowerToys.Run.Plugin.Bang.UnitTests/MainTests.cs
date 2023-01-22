using System.Net;
using System.Web;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Bang.UnitTests
{
    [TestClass]
    public class MainTests
    {
        private Main subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://localhost/ac/?q=!&kl=wt-wt")
                .Respond("application/json", "[ { \"phrase\": \"!w\", \"score\": 3015746, \"snippet\": \"Wikipedia\", \"image\": \"https://external-content.duckduckgo.com/i/en.wikipedia.org.ico?imgFallback=/watrcoolr/img/search-suggestions_default.png\" } ]");
            mockHttp.When("http://localhost/ac/?q=!gh&kl=wt-wt")
                .Respond("application/json", "[ { \"phrase\": \"!gh\", \"score\": 3015741, \"snippet\": \"GitHub\", \"image\": \"https://external-content.duckduckgo.com/i/github.com.ico?imgFallback=/watrcoolr/img/search-suggestions_default.png\" } ]");
            mockHttp.When("http://localhost/ac/?q=!unknown&kl=wt-wt")
                .Respond("application/json", "[]");
            mockHttp.When("http://localhost/ac/?q=!%C3%A4x&kl=wt-wt")
                .Respond("application/json", "[ { \"phrase\": \"!äx\", \"score\": 0, \"snippet\": \"Levykauppa Äx\", \"image\": \"https://external-content.duckduckgo.com/i/www.levykauppax.fi.ico?imgFallback=/watrcoolr/img/search-suggestions_default.png\" } ]");
            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");

            subject = new Main(httpClient);
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
        public void Query_with_bang_gh_query_should_return_Suggestion_result()
        {
            subject.Query(new("!gh"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "GitHub", SubTitle = "!gh" } });
        }

        [TestMethod]
        public void Query_with_bang_unknown_query_should_return_empty_result()
        {
            subject.Query(new("!unknown"), true)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Query_with_bang_gh_PowerToys_query_should_return_Query_result()
        {
            subject.Query(new("!gh PowerToys"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "GitHub: PowerToys", SubTitle = "!gh PowerToys" } });
        }

        [TestMethod]
        public void Query_should_URL_encode_q_parameter()
        {
            subject.Query(new("!äx"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "Levykauppa Äx", SubTitle = "!äx" } });

            subject.Query(new("!äx Björk"), true)
                .Single().ProgramArguments.Should().Be("https://duckduckgo.com/?va=j&t=hc&q=!%C3%A4x+Bj%C3%B6rk");
        }

        [TestMethod]
        public void Query_should_add_bang_if_missing()
        {
            subject.Query(new("gh"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "GitHub", SubTitle = "!gh" } });

            subject.Query(new("gh PowerToys"), true)
                .Single().ProgramArguments.Should().Be("https://duckduckgo.com/?va=j&t=hc&q=!gh+PowerToys");
        }

        [TestMethod]
        public void UrlEncode()
        {
            // https://stackoverflow.com/a/21771206
            // !so !"#$%&'
            // https://duckduckgo.com/?va=j&t=hb&q=%21so+%21%22%23%24%25%26%27

            var q = " !\"#$%&'";
            HttpUtility.UrlEncode(q).Should().Be("+!%22%23%24%25%26%27");
#pragma warning disable CS0618 // Type or member is obsolete
            HttpUtility.UrlEncodeUnicode(q).Should().Be("+!%22%23%24%25%26%27");
#pragma warning restore CS0618 // Type or member is obsolete
            HttpUtility.UrlPathEncode(q).Should().Be("%20!\"#$%&'");
            WebUtility.UrlEncode(q).Should().Be("+!%22%23%24%25%26%27");
            Uri.EscapeDataString(q).Should().Be("%20%21%22%23%24%25%26%27");
        }
    }
}

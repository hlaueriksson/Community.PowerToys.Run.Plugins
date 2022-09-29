using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Bang.UnitTests
{
    [TestClass]
    public class MainTests
    {
        private Main subject;
        private HttpClient httpClient;

        [TestInitialize]
        public void TestInitialize()
        {
            subject = new Main();

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://localhost/ac/?q=!&kl=wt-wt")
                .Respond("application/json", "[ { \"phrase\": \"!w\", \"score\": 3015746, \"snippet\": \"Wikipedia\", \"image\": \"https://external-content.duckduckgo.com/i/en.wikipedia.org.ico?imgFallback=/watrcoolr/img/search-suggestions_default.png\" } ]");
            mockHttp.When("http://localhost/ac/?q=!gh&kl=wt-wt")
                .Respond("application/json", "[ { \"phrase\": \"!gh\", \"score\": 3015741, \"snippet\": \"GitHub\", \"image\": \"https://external-content.duckduckgo.com/i/github.com.ico?imgFallback=/watrcoolr/img/search-suggestions_default.png\" } ]");
            mockHttp.When("http://localhost/ac/?q=!åäö&kl=wt-wt")
                .Respond("application/json", "[]");
            httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");
        }

        [TestMethod]
        public void Query_should_return_empty_result()
        {
            subject.Query(new(""))
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Query_without_delayedExecution_should_return_empty_result()
        {
            subject.Query(new(""), false)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Query_with_delayedExecution_and_bang_query_should_return_BangSuggestion_result()
        {
            subject.HttpClient = httpClient;
            subject.Query(new("!"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "Wikipedia", SubTitle = "!w" } });
        }

        [TestMethod]
        public void Query_with_delayedExecution_and_bang_gh_query_should_return_BangSuggestion_result()
        {
            subject.HttpClient = httpClient;
            subject.Query(new("!gh"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "GitHub", SubTitle = "!gh" } });
        }

        [TestMethod]
        public void Query_with_delayedExecution_and_bang_åäö_query_should_return_BangSuggestion_result()
        {
            subject.HttpClient = httpClient;
            subject.Query(new("!åäö"), true)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Query_with_delayedExecution_and_bang_gh_PowerToys_query_should_return_Query_result()
        {
            subject.HttpClient = httpClient;
            subject.Query(new("!gh PowerToys"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "GitHub: PowerToys", SubTitle = "!gh PowerToys" } });
        }

        [TestMethod]
        public void Init_should_initialize_the_plugin()
        {
            subject.Init(new PluginInitContext { API = new Mock<IPublicAPI>().Object });
            subject.HttpClient
                .Should().NotBeNull();
        }
    }
}

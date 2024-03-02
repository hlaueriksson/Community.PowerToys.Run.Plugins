using System.Net;
using System.Web;
using Community.PowerToys.Run.Plugin.Bang.Models;
using FluentAssertions;
using LazyCache;
using Moq;
using RichardSzalay.MockHttp;

namespace Community.PowerToys.Run.Plugin.Bang.UnitTests
{
    [TestClass]
    public class DuckDuckGoClientTests
    {
        private DuckDuckGoClient _subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://localhost/ac/?q=!&kl=wt-wt")
                .Respond("application/json", "[ { \"phrase\": \"!w\", \"score\": 3015746, \"snippet\": \"Wikipedia\", \"image\": \"https://external-content.duckduckgo.com/i/en.wikipedia.org.ico?imgFallback=/watrcoolr/img/search-suggestions_default.png\" } ]");
            mockHttp.When("http://localhost/ac/?q=!unknown&kl=wt-wt")
                .Respond("application/json", "[]");
            mockHttp.When("http://localhost/ac/?q=!%C3%A4x&kl=wt-wt")
                .Respond("application/json", "[ { \"phrase\": \"!äx\", \"score\": 0, \"snippet\": \"Levykauppa Äx\", \"image\": \"https://external-content.duckduckgo.com/i/www.levykauppax.fi.ico?imgFallback=/watrcoolr/img/search-suggestions_default.png\" } ]");
            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");

            _subject = new DuckDuckGoClient(new Mock<IAppCache>().Object, httpClient);
        }

        [TestMethod]
        public async Task AutoCompleteAsync_should_return_suggestions()
        {
            var result = await _subject.AutoCompleteAsync("!");
            result.Should().BeEquivalentTo(new[] { new Suggestion { Phrase = "!w", Snippet = "Wikipedia" } });
        }

        [TestMethod]
        public async Task AutoCompleteAsync_with_unknown_query_should_return_empty_suggestions()
        {
            var result = await _subject.AutoCompleteAsync("!unknown");
            result.Should().BeEmpty();
        }

        [TestMethod]
        public async Task AutoCompleteAsync_should_URL_encode_q_parameter()
        {
            var result = await _subject.AutoCompleteAsync("!äx");
            result.Should().BeEquivalentTo(new[] { new Suggestion { Phrase = "!äx", Snippet = "Levykauppa Äx" } });
        }

        [TestMethod]
        public void GetSearchUrl_should_URL_encode_q_parameter()
        {
            var result = _subject.GetSearchUrl("!äx Björk");
            result.Should().Be("https://duckduckgo.com/?t=h_&q=!%C3%A4x+Bj%C3%B6rk");
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

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Dice.UnitTests
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
            mockHttp.When("http://localhost/api/?3d6.json")
                .Respond("application/json", "{\"input\":\"3d6\",\"result\":12,\"details\":\" (4 +4 +4) \",\"code\":\"\",\"illustration\":\"<span style=\\\"color: gray;\\\"><\\/span> <span class=\\\"dc_dice_a\\\">3<\\/span><span class=\\\"dc_dice_d\\\">D6<\\/span>\",\"timestamp\":1664220883,\"x\":1664220883}");
            mockHttp.When("http://localhost/api/?asd.json")
                    .Respond("", "");
            mockHttp.When("http://localhost/api/?+.json")
                    .Respond("application/json", "{\"input\":\"+\",\"result\":\"dice code error\",\"details\":\"+\",\"code\":\"\",\"illustration\":\" <span class=\\\"dc_operator\\\">+<\\/span> \",\"timestamp\":1664222083,\"x\":1664222083}");
            httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost/api/");
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
        public void Query_with_delayedExecution_and_empty_search_query_should_return_RollOptions_result()
        {
            subject.RollOptions = new[] { new RollOption { Expression = "1d6" } };
            subject.Query(new(""), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "1d6", SubTitle = "Roll 1d6" } });
        }

        [TestMethod]
        public void Query_with_delayedExecution_and_search_query_should_return_Roll_result()
        {
            subject.HttpClient = httpClient;
            subject.Query(new("3d6"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "12", SubTitle = "3d6 => (4 +4 +4) = 12" } });
        }

        [TestMethod]
        public void Query_should_return_empty_result_when_Rolz_response_is_empty()
        {
            subject.HttpClient = httpClient;
            subject.Query(new("asd"), true)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Query_should_return_empty_result_when_Rolz_response_is_error()
        {
            subject.HttpClient = httpClient;
            subject.Query(new("+"), true)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Init_should_initialize_the_plugin()
        {
            subject.Init(new PluginInitContext { API = new Mock<IPublicAPI>().Object });
            subject.HttpClient
                .Should().NotBeNull();
            subject.RollOptions
                .Should().NotBeNull();
        }

        [TestMethod]
        public void LoadContextMenus_with_no_ContextData_should_return_empty_result()
        {
            subject.LoadContextMenus(new Result())
                .Should().BeEmpty();
        }

        [TestMethod]
        public void LoadContextMenus_with_RollOption_should_return_menu_with_copy_result()
        {
            var result = new Result { ContextData = new RollOption() };
            subject.LoadContextMenus(result)
                .Should().HaveCount(1)
                .And.Contain(x => x.Title == "Copy result (Enter)");
        }

        [TestMethod]
        public void LoadContextMenus_with_Roll_should_return_menu_with_copy_result_and_copy_details()
        {
            var result = new Result { ContextData = new Roll() };
            subject.LoadContextMenus(result)
                .Should().HaveCount(2)
                .And.Contain(x => x.Title == "Copy result (Enter)")
                .And.Contain(x => x.Title == "Copy details (Ctrl+C)");
        }
    }
}

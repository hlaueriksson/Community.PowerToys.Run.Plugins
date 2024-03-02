using Community.PowerToys.Run.Plugin.Dice.Models;
using FluentAssertions;
using RichardSzalay.MockHttp;

namespace Community.PowerToys.Run.Plugin.Dice.UnitTests
{
    [TestClass]
    public class RolzClientTests
    {
        private RolzClient _subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://localhost/api/?3d6.json")
                .Respond("application/json", "{\"input\":\"3d6\",\"result\":12,\"details\":\" (4 +4 +4) \",\"code\":\"\",\"illustration\":\"<span style=\\\"color: gray;\\\"><\\/span> <span class=\\\"dc_dice_a\\\">3<\\/span><span class=\\\"dc_dice_d\\\">D6<\\/span>\",\"timestamp\":1664220883,\"x\":1664220883}");
            mockHttp.When("http://localhost/api/?unknown.json")
                    .Respond("application/json", "");
            mockHttp.When("http://localhost/api/?+.json")
                    .Respond("application/json", "{\"input\":\"+\",\"result\":\"dice code error\",\"details\":\"+\",\"code\":\"\",\"illustration\":\" <span class=\\\"dc_operator\\\">+<\\/span> \",\"timestamp\":1664222083,\"x\":1664222083}");
            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost/api/");

            _subject = new RolzClient(httpClient);
        }

        [TestMethod]
        public async Task RollAsync_should_return_roll()
        {
            var result = await _subject.RollAsync("3d6");
            result.Should().BeEquivalentTo(new Roll { Input = "3d6", Result = 12, Details = " (4 +4 +4) " });
        }

        [TestMethod]
        public async Task RollAsync_with_unknown_expression_should_return_null_roll()
        {
            var result = await _subject.RollAsync("unknown");
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task RollAsync_with_invalid_expression_should_return_null_roll()
        {
            var result = await _subject.RollAsync("+");
            result.Should().BeNull();
        }
    }
}

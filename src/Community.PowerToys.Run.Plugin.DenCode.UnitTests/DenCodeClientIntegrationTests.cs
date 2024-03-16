using Community.PowerToys.Run.Plugin.DenCode.Models;
using FluentAssertions;

namespace Community.PowerToys.Run.Plugin.DenCode.UnitTests
{
    [TestClass]
    [Ignore("Explicit")]
    public class DenCodeClientIntegrationTests
    {
        private DenCodeClient _subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new DenCodeClient();
        }

        [TestMethod]
        public async Task DenCodeAsync()
        {
            var result = await _subject.DenCodeAsync("value");
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task DenCodeAsync_method()
        {
            var method = new DenCodeMethod
            {
                Key = "date.all"
            };
            var result = await _subject.DenCodeAsync(method, "1710609495");
            result.Should().NotBeNull();
        }
    }
}

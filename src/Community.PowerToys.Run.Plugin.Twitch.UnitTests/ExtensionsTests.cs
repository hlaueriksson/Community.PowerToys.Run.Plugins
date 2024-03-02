using FluentAssertions;

namespace Community.PowerToys.Run.Plugin.Twitch.UnitTests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void Elapsed()
        {
            DateTime.UtcNow.AddMinutes(-59).AddSeconds(-59).ToString("yyyy-MM-ddTHH:mm:ssZ").Elapsed().Should().Be("0:59:59");
            DateTime.UtcNow.AddHours(-1).ToString("yyyy-MM-ddTHH:mm:ssZ").Elapsed().Should().Be("1:00:00");
            DateTime.UtcNow.AddHours(-59).AddMinutes(-59).AddSeconds(-59).ToString("yyyy-MM-ddTHH:mm:ssZ").Elapsed().Should().Be("59:59:59");
        }

        [TestMethod]
        public void Format()
        {
            100.Format().Should().Be("100");
            1_000.Format().Should().Be("1,000");
            1_000_000.Format().Should().Be("1,000,000");
        }

        [TestMethod]
        public void Clean()
        {
            " a b ⏮️".Clean().Should().Be("a b");
            " a b ⏭️".Clean().Should().Be("a b");
        }

        [TestMethod]
        public void Pipe()
        {
            "a".Pipe().Should().Be("a");
            "a".Pipe("b", "", " ", null!, "c").Should().Be("a | b | c");
            "".Pipe("b", "c").Should().Be("b | c");
        }
    }
}

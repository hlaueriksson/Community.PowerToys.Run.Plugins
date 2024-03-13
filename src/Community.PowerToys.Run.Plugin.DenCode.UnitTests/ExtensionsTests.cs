using Community.PowerToys.Run.Plugin.DenCode.Models;
using FluentAssertions;

namespace Community.PowerToys.Run.Plugin.DenCode.UnitTests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void GetDenCodeMethods()
        {
            var result = Constants.Methods.GetDenCodeMethods();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(67);

            var last = result.Last().Value;
            last.Should().BeEquivalentTo(new DenCodeMethod()
            {
                Key = "hash.crc32",
                Method = "CRC32",
                Title = "CRC32 Hash Value Calculator Online",
                Description = "CRC32 hash value calculator.",
                Tooltip = "Enter the text to be converted. (e.g. \"Hello, world!\" => \"ebe6c6e6\")",
                Label = new Dictionary<string, string>
                {
                    { "encHashCRC32", "CRC32" }
                }
            });
        }

        [TestMethod]
        public void GetDenCodeLabels()
        {
            var result = Constants.Methods.GetDenCodeMethods().GetDenCodeLabels();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(104);

            var last = result.Last().Value;
            last.Should().BeEquivalentTo(new DenCodeMethod()
            {
                Key = "hash.crc32",
                Method = "CRC32",
                Title = "CRC32 Hash Value Calculator Online",
                Description = "CRC32 hash value calculator.",
                Tooltip = "Enter the text to be converted. (e.g. \"Hello, world!\" => \"ebe6c6e6\")",
                Label = new Dictionary<string, string>
                {
                    { "encHashCRC32", "CRC32" }
                }
            });
        }

        [TestMethod]
        public void GetRequestType()
        {
            var method = new DenCodeMethod()
            {
                Key = "hash.crc32",
            };
            var result = method.GetRequestType();
            result.Should().Be("hash");
        }
    }
}

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
            result.Should().HaveCount(68);

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
            result.Should().HaveCount(106);

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

        [TestMethod]
        public void IsRoot()
        {
            var result = Constants.Methods.GetDenCodeMethods();
            var first = result.First().Value;
            first.IsRoot().Should().BeTrue();
            result.Skip(1).Should().AllSatisfy(x => x.Value.IsRoot().Should().BeFalse());
        }

        [TestMethod]
        public void IsBranch()
        {
            string[] branches = ["all.all", "string.all", "number.all", "date.all", "color.all", "cipher.all", "hash.all"];
            var result = Constants.Methods.GetDenCodeMethods();

            foreach (var method in result.Values)
            {
                if (branches.Contains(method.Key))
                {
                    method.IsBranch().Should().BeTrue();
                }
                else
                {
                    method.IsBranch().Should().BeFalse();
                }
            }
        }
    }
}

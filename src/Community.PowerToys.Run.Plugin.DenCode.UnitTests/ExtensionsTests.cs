using Community.PowerToys.Run.Plugin.DenCode.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            result.Should().HaveCount(63);

            var last = result.Last().Value;
            last.Should().BeEquivalentTo(new DenCodeMethod()
            {
                Key = "hash.crc32",
                useOe = true,
                useNl = true,
                useTz = false,
                hasEncoded = true,
                hasDecoded = false,
                method = "CRC32",
                title = "CRC32 Hash Value Calculator Online",
                desc = "CRC32 hash value calculator.",
                tooltip = "Please input converting text here. (e.g. \"Hello, world!\" => \"ebe6c6e6\")",
                label = new Dictionary<string, string>
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
            result.Should().HaveCount(98);

            //var last = result.Last().Value;
            //last.Should().BeEquivalentTo(new DenCodeMethod()
            //{
            //    Key = "hash.crc32",
            //    useOe = true,
            //    useNl = true,
            //    useTz = false,
            //    hasEncoded = true,
            //    hasDecoded = false,
            //    method = "CRC32",
            //    title = "CRC32 Hash Value Calculator Online",
            //    desc = "CRC32 hash value calculator.",
            //    tooltip = "Please input converting text here. (e.g. \"Hello, world!\" => \"ebe6c6e6\")",
            //    label = new Dictionary<string, string>
            //    {
            //        { "rc32.encHashCRC32", "CRC32" }
            //    }
            //});
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

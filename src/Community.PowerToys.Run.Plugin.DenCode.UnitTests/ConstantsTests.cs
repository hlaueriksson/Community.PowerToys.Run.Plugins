using Community.PowerToys.Run.Plugin.DenCode.Models;
using FluentAssertions;

namespace Community.PowerToys.Run.Plugin.DenCode.UnitTests
{
    [TestClass]
    public class ConstantsTests
    {
        [TestMethod]
        public void AllMethod()
        {
            var result = Constants.Methods.GetDenCodeMethods();
            var first = result.First().Value;
            first.Method.Should().Be(Constants.AllMethod);
        }
    }
}

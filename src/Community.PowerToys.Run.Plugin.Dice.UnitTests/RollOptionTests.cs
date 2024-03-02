using Community.PowerToys.Run.Plugin.Dice.Models;
using FluentAssertions;

namespace Community.PowerToys.Run.Plugin.Dice.UnitTests
{
    [TestClass]
    public class RollOptionTests
    {
        [TestMethod]
        public void To_string()
        {
            string result = new RollOption { Expression = " Expression ", Description = " Description " };
            result.Should().Be("Expression;Description");

            result = new RollOption { Expression = "Exp;ression", Description = "Description ;)" };
            result.Should().Be("\"Exp;ression\";\"Description ;)\"");

            result = new RollOption();
            result.Should().Be(";");

            result = (RollOption)null!;
            result.Should().Be("");
        }

        [TestMethod]
        public void From_string()
        {
            RollOption result = " Expression ; Description ";
            result.Should().BeEquivalentTo(new RollOption { Expression = "Expression", Description = "Description" });

            result = "\"Exp;ression\";\"Description ;)\"";
            result.Should().BeEquivalentTo(new RollOption { Expression = "Exp;ression", Description = "Description ;)" });

            result = ";";
            result.Should().BeEquivalentTo(new RollOption { Expression = "", Description = "" });

            result = "";
            result.Should().Be(RollOption.Empty);
        }
    }
}

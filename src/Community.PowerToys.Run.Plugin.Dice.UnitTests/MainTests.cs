using Community.PowerToys.Run.Plugin.Dice.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Dice.UnitTests
{
    [TestClass]
    public class MainTests
    {
        private Main _subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var settings = new DiceSettings
            {
                RollOptions =
                [
                    new RollOption { Expression = "1d6" },
                ]
            };

            var mock = new Mock<IRolzClient>();
            mock.Setup(x => x.RollAsync("3d6")).ReturnsAsync(new Roll { Input = "3d6", Result = 12, Details = " (4 +4 +4) " });
            mock.Setup(x => x.RollAsync("unknown")).ReturnsAsync((Roll)null!);

            _subject = new Main(settings, mock.Object);
        }

        [TestMethod]
        public void Query_without_delayedExecution_should_return_empty_result()
        {
            _subject.Query(new(""))
                .Should().BeEmpty();

            _subject.Query(new(""), false)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Query_without_expression_should_return_RollOptions_result()
        {
            _subject.Query(new(""), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "1d6", SubTitle = "Roll 1d6", IcoPath = @"Images\dice.png" } });
        }

        [TestMethod]
        public void Query_with_expression_should_return_Roll_result()
        {
            _subject.Query(new("3d6"), true)
                .Should().BeEquivalentTo(new[] { new Result { Title = "12", SubTitle = "3d6 => (4 +4 +4) = 12", IcoPath = @"Images\dice.png" } });
        }

        [TestMethod]
        public void Query_should_return_empty_result_when_Rolz_response_is_null()
        {
            _subject.Query(new("unknown"), true)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void LoadContextMenus_with_no_ContextData_should_return_empty_result()
        {
            _subject.LoadContextMenus(new Result())
                .Should().BeEmpty();
        }

        [TestMethod]
        public void LoadContextMenus_with_RollOption_should_return_menu_with_roll_result()
        {
            var result = new Result { ContextData = new RollOption() };
            _subject.LoadContextMenus(result)
                .Should().HaveCount(1)
                .And.Contain(x => x.Title == "Roll expression (Enter)");
        }

        [TestMethod]
        public void LoadContextMenus_with_Roll_should_return_menu_with_copy_result_and_copy_details()
        {
            var result = new Result { ContextData = new Roll() };
            _subject.LoadContextMenus(result)
                .Should().HaveCount(2)
                .And.Contain(x => x.Title == "Copy result (Enter)")
                .And.Contain(x => x.Title == "Copy details (Ctrl+C)");
        }
    }
}

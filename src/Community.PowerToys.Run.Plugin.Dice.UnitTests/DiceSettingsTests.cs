using Community.PowerToys.Run.Plugin.Dice.Models;
using FluentAssertions;
using Microsoft.PowerToys.Settings.UI.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Community.PowerToys.Run.Plugin.Dice.UnitTests
{
    [TestClass]
    public class DiceSettingsTests
    {
        [TestMethod]
        public void RollOptions_should_have_defaults()
        {
            var settings = new DiceSettings();
            settings.RollOptions.Should().BeEquivalentTo(RollOption.Defaults);
        }

        [TestMethod]
        public void GetAdditionalOptions_should_return_RollOptions()
        {
            var settings = new DiceSettings();
            settings.GetAdditionalOptions().Select(x => x.TextValue).Should().BeEquivalentTo(
                "d20;Twenty sided die",
                "d12;Twelve sided die",
                "d10;Ten sided die",
                "d6;Six sided die",
                "d4;Four sided die",
                "");
        }

        [TestMethod]
        public void SetAdditionalOptions_should_set_RollOptions()
        {
            var options = new[]
            {
                new PluginAdditionalOption
                {
                    Key = "RollOptions0",
                    TextValue = "3d6;Three six sided dice"
                },
            };

            var settings = new DiceSettings();
            settings.SetAdditionalOptions(options);
            settings.RollOptions.Should().BeEquivalentTo(new[] { new RollOption { Expression = "3d6", Description = "Three six sided dice" } });

            options.First().TextValue = "";
            settings.SetAdditionalOptions(options);
            settings.RollOptions.Should().BeEquivalentTo(RollOption.Defaults);
        }
    }
}

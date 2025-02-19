using Community.PowerToys.Run.Plugin.MobTimer.Models;
using FluentAssertions;
using Microsoft.PowerToys.Settings.UI.Library;

namespace Community.PowerToys.Run.Plugin.MobTimer.UnitTests;

[TestClass]
public class MobTimerSettingsTests
{
    [TestMethod]
    public void Ctor()
    {
        var settings = new MobTimerSettings();
        settings.DriverAssignment.Should().Be(DriverAssignmentType.Manual);
        settings.SoundEnabled.Should().BeTrue();
        settings.SoundPath.Should().Be(@"C:\Windows\Media\Alarm01.wav");
        settings.Kudos.Should().NotBeEmpty();
    }

    [TestMethod]
    public void GetAdditionalOptions()
    {
        var settings = new MobTimerSettings();
        var result = settings.GetAdditionalOptions().ToArray();

        result[0].ComboBoxValue.Should().Be(0);
        result[1].ComboBoxValue.Should().Be(0);
        result[2].TextValueAsMultilineList.Should().NotBeEmpty();
    }

    [TestMethod]
    public void SetAdditionalOptions()
    {
        var options = new[]
        {
            new PluginAdditionalOption
            {
                Key = "DriverAssignment",
                ComboBoxValue = 1,
            },
            new PluginAdditionalOption
            {
                Key = "SoundPath",
                Value = true,
                ComboBoxValue = 1,
            },
            new PluginAdditionalOption
            {
                Key = "Kudos",
                TextValueAsMultilineList = ["foo", "bar"],
            },
        };

        var settings = new MobTimerSettings();
        settings.SetAdditionalOptions(options);
        settings.DriverAssignment.Should().Be(DriverAssignmentType.Sequential);
        settings.SoundEnabled.Should().BeTrue();
        settings.SoundPath.Should().Be(@"C:\Windows\Media\Alarm02.wav");
        settings.Kudos.Should().NotBeEmpty();

        options[0].ComboBoxValue = -1;
        options[1].ComboBoxValue = -1;
        options[2].TextValueAsMultilineList = null;
        settings.SetAdditionalOptions(options);
        settings.DriverAssignment.Should().Be(DriverAssignmentType.Manual);
        settings.SoundEnabled.Should().BeTrue();
        settings.SoundPath.Should().Be(@"C:\Windows\Media\Alarm01.wav");
        settings.Kudos.Should().BeEmpty();
    }

    [TestMethod]
    public void GetKudos()
    {
        var settings = new MobTimerSettings();
        settings.GetKudos().Should().NotBeEmpty();

        settings.Kudos = [];
        settings.GetKudos().Should().BeEmpty();
    }
}
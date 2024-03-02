using FluentAssertions;
using Microsoft.PowerToys.Settings.UI.Library;

namespace Community.PowerToys.Run.Plugin.Twitch.UnitTests
{
    [TestClass]
    public class TwitchSettingsTests
    {
        [TestMethod]
        public void GetAdditionalOptions_should_return_settings()
        {
            var settings = new TwitchSettings
            {
                TwitchApiClientId = "TwitchApiClientId",
                TwitchApiClientSecret = "TwitchApiClientSecret",
            };
            settings.GetAdditionalOptions().ElementAt(0).TextValue.Should().Be(settings.TwitchApiClientId);
            settings.GetAdditionalOptions().ElementAt(1).TextValue.Should().Be(settings.TwitchApiClientSecret);
            settings.GetAdditionalOptions().ElementAt(2).NumberValue.Should().Be(settings.TwitchApiParameterFirst);
            settings.GetAdditionalOptions().ElementAt(3).TextValue.Should().Be(settings.TwitchApiParameterLanguage);
            settings.GetAdditionalOptions().ElementAt(4).Value.Should().Be(settings.TwitchApiParameterLiveOnly);
        }

        [TestMethod]
        public void SetAdditionalOptions_should_set_settings()
        {
            var options = new[]
            {
                new PluginAdditionalOption
                {
                    Key = "TwitchApiClientId",
                    TextValue = "foo",
                },
                new PluginAdditionalOption
                {
                    Key = "TwitchApiClientSecret",
                    TextValue = "bar",
                },
                new PluginAdditionalOption
                {
                    Key = "TwitchApiParameterFirst",
                    NumberValue = 10,
                },
                new PluginAdditionalOption
                {
                    Key = "TwitchApiParameterLanguage",
                    TextValue = "sv",
                },
                new PluginAdditionalOption
                {
                    Key = "TwitchApiParameterLiveOnly",
                    Value = false,
                },
            };

            var settings = new TwitchSettings();
            settings.SetAdditionalOptions(options);
            settings.TwitchApiClientId.Should().Be("foo");
            settings.TwitchApiClientSecret.Should().Be("bar");
            settings.TwitchApiParameterFirst.Should().Be(10);
            settings.TwitchApiParameterLanguage.Should().Be("sv");
            settings.TwitchApiParameterLiveOnly.Should().Be(false);
        }
    }
}

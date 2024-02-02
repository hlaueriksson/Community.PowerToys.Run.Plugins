using FluentAssertions;
using Microsoft.PowerToys.Settings.UI.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Community.PowerToys.Run.Plugin.Twitch.UnitTests
{
    [TestClass]
    public class TwitchSettingsTests
    {
        [TestMethod]
        public void GetAdditionalOptions_should_return_TwitchApi_settings()
        {
            var settings = new TwitchSettings
            {
                TwitchApiClientId = "TwitchApiClientId",
                TwitchApiClientSecret = "TwitchApiClientSecret",
            };
            settings.GetAdditionalOptions().First().TextValue.Should().Be(settings.TwitchApiClientId);
            settings.GetAdditionalOptions().Last().TextValue.Should().Be(settings.TwitchApiClientSecret);
        }

        [TestMethod]
        public void SetAdditionalOptions_should_set_TwitchApi_settings()
        {
            var options = new[]
            {
                new PluginAdditionalOption
                {
                    Key = "TwitchApiClientId",
                    TextValue = "foo"
                },
                new PluginAdditionalOption
                {
                    Key = "TwitchApiClientSecret",
                    TextValue = "bar"
                },
            };

            var settings = new TwitchSettings();
            settings.SetAdditionalOptions(options);
            settings.TwitchApiClientId.Should().Be("foo");
            settings.TwitchApiClientSecret.Should().Be("bar");
        }
    }
}

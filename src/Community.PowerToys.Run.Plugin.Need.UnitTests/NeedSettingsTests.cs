using FluentAssertions;
using Microsoft.PowerToys.Settings.UI.Library;

namespace Community.PowerToys.Run.Plugin.Need.UnitTests
{
    [TestClass]
    public class NeedSettingsTests
    {
        [TestMethod]
        public void StorageFileName_should_have_default()
        {
            var settings = new NeedSettings();
            settings.StorageFileName.Should().Be(NeedStorage.DefaultFileName);
        }

        [TestMethod]
        public void GetAdditionalOptions_should_return_StorageFileName()
        {
            var settings = new NeedSettings();
            settings.GetAdditionalOptions().First().TextValue.Should().Be(NeedStorage.DefaultFileName);
        }

        [TestMethod]
        public void SetAdditionalOptions_should_set_StorageFileName()
        {
            var options = new[]
            {
                new PluginAdditionalOption
                {
                    Key = "StorageFileName",
                    TextValue = "need.json"
                },
            };

            var settings = new NeedSettings();
            settings.SetAdditionalOptions(options);
            settings.StorageFileName.Should().Be("need.json");

            options.First().TextValue = "";
            settings.SetAdditionalOptions(options);
            settings.StorageFileName.Should().Be(NeedStorage.DefaultFileName);
        }
    }
}

using System.IO;
using Microsoft.PowerToys.Settings.UI.Library;

namespace Community.PowerToys.Run.Plugin.Need
{
    /// <summary>
    /// Plugin settings.
    /// </summary>
    public class NeedSettings
    {
        private string _storageFileName = NeedStorage.DefaultFileName;

        /// <summary>
        /// File store.
        /// </summary>
        public string StorageFileName
        {
            get => _storageFileName;
            set => _storageFileName = IsValidFileName(value) ? value : NeedStorage.DefaultFileName;
        }

        internal string StorageDirectoryPath { get; set; } = null!;

        internal IEnumerable<PluginAdditionalOption> GetAdditionalOptions()
        {
            return
            [
                new()
                {
                    Key = nameof(StorageFileName),
                    DisplayLabel = "Storage File Name",
                    DisplayDescription = StorageDirectoryPath,
                    PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                    TextValue = StorageFileName,
                },
            ];
        }

        internal void SetAdditionalOptions(IEnumerable<PluginAdditionalOption> additionalOptions)
        {
            ArgumentNullException.ThrowIfNull(additionalOptions);

            var options = additionalOptions.ToList();
            StorageFileName = options.Find(x => x.Key == nameof(StorageFileName))?.TextValue ?? NeedStorage.DefaultFileName;
        }

        private static bool IsValidFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }
    }
}

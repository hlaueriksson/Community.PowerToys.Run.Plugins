using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Community.PowerToys.Run.Plugin.Need.Models;
using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Infrastructure.Storage;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Need
{
    /// <summary>
    /// Main class of this plugin that implement all used interfaces.
    /// </summary>
    public class Main : IPlugin, IContextMenu, ISettingProvider, ISavable, IReloadable, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            Storage = new PluginJsonStorage<NeedSettings>();
            Settings = Storage.Load();
            Settings.StorageDirectoryPath = Storage.DirectoryPath;
            NeedStorage = new NeedStorage(Settings);
        }

        internal Main(NeedSettings settings, INeedStorage needStorage)
        {
            Storage = new PluginJsonStorage<NeedSettings>();
            Settings = settings;
            NeedStorage = needStorage;
        }

        /// <summary>
        /// ID of the plugin.
        /// </summary>
        public static string PluginID => "0B47DD2677CD41E9927218E9D67EFAD1";

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "Need";

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Store things you need, but can't remember";

        /// <summary>
        /// Additional options for the plugin.
        /// </summary>
        public IEnumerable<PluginAdditionalOption> AdditionalOptions => Settings.GetAdditionalOptions();

        private PluginInitContext? Context { get; set; }

        private string? IconPath { get; set; }

        private bool Disposed { get; set; }

        private PluginJsonStorage<NeedSettings> Storage { get; }

        private NeedSettings Settings { get; }

        private INeedStorage NeedStorage { get; }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query)
        {
            if (query?.Search is null)
            {
                return [];
            }

            var args = query.Search;

            if (string.IsNullOrEmpty(args))
            {
                return NeedStorage.GetRecords().Select(GetResultForGetRecord).ToList() ?? [];
            }

            var tokens = args.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 1)
            {
                var q = tokens[0];

                return NeedStorage.GetRecords(q).Select(GetResultForGetRecord).ToList() ?? [];
            }

            var key = tokens[0];
            var value = tokens[1];

            return [GetResultForSetRecord(key, value)];

            Result GetResultForGetRecord(Record record) => new()
            {
                QueryTextDisplay = args,
                IcoPath = IconPath,
                Title = record.Key,
                SubTitle = record.Value,
                ToolTipData = new ToolTipData("Get", $"Key: {record.Key}\nValue: {record.Value}\nCreated: {record.Created}\nUpdated: {record.Updated}"),
                ContextData = record,
            };

            Result GetResultForSetRecord(string key, string value)
            {
                var existing = NeedStorage.GetRecord(key);

                return new()
                {
                    QueryTextDisplay = args,
                    IcoPath = IconPath,
                    Title = key,
                    SubTitle = existing != null ? existing.Value + " -> " + value : value,
                    ToolTipData = existing != null ? new ToolTipData("Set", $"Key: {key}\nOld value: {existing.Value}\nNew value: {value}") : new ToolTipData("Set", $"Key: {key}\nValue: {value}"),
                    ContextData = (key, value),
                };
            }
        }

        /// <summary>
        /// Initialize the plugin with the given <see cref="PluginInitContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="PluginInitContext"/> for this plugin.</param>
        public void Init(PluginInitContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(Context.API.GetCurrentTheme());
        }

        /// <summary>
        /// Return a list context menu entries for a given <see cref="Result"/> (shown at the right side of the result).
        /// </summary>
        /// <param name="selectedResult">The <see cref="Result"/> for the list with context menu entries.</param>
        /// <returns>A list context menu entries.</returns>
        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            if (selectedResult?.ContextData is Record record)
            {
                return
                [
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Copy value (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE8C8", // E8C8 => Symbol: Copy
                        AcceleratorKey = Key.Enter,
                        Action = _ => CopyToClipboard(record.Value),
                    },
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Copy details (Ctrl+C)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xF413", // F413 => Symbol: CopyTo
                        AcceleratorKey = Key.C,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ => CopyToClipboard(record.ToJson()),
                    },
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Delete key (Ctrl+Del)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE74D", // E74D => Symbol: Delete
                        AcceleratorKey = Key.Delete,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ =>
                        {
                            NeedStorage.RemoveRecord(record.Key);
                            return ChangeQuery(selectedResult.QueryTextDisplay);
                        },
                    },
                ];
            }

            if (selectedResult?.ContextData is (string key, string value))
            {
                var existing = NeedStorage.GetRecord(key);

                return
                [
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = existing != null ? "Update value (Enter)" : "Add value (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE74E", // E74E => Symbol: Save
                        AcceleratorKey = Key.Enter,
                        Action = _ =>
                        {
                            NeedStorage.SetRecord(key, value);
                            return ChangeQuery(selectedResult.QueryTextDisplay);
                        },
                    },
                ];
            }

            return [];
        }

        /// <summary>
        /// Creates setting panel.
        /// </summary>
        /// <returns>The control.</returns>
        /// <exception cref="NotImplementedException">method is not implemented.</exception>
        public Control CreateSettingPanel() => throw new NotImplementedException();

        /// <summary>
        /// Updates settings.
        /// </summary>
        /// <param name="settings">The plugin settings.</param>
        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            Settings.SetAdditionalOptions(settings.AdditionalOptions);
            Save();
        }

        /// <summary>
        /// Saves data.
        /// </summary>
        public void Save()
        {
            Storage.Save();
            NeedStorage.Save();
        }

        /// <summary>
        /// Reloads data.
        /// </summary>
        public void ReloadData()
        {
            NeedStorage.Load();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Wrapper method for <see cref="Dispose()"/> that dispose additional objects and events form the plugin itself.
        /// </summary>
        /// <param name="disposing">Indicate that the plugin is disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed || !disposing)
            {
                return;
            }

            if (Context?.API != null)
            {
                Context.API.ThemeChanged -= OnThemeChanged;
            }

            Disposed = true;
        }

        private void UpdateIconPath(Theme theme) => IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite ? "Images/need.light.png" : "Images/need.dark.png";

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);

        private static bool CopyToClipboard(string? value)
        {
            if (value != null)
            {
                Clipboard.SetText(value);
            }

            return true;
        }

        private bool ChangeQuery(string query)
        {
            Context?.API.ChangeQuery(query, true);

            return true;
        }
    }
}

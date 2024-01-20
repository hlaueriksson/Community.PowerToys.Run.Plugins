using System.Windows;
using System.Windows.Input;
using ManagedCommon;
using Wox.Infrastructure.Storage;
using Wox.Plugin;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Need
{
    /// <summary>
    /// Main class of this plugin that implement all used interfaces.
    /// </summary>
    public class Main : IPlugin, IContextMenu, ISavable, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            Log.Info("Ctor", GetType());

            Storage = new PluginJsonStorage<NeedSettings>();
            Settings = Storage.Load();
        }

        internal Main(NeedSettings settings)
        {
            Settings = settings;
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

        private PluginJsonStorage<NeedSettings>? Storage { get; }

        private NeedSettings Settings { get; }

        private PluginInitContext? Context { get; set; }

        private string? IconPath { get; set; }

        private bool Disposed { get; set; }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query)
        {
            Log.Info($"Query: {query?.RawQuery}", GetType());

            if (query?.Search is null)
            {
                return new List<Result>(0);
            }

            var args = query.Search;

            if (string.IsNullOrEmpty(args))
            {
                return Settings.GetRecords().Select(GetResultForGetRecord).ToList() ?? new List<Result>(0);
            }

            var tokens = args.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 1)
            {
                var q = tokens[0];

                return Settings.GetRecords(q).Select(GetResultForGetRecord).ToList() ?? new List<Result>(0);
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

            Result GetResultForSetRecord(string key, string value) => new()
            {
                QueryTextDisplay = args,
                IcoPath = IconPath,
                Title = key,
                SubTitle = value,
                ToolTipData = new ToolTipData("Set", $"Key: {key}\nValue: {value}"),
                Action = _ =>
                {
                    Log.Info("Set: " + args, GetType());
                    Settings.SetRecord(key, value);
                    return true;
                },
            };
        }

        /// <summary>
        /// Initialize the plugin with the given <see cref="PluginInitContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="PluginInitContext"/> for this plugin.</param>
        public void Init(PluginInitContext context)
        {
            Log.Info("Init", GetType());

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
                        Action = _ =>
                        {
                            Log.Info("Copy value (Enter): " + record.Value, GetType());
                            return CopyToClipboard(record.Value);
                        },
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
                            Log.Info("Delete key (Ctrl+Del): " + record.Key, GetType());
                            Settings.RemoveRecord(record.Key);
                            return true;
                        },
                    },
                ];
            }

            return new List<ContextMenuResult>(0);
        }

        /// <inheritdoc/>
        public void Save()
        {
            Log.Info("Save", GetType());

            Storage?.Save();
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
            Log.Info("Dispose", GetType());

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

        private static bool CopyToClipboard(string? value)
        {
            if (value != null)
            {
                Clipboard.SetText(value);
            }

            return true;
        }

        private void UpdateIconPath(Theme theme) => IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite ? "Images/need.light.png" : "Images/need.dark.png";

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);
    }
}

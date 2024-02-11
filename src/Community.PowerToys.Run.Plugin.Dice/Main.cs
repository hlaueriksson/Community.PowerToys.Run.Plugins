using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Community.PowerToys.Run.Plugin.Dice.Models;
using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Infrastructure.Storage;
using Wox.Plugin;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Dice
{
    /// <summary>
    /// Main class of this plugin that implement all used interfaces.
    /// </summary>
    public class Main : IPlugin, IDelayedExecutionPlugin, IContextMenu, ISettingProvider, ISavable, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            Storage = new PluginJsonStorage<DiceSettings>();
            Settings = Storage.Load();
            RolzClient = new RolzClient();
        }

        internal Main(DiceSettings settings, IRolzClient rolzClient)
        {
            Storage = new PluginJsonStorage<DiceSettings>();
            Settings = settings;
            RolzClient = rolzClient;
        }

        /// <summary>
        /// ID of the plugin.
        /// </summary>
        public static string PluginID => "0CB159EBD5394A01A8CE995982FE2622";

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "Dice";

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Roleplaying Dice Roller";

        /// <summary>
        /// Additional options for the plugin.
        /// </summary>
        public IEnumerable<PluginAdditionalOption> AdditionalOptions => Settings.GetAdditionalOptions();

        private PluginInitContext? Context { get; set; }

        private string? IconPath { get; set; }

        private bool Disposed { get; set; }

        private PluginJsonStorage<DiceSettings> Storage { get; }

        private DiceSettings Settings { get; }

        private IRolzClient RolzClient { get; }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query)
        {
            return new List<Result>(0);
        }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <param name="delayedExecution">False if this is the first pass through plugins, true otherwise. Slow plugins should run delayed.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query, bool delayedExecution)
        {
            if (query?.Search is null || !delayedExecution)
            {
                return new List<Result>(0);
            }

            var expression = query.Search;

            if (string.IsNullOrEmpty(expression))
            {
                return Settings.RollOptions.ConvertAll(GetResultFromRollOption) ?? new List<Result>(0);
            }

            var roll = Roll(expression);

            if (roll != null)
            {
                return [GetResultFromRoll(roll)];
            }

            return new List<Result>(0);

            Result GetResultFromRollOption(RollOption option) => new()
            {
                QueryTextDisplay = option.Expression,
                IcoPath = IconPath,
                Title = option.Expression,
                SubTitle = option.Description ?? $"Roll {option.Expression}",
                ToolTipData = new ToolTipData("Dice", $"Roll {option.Expression}"),
                ContextData = option,
            };

            Result GetResultFromRoll(Roll roll) => new()
            {
                QueryTextDisplay = roll.Input,
                IcoPath = IconPath,
                Title = roll.Result.ToString(CultureInfo.InvariantCulture),
                SubTitle = roll.Input + " => " + roll.Details?.Trim() + " = " + roll.Result.ToString(CultureInfo.InvariantCulture),
                ToolTipData = new ToolTipData("Dice", $"Roll {roll.Input}"),
                ContextData = roll,
            };
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
            if (selectedResult?.ContextData is RollOption option)
            {
                return
                [
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Roll expression (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE72C", // E72C => Symbol: Refresh
                        AcceleratorKey = Key.Enter,
                        Action = _ =>
                        {
                            Context?.API.ChangeQuery(Context?.CurrentPluginMetadata.ActionKeyword + " " + option.Expression, true);
                            return false;
                        },
                    },
                ];
            }

            if (selectedResult?.ContextData is Roll roll)
            {
                return
                [
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Copy result (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE8C8", // E8C8 => Symbol: Copy
                        AcceleratorKey = Key.Enter,
                        Action = _ => CopyToClipboard(roll.Result.ToString(CultureInfo.InvariantCulture)),
                    },
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Copy details (Ctrl+C)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xF413", // F413 => Symbol: CopyTo
                        AcceleratorKey = Key.C,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ => CopyToClipboard(roll.Input + " => " + roll.Details?.Trim() + " = " + roll.Result.ToString(CultureInfo.InvariantCulture)),
                    },
                ];
            }

            return new List<ContextMenuResult>(0);
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
        /// Saves settings.
        /// </summary>
        public void Save()
        {
            Storage.Save();
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

        private void UpdateIconPath(Theme theme) => IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite ? "Images/dice.light.png" : "Images/dice.dark.png";

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);

        private Roll? Roll(string expression)
        {
            try
            {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                return RolzClient.RollAsync(expression).Result;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
            }
            catch (Exception ex)
            {
                Log.Exception("Roll failed.", ex, GetType());
            }

            return null;
        }

        private static bool CopyToClipboard(string? value)
        {
            if (value != null)
            {
                Clipboard.SetText(value);
            }

            return true;
        }
    }
}

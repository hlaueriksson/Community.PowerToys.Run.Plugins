using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using ManagedCommon;
using Wox.Plugin;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Dice
{
    /// <summary>
    /// Main class of this plugin that implement all used interfaces.
    /// </summary>
    public class Main : IPlugin, IDelayedExecutionPlugin, IContextMenu, IDisposable
    {
        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "Dice";

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Roleplaying Dice Roller";

        private PluginInitContext? Context { get; set; }

        private string? IconPath { get; set; }

        private bool Disposed { get; set; }

        private HttpClient? HttpClient { get; set; }

        private IReadOnlyCollection<RollOption>? RollOptions { get; set; }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query)
        {
            Log.Info($"Query: {query?.RawQuery}", GetType());

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
            Log.Info($"Query: {query?.RawQuery}, {delayedExecution}", GetType());

            if (query?.Search is null || !delayedExecution)
            {
                return new List<Result>(0);
            }

            var expression = query.Search;

            if (string.IsNullOrEmpty(expression))
            {
                return RollOptions?.Select(GetResultFromRollOption).ToList() ?? new List<Result>(0);
            }

            var results = new List<Result>();

            var roll = Roll(expression);
            if (roll != null)
            {
                results.Add(GetResultFromRoll(roll));
            }

            return results;

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

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://rolz.org/api/"),
                Timeout = TimeSpan.FromSeconds(5),
            };
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Community.PowerToys.Run.Plugin.Dice");

            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + @"\Plugins\Dice\appsettings.json";
            Log.Info(path, GetType());
            if (!File.Exists(path))
            {
                Log.Info("AppSettings not found.", GetType());
                return;
            }

            try
            {
                using FileStream stream = File.OpenRead(path);
                var appSettings = JsonSerializer.Deserialize<AppSettings>(stream);
                RollOptions = appSettings?.RollOptions;
            }
            catch (Exception ex)
            {
                Log.Exception("Reading AppSettings failed.", ex, GetType());
            }
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
                return new List<ContextMenuResult>
                {
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Copy result (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE8C8", // E8C8 => Symbol: Copy
                        AcceleratorKey = Key.Enter,
                        Action = _ =>
                        {
                            Log.Info("Expression Copy result (Enter): " + option.Expression, GetType());
                            if (option.Expression == null)
                            {
                                return false;
                            }

                            var roll = Roll(option.Expression);
                            Log.Info("Expression Copy result (Enter): " + roll?.Result.ToString(CultureInfo.InvariantCulture), GetType());
                            return CopyToClipboard(roll?.Result.ToString(CultureInfo.InvariantCulture));
                        },
                    },
                };
            }

            if (selectedResult?.ContextData is Roll roll)
            {
                return new List<ContextMenuResult>
                {
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Copy result (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE8C8", // E8C8 => Symbol: Copy
                        AcceleratorKey = Key.Enter,
                        Action = _ =>
                        {
                            Log.Info("Roll Copy result (Enter): " + roll.Result.ToString(CultureInfo.InvariantCulture), GetType());
                            return CopyToClipboard(roll.Result.ToString(CultureInfo.InvariantCulture));
                        },
                    },
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Copy details (Ctrl+C)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xF413", // F413 => Symbol: CopyTo
                        AcceleratorKey = Key.C,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ =>
                        {
                            Log.Info("Roll Copy details (Ctrl+C): " + roll.Details?.Trim(), GetType());
                            return CopyToClipboard(roll.Details?.Trim());
                        },
                    },
                };
            }

            return new List<ContextMenuResult>(0);
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

            if (Context != null && Context.API != null)
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

        private void UpdateIconPath(Theme theme) => IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite ? "Images/dice.light.png" : "Images/dice.dark.png";

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);

        private Roll? Roll(string expression)
        {
            try
            {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                var content = HttpClient?.GetStringAsync($"?{expression}.json").Result;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits

                if (string.IsNullOrEmpty(content) || content.Contains("dice code error", StringComparison.InvariantCulture))
                {
                    return null;
                }

                return JsonSerializer.Deserialize<Roll>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Log.Exception("Roll failed.", ex, GetType());
            }

            return null;
        }
    }
}

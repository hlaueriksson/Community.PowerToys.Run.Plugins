using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Community.PowerToys.Run.Plugin.DenCode.Models;
using ManagedCommon;
using Wox.Infrastructure;
using Wox.Plugin;
using Wox.Plugin.Common;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.DenCode
{
    /// <summary>
    /// Main class of this plugin that implement all used interfaces.
    /// </summary>
    public class Main : IPlugin, IDelayedExecutionPlugin, IContextMenu, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            Log.Info($"Ctor", GetType());

            DenCodeClient = new DenCodeClient();
            DenCodeMethods = Constants.Methods.GetDenCodeMethods();
            DenCodeLabels = DenCodeMethods.GetDenCodeLabels();
        }

        internal Main(IDenCodeClient denCodeClient)
        {
            DenCodeClient = denCodeClient;
            DenCodeMethods = Constants.Methods.GetDenCodeMethods();
            DenCodeLabels = DenCodeMethods.GetDenCodeLabels();
        }

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "DenCode";

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Encoding and decoding values";

        private IDenCodeClient DenCodeClient { get; }

        private Dictionary<string, DenCodeMethod> DenCodeMethods { get; }

        private Dictionary<string, DenCodeMethod> DenCodeLabels { get; }

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

            var args = query.Search;

            if (string.IsNullOrEmpty(args))
            {
                return DenCodeMethods.Values.Where(x => x.method != null).Select(GetResultFromDenCodeMethod).ToList() ?? new List<Result>(0);
            }

            var tokens = args.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            var key = tokens[0];

            if (tokens.Length == 1)
            {
                var methods = DenCodeMethods.Values.Where(x => x.method != null && x.Key.Contains(key, StringComparison.InvariantCultureIgnoreCase));

                if (methods.Any())
                {
                    return methods.Select(GetResultFromDenCodeMethod).ToList();
                }
            }

            var method = DenCodeMethods.GetValueOrDefault(key);

            if (method != null)
            {
                var value = tokens[1];

#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                var response = DenCodeClient.DenCodeAsync(method, value).Result;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits

                return GetResultsFromDenCodeResponse(response, value);
            }
            else
            {
                var value = args;

#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                var response = DenCodeClient.DenCodeAsync(value).Result;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits

                return GetResultsFromDenCodeResponse(response, value);
            }

            Result GetResultFromDenCodeMethod(DenCodeMethod method) => new()
            {
                QueryTextDisplay = method.Key + " ",
                IcoPath = IconPath,
                Title = method.method,
                SubTitle = method.desc,
                ToolTipData = new ToolTipData(method.title, method.tooltip),
                ContextData = method,
            };

            List<Result> GetResultsFromDenCodeResponse(DenCodeResponse? response, string value)
            {
                var results = new List<Result>();

                foreach (var kvp in response!.response)
                {
                    if (kvp.Value.ValueKind != JsonValueKind.String)
                    {
                        continue;
                    }

                    var result = kvp.Value.GetString();

                    if (result == null)
                    {
                        continue;
                    }

                    var method = DenCodeLabels.GetValueOrDefault(kvp.Key);
                    var prefix = kvp.Key.StartsWith("dec", StringComparison.Ordinal) ? "Decoded: " : "Encoded: ";

                    results.Add(new Result
                    {
                        IcoPath = IconPath,
                        Title = result,
                        SubTitle = prefix + (method?.label[kvp.Key] ?? kvp.Key),
                        ToolTipData = new ToolTipData(method?.title ?? "DenCode", method?.desc ?? kvp.Key),
                        Action = _ =>
                        {
                            Log.Info("Copy result (Enter): " + result, GetType());
                            return CopyToClipboard(result);
                        },
                        ContextData = new DenCodeContextData
                        {
                            Value = value,
                            Result = kvp,
                            Method = method,
                        },
                    });
                }

                return results;
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
            if (selectedResult?.ContextData is DenCodeMethod method)
            {
                return new List<ContextMenuResult>
                {
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Open website (Ctrl+Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xEB41", // EB41 => Symbol: Website
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ =>
                        {
                            var slug = method.Key.Replace('.', '/') ?? string.Empty;
                            var arguments = $"https://dencode.com/{slug}";

                            Log.Info("Open website (Ctrl+Enter): " + arguments, GetType());

                            if (!Helper.OpenCommandInShell(DefaultBrowserInfo.Path, DefaultBrowserInfo.ArgumentsPattern, arguments))
                            {
                                Log.Error("Open default browser failed.", GetType());
                                Context?.API.ShowMsg($"Plugin: {Name}", "Open default browser failed.");
                                return false;
                            }

                            return true;
                        },
                    },
                };
            }

            if (selectedResult?.ContextData is DenCodeContextData data)
            {
                return new List<ContextMenuResult>
                {
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Copy result (Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xE93E", // E93E => Symbol: Streaming
                        /*AcceleratorKey = Key.Enter,*/
                        Action = _ =>
                        {
                            var result = data.Result.Value.GetString();
                            Log.Info("Copy result (Enter): " + result, GetType());
                            return CopyToClipboard(result);
                        },
                    },
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Open website (Ctrl+Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xEB41", // EB41 => Symbol: Website
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ =>
                        {
                            var slug = data.Method?.Key.Replace('.', '/') ?? string.Empty;
                            var arguments = $"https://dencode.com/{slug}?v={UrlEncode(data.Value)}";

                            Log.Info("Open website (Ctrl+Enter): " + arguments, GetType());

                            if (!Helper.OpenCommandInShell(DefaultBrowserInfo.Path, DefaultBrowserInfo.ArgumentsPattern, arguments))
                            {
                                Log.Error("Open default browser failed.", GetType());
                                Context?.API.ShowMsg($"Plugin: {Name}", "Open default browser failed.");
                                return false;
                            }

                            return true;
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

        private static string UrlEncode(string q)
        {
            return Uri.EscapeDataString(q);
        }

        private void UpdateIconPath(Theme theme) => IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite ? "Images/dencode.light.png" : "Images/dencode.dark.png";

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);
    }
}

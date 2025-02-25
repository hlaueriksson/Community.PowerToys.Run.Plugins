using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Community.PowerToys.Run.Plugin.DenCode.Models;
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
        /// ID of the plugin.
        /// </summary>
        public static string PluginID => "8738EC80820A4BF89FA9C3AED709AF9A";

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "DenCode";

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Encoding & Decoding";

        private PluginInitContext? Context { get; set; }

        private static string IconPath => @"Images\dencode.png";

        private bool Disposed { get; set; }

        private IDenCodeClient DenCodeClient { get; }

        private Dictionary<string, DenCodeMethod> DenCodeMethods { get; }

        private Dictionary<string, DenCodeMethod> DenCodeLabels { get; }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query)
        {
            return [];
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
                return [];
            }

            var args = query.Search;

            if (string.IsNullOrEmpty(args))
            {
                return DenCodeMethods.Values.Where(x => x.Method != null).Select(GetResultFromDenCodeMethod).ToList() ?? [];
            }

            var tokens = args.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            var key = tokens[0];

            if (tokens.Length == 1)
            {
                var methods = DenCodeMethods.Values.Where(x => x.Method != null && x.Key.Contains(key, StringComparison.InvariantCultureIgnoreCase)).ToList();

                if (methods.Count != 0)
                {
                    return methods.ConvertAll(GetResultFromDenCodeMethod);
                }
            }

            var method = DenCodeMethods.GetValueOrDefault(key);

            if (method != null)
            {
                var value = tokens[1];

#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
#pragma warning disable VSTHRD104 // Offer async methods
                var response = DenCodeClient.DenCodeAsync(method, value).Result;
#pragma warning restore VSTHRD104 // Offer async methods
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits

                return GetResultsFromDenCodeResponse(response, value, method.IsBranch());
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
                Title = method.Method,
                SubTitle = method.Description,
                ToolTipData = new ToolTipData(method.Title, method.Tooltip),
                ContextData = method,
            };

            List<Result> GetResultsFromDenCodeResponse(DenCodeResponse? response, string value, bool removeUnchangedResults = true)
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

                    if (removeUnchangedResults && result == value)
                    {
                        continue;
                    }

                    var method = DenCodeLabels.GetValueOrDefault(kvp.Key);
                    var prefix = kvp.Key.StartsWith("dec", StringComparison.Ordinal) ? "Decoded: " : "Encoded: ";

                    results.Add(new Result
                    {
                        QueryTextDisplay = method?.Key + " " + value,
                        IcoPath = IconPath,
                        Title = result,
                        SubTitle = prefix + (method?.Label[kvp.Key] ?? kvp.Key),
                        ToolTipData = new ToolTipData(method?.Title ?? "DenCode", method?.Description ?? kvp.Key),
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
                return
                [
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Open website (Ctrl+Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xF6FA", // F6FA => Symbol: WebSearch
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ => OpenInBrowser(DenCodeClient.GetUrl(method)),
                    },
                ];
            }

            if (selectedResult?.ContextData is DenCodeContextData data)
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
                        Action = _ => CopyToClipboard(data.Result.Value.GetString()),
                    },
                    new ContextMenuResult
                    {
                        PluginName = Name,
                        Title = "Open website (Ctrl+Enter)",
                        FontFamily = "Segoe MDL2 Assets",
                        Glyph = "\xF6FA", // F6FA => Symbol: WebSearch
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ => OpenInBrowser(DenCodeClient.GetUrl(data)),
                    },
                ];
            }

            return [];
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

        private bool OpenInBrowser(string url)
        {
            if (!Helper.OpenCommandInShell(DefaultBrowserInfo.Path, DefaultBrowserInfo.ArgumentsPattern, url))
            {
                Log.Error("Open default browser failed.", GetType());
                Context?.API.ShowMsg($"Plugin: {Name}", "Open default browser failed.");
                return false;
            }

            return true;
        }
    }
}

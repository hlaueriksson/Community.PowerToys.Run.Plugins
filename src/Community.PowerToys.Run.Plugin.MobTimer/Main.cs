using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Community.PowerToys.Run.Plugin.MobTimer.Models;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Infrastructure.Storage;
using Wox.Plugin;
using Wox.Plugin.Logger;
using Duration = Community.PowerToys.Run.Plugin.MobTimer.Models.Duration;
using Timer = Community.PowerToys.Run.Plugin.MobTimer.Models.Timer;

namespace Community.PowerToys.Run.Plugin.MobTimer;

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
        Settings = new MobTimerSettings();
        SessionStorage = new PluginJsonStorage<MobTimerSession>();
        Session = SessionStorage.Load();
        Session.Clear();
        Service = new MobTimerService();
        Service.MobTimerStarted += OnMobTimerStarted;
        Service.MobTimerElapsed += OnMobTimerElapsed;
    }

    internal Main(MobTimerSettings settings, MobTimerSession session, IMobTimerService service)
    {
        Settings = settings;
        SessionStorage = new PluginJsonStorage<MobTimerSession>();
        Session = session;
        Service = service;
    }

    /// <summary>
    /// ID of the plugin.
    /// </summary>
    public static string PluginID => "98709941F8E2497D8891B7932F478CF0";

    /// <summary>
    /// Name of the plugin.
    /// </summary>
    public string Name => "MobTimer";

    /// <summary>
    /// Description of the plugin.
    /// </summary>
    public string Description => "Timer for mob programming";

    /// <summary>
    /// Additional options for the plugin.
    /// </summary>
    public IEnumerable<PluginAdditionalOption> AdditionalOptions => Settings.GetAdditionalOptions();

    private PluginInitContext Context { get; set; } = null!;

    private static string IconPath => @"Images\mobtimer.png";

    private bool Disposed { get; set; }

    private MobTimerSettings Settings { get; }

    private PluginJsonStorage<MobTimerSession> SessionStorage { get; }

    private MobTimerSession Session { get; set; }

    private IMobTimerService Service { get; }

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

        var search = query.Search.Clean();

        if (search.IsExport())
        {
            return [GetResultForExport()];
        }

        if (search.IsImport())
        {
            return [GetResultForImport()];
        }

        var results = new List<Result>
        {
            GetResultForDuration(Session.Duration),
            GetResultForBreaks(Session.Breaks),
        };
        results.AddRange(Session.Participants.Select(GetResultForParticipant));

        results.Add(Service.HasStarted
            ? GetResultForTimer(Service.TimeElapsed, Service.TimeLeft, Session.Driver)
            : GetResultForStart(Session.Duration));

        if (search.TryGetDuration(out int duration))
        {
            results.Add(GetResultForSetDuration(duration));
        }

        if (search.TryGetBreaks(out int breaks))
        {
            results.Add(GetResultForSetBreaks(breaks));
        }

        if (search.TryGetParticipant(out string name))
        {
            results.Add(GetResultForAddParticipant(name));
        }

        return results;

        Result GetResultForExport() => new()
        {
            QueryTextDisplay = Extensions.Backspace,
            IcoPath = IconPath,
            Title = "Export",
            SubTitle = "Export mob session",
            ToolTipData = new ToolTipData("Export", Session.ToString()),
            ContextData = new Export(),
        };

        Result GetResultForImport() => new()
        {
            QueryTextDisplay = Extensions.Backspace,
            IcoPath = IconPath,
            Title = "Import",
            SubTitle = "Import mob session",
            ToolTipData = new ToolTipData("Import", MobTimerSession.Deserialize(new Import(search).Json)?.ToString()),
            ContextData = new Import(search),
        };

        Result GetResultForStart(Duration value) => new()
        {
            QueryTextDisplay = Extensions.Backspace,
            IcoPath = IconPath,
            Title = "Start timer",
            SubTitle = "Start timer for new rotation",
            ToolTipData = new ToolTipData("New rotation", $"Duration: {value.Format()}\nDriverAssignment: {Settings.DriverAssignment}"),
            ContextData = new Start(value),
            Score = 3000,
        };

        Result GetResultForTimer(TimeSpan elapsed, TimeSpan left, Participant? driver) => new()
        {
            QueryTextDisplay = Extensions.Backspace,
            IcoPath = IconPath,
            Title = $"Timer: {Math.Ceiling(left.TotalMinutes).Format()} left for {driver?.Name ?? "current driver"}",
            SubTitle = "Timer for current rotation",
            ToolTipData = new ToolTipData("Current rotation", $"Timer:\n {elapsed:c} elapsed\n {left:c} left\nDriver: {driver?.Name}"),
            ContextData = new Timer(),
            Score = 3000,
        };

        Result GetResultForDuration(Duration value) => new()
        {
            QueryTextDisplay = Extensions.Backspace,
            IcoPath = IconPath,
            Title = $"Duration: {value.Format()}",
            SubTitle = "Duration in minutes for each rotation",
            ToolTipData = new ToolTipData("Duration", $"{value.Format()} per rotation\n~{value.RotationsPerDay()} rotations per day"),
            ContextData = value,
            Score = 2000,
        };

        Result GetResultForBreaks(Breaks value) => new()
        {
            QueryTextDisplay = Extensions.Backspace,
            IcoPath = IconPath,
            Title = $"Break: {value.Format()}",
            SubTitle = "Take a break after completing a number of rotations",
            ToolTipData = new ToolTipData("Break", $"Take a break {value.Format()}\n~{value.BreaksPerDay(Session.Duration)} breaks per day"),
            ContextData = value,
            Score = 2000 - 1,
        };

        Result GetResultForParticipant(Participant value, int index) => new()
        {
            QueryTextDisplay = Extensions.Backspace,
            IcoPath = IconPath,
            Title = value.Name,
            SubTitle = "Participant of mob session",
            ToolTipData = new ToolTipData(value.Name, $"Rotations:\n{string.Join("\n", value.Rotations.Select(x => " " + x.Begin?.ToString("s") + " + " + x.Duration.Format()))}"),
            ContextData = value,
            Score = 1000 - index,
        };

        Result GetResultForSetDuration(double value) => new()
        {
            QueryTextDisplay = search,
            IcoPath = IconPath,
            Title = $"Set duration: {value.Format()}",
            SubTitle = "Duration in minutes for each rotation",
            ToolTipData = new ToolTipData("Duration", $"{value.Format()} per rotation\n~{value.RotationsPerDay()} rotations per day"),
            ContextData = new Set<Duration>(new Duration(value)),
            Score = 4000,
        };

        Result GetResultForSetBreaks(int value) => new()
        {
            QueryTextDisplay = search,
            IcoPath = IconPath,
            Title = $"Set break: {value.Format()}",
            SubTitle = "Take a break after completing a number of rotations",
            ToolTipData = new ToolTipData("Break", $"Take a break {value.Format()}\n~{value.BreaksPerDay(Session.Duration)} breaks per day"),
            ContextData = new Set<Breaks>(new Breaks(value)),
            Score = 4000 - 1,
        };

        Result GetResultForAddParticipant(string value) => new()
        {
            QueryTextDisplay = search,
            IcoPath = IconPath,
            Title = value,
            SubTitle = "Add participant to mob session",
            ToolTipData = new ToolTipData("Participant", value),
            ContextData = new Add<Participant>(new Participant(value)),
            Score = 4000 - 2,
        };
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
        if (selectedResult?.ContextData is Start start)
        {
            return
            [
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Start (Enter)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE768", // Play
                    AcceleratorKey = Key.Enter,
                    Action = _ =>
                    {
                        Service.Start(start.Duration);
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                },
            ];
        }

        if (selectedResult?.ContextData is Timer _ && Service.IsRunning)
        {
            return
            [
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Pause (Enter)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE769", // Pause
                    AcceleratorKey = Key.Enter,
                    Action = _ =>
                    {
                        Service.Pause();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                },
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Clear (Delete)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE894", // Clear
                    AcceleratorKey = Key.Delete,
                    Action = _ =>
                    {
                        Service.Clear();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                },
            ];
        }

        if (selectedResult?.ContextData is Timer _ && !Service.IsRunning)
        {
            return
            [
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Resume (Enter)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE768", // Play
                    AcceleratorKey = Key.Enter,
                    Action = _ =>
                    {
                        Service.Resume();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                },
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Clear (Delete)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE894", // Clear
                    AcceleratorKey = Key.Delete,
                    Action = _ =>
                    {
                        Service.Clear();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                },
            ];
        }

        if (selectedResult?.ContextData is Duration _)
        {
            return
            [
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Reset to default (Delete)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE894", // Clear
                    AcceleratorKey = Key.Delete,
                    Action = _ =>
                    {
                        Session.ResetDuration();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                }
            ];
        }

        if (selectedResult?.ContextData is Breaks _)
        {
            return
            [
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Reset to default (Delete)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE894", // Clear
                    AcceleratorKey = Key.Delete,
                    Action = _ =>
                    {
                        Session.ResetBreaks();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                }
            ];
        }

        if (selectedResult?.ContextData is Participant participant)
        {
            return
            [
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Select as driver (Enter)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE73E", // CheckMark
                    AcceleratorKey = Key.Enter,
                    Action = _ =>
                    {
                        Session.Driver = participant;
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                },
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Move up (Alt + Up)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE74A", // Up
                    AcceleratorKey = Key.Up,
                    AcceleratorModifiers = ModifierKeys.Alt,
                    Action = _ =>
                    {
                        Session.MoveUp(participant);
                        Save();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                },
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Move down (Alt + Down)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE74B", // Down
                    AcceleratorKey = Key.Down,
                    AcceleratorModifiers = ModifierKeys.Alt,
                    Action = _ =>
                    {
                        Session.MoveDown(participant);
                        Save();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                },
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Remove (Delete)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE74D", // Delete
                    AcceleratorKey = Key.Delete,
                    Action = _ =>
                    {
                        Session.Remove(participant);
                        Save();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                }
            ];
        }

        if (selectedResult?.ContextData is Set<Duration> setDuration)
        {
            return
            [
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Set duration (Enter)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE74E", // Save
                    AcceleratorKey = Key.Enter,
                    Action = _ =>
                    {
                        Session.Duration = setDuration.Value;
                        Save();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                }
            ];
        }

        if (selectedResult?.ContextData is Set<Breaks> setBreaks)
        {
            return
            [
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Set break (Enter)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE74E", // Save
                    AcceleratorKey = Key.Enter,
                    Action = _ =>
                    {
                        Session.Breaks = setBreaks.Value;
                        Save();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                }
            ];
        }

        if (selectedResult?.ContextData is Add<Participant> addParticipant)
        {
            return
            [
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Add participant (Enter)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xE74E", // Save
                    AcceleratorKey = Key.Enter,
                    Action = _ =>
                    {
                        Session.Add(addParticipant.Value);
                        Save();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                }
            ];
        }

        if (selectedResult?.ContextData is Export _)
        {
            return
            [
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Export (Enter)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xEDE1", // Export
                    AcceleratorKey = Key.Enter,
                    Action = _ =>
                    {
                        Session.Clear();
                        var json = Session.Export();
                        var result = "mob import " + json;
                        Clipboard.SetText(result);
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                }
            ];
        }

        if (selectedResult?.ContextData is Import import)
        {
            return
            [
                new ContextMenuResult
                {
                    PluginName = Name,
                    Title = "Import (Enter)",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xEA52", // ImportMirrored
                    AcceleratorKey = Key.Enter,
                    Action = _ =>
                    {
                        Session.Import(import.Json);
                        Session.Clear();
                        Save();
                        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
                        return false;
                    },
                }
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
    }

    /// <summary>
    /// Saves data.
    /// </summary>
    public void Save()
    {
        SessionStorage.Save();
    }

    /// <summary>
    /// Reloads data.
    /// </summary>
    public void ReloadData()
    {
        Session = SessionStorage.Load();
        Session.Clear();
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

        Service.Dispose();

        Disposed = true;
    }

    private void OnMobTimerStarted(object? sender, MobTimerStartedEventArgs e)
    {
        Log.Info("OnMobTimerStarted: " + e.Begin, GetType());
        Session.Begin(new Rotation(e.Duration, Begin: e.Begin), Settings.DriverAssignment);
        Save();
        Context.API.ShowNotification($"Let's go! {Extensions.Stopwatch}", $"{Session.Driver?.Name ?? "The driver"} has {Session.Duration.Format()}{Settings.GetKudos()}");
        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);
    }

    private void OnMobTimerElapsed(object? sender, MobTimerElapsedEventArgs e)
    {
        Log.Info("MobTimerElapsed: " + e.End, GetType());
        var breakTime = Session.End(new Rotation(e.Duration, End: e.End));
        Save();

        if (breakTime)
        {
            Context.API.ShowNotification($"Times up! {Extensions.AlarmClock}", $"{Session.Duration.Format()} has elapsed and {Session.Driver?.Name ?? "the driver"} should finish");
            Context.API.ShowNotification($"Break time! {Extensions.Coffee}", $"It's time for a break {Session.Breaks.Format()}");
        }
        else
        {
            Context.API.ShowNotification($"Times up! {Extensions.AlarmClock}", $"{Session.Duration.Format()} has elapsed and {Session.Driver?.Name ?? "the driver"} should handover");
        }

        Context.API.ChangeQuery(Context.CurrentPluginMetadata.ActionKeyword, true);

        if (Settings.SoundEnabled)
        {
            using var player = new SoundPlayer(Settings.SoundPath);
            player.Play();
        }
    }
}

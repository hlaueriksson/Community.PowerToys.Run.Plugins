using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using Community.PowerToys.Run.Plugin.MobTimer.Models;
using Microsoft.PowerToys.Settings.UI.Library;

namespace Community.PowerToys.Run.Plugin.MobTimer;

/// <summary>
/// Plugin settings.
/// </summary>
public class MobTimerSettings
{
    private readonly List<(string Name, string Path)> _sounds = [];

    private static readonly List<(string Name, string Path)> _windowsSounds =
    [
        ("Alarm01", @"C:\Windows\Media\Alarm01.wav"),
        ("Alarm02", @"C:\Windows\Media\Alarm02.wav"),
        ("Alarm03", @"C:\Windows\Media\Alarm03.wav"),
        ("Alarm04", @"C:\Windows\Media\Alarm04.wav"),
        ("Alarm05", @"C:\Windows\Media\Alarm05.wav"),
        ("Alarm06", @"C:\Windows\Media\Alarm06.wav"),
        ("Alarm07", @"C:\Windows\Media\Alarm07.wav"),
        ("Alarm08", @"C:\Windows\Media\Alarm08.wav"),
        ("Alarm09", @"C:\Windows\Media\Alarm09.wav"),
        ("Alarm10", @"C:\Windows\Media\Alarm10.wav"),
        ("Ring01", @"C:\Windows\Media\Ring01.wav"),
        ("Ring02", @"C:\Windows\Media\Ring02.wav"),
        ("Ring03", @"C:\Windows\Media\Ring03.wav"),
        ("Ring04", @"C:\Windows\Media\Ring04.wav"),
        ("Ring05", @"C:\Windows\Media\Ring05.wav"),
        ("Ring06", @"C:\Windows\Media\Ring06.wav"),
        ("Ring07", @"C:\Windows\Media\Ring07.wav"),
        ("Ring08", @"C:\Windows\Media\Ring08.wav"),
        ("Ring09", @"C:\Windows\Media\Ring09.wav"),
        ("Ring10", @"C:\Windows\Media\Ring10.wav"),
    ];

    private static readonly List<string> _defaultKudos = ["kick ass", "be awesome", "slay", "rock", "crush it", "nail it", "get busy", "flex", "lock in", "hustle"];

    /// <summary>
    /// Initializes a new instance of the <see cref="MobTimerSettings"/> class.
    /// </summary>
    public MobTimerSettings()
    {
        _sounds.AddRange(_windowsSounds);
        _sounds.AddRange(CustomSounds());
    }

    /// <summary>
    /// Assignment of the next driver.
    /// </summary>
    public DriverAssignmentType DriverAssignment { get; set; }

    /// <summary>
    /// Enable sound when rotation ends.
    /// </summary>
    public bool SoundEnabled { get; set; } = true;

    /// <summary>
    /// Path to sound file.
    /// </summary>
    public string SoundPath { get; set; } = _windowsSounds[0].Path;

    /// <summary>
    /// A list of praise to the current driver.
    /// </summary>
    public List<string> Kudos
    {
        get;
        set => field = [.. value.Where(x => x.IsValid())];
    }

    = _defaultKudos;

    internal IEnumerable<PluginAdditionalOption> GetAdditionalOptions()
    {
        return
        [
            new()
            {
                Key = nameof(DriverAssignment),
                DisplayLabel = "Driver assignment",
                DisplayDescription = "How should the next driver be assigned?",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Combobox,
                ComboBoxItems = [.. Enum.GetValues<DriverAssignmentType>().Select(x => new KeyValuePair<string, string>(x.ToString(), x.ToString("D")))],
                ComboBoxValue = (int)DriverAssignment,
            },
            new()
            {
                Key = nameof(SoundPath),
                DisplayLabel = "Sound",
                DisplayDescription = "Sound to play after end of rotation",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.CheckboxAndCombobox,
                Value = SoundEnabled,
                ComboBoxItems = [.. _sounds.Select((x, index) => new KeyValuePair<string, string>(x.Name, index.ToString(CultureInfo.InvariantCulture)))],
                ComboBoxValue = _sounds.FindIndex(x => x.Path == SoundPath),
            },
            new()
            {
                Key = nameof(Kudos),
                DisplayLabel = "Kudos",
                DisplayDescription = "Kudos to the current driver",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.MultilineTextbox,
                TextValueAsMultilineList = Kudos,
            },
        ];
    }

    internal void SetAdditionalOptions(IEnumerable<PluginAdditionalOption> additionalOptions)
    {
        ArgumentNullException.ThrowIfNull(additionalOptions);

        var options = additionalOptions.ToList();

        var assignment = options.Find(x => x.Key == nameof(DriverAssignment));
        var value = (DriverAssignmentType)(assignment?.ComboBoxValue ?? 0);
        DriverAssignment = Enum.IsDefined(value) ? value : DriverAssignmentType.Manual;

        var sound = options.Find(x => x.Key == nameof(SoundPath));
        SoundEnabled = sound?.Value ?? true;
        var index = sound?.ComboBoxValue ?? 0;
        SoundPath = _sounds[index >= 0 && index < _sounds.Count ? index : 0].Path;

        var kudos = options.Find(x => x.Key == nameof(Kudos));
        Kudos = kudos?.TextValueAsMultilineList ?? _defaultKudos;
    }

    internal static List<(string Name, string Path)> CustomSounds()
    {
        var fileSystem = new FileSystem();
        var path = fileSystem.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (path == null)
        {
            return [];
        }

        var files = fileSystem.Directory.GetFiles(path, "*.wav", SearchOption.AllDirectories);

        return [.. files.Select(x => (fileSystem.Path.GetFileNameWithoutExtension(x), x))];
    }

    internal string GetKudos() => Kudos.Count != 0 ? " to " + Kudos[new Random().Next(Kudos.Count)] : string.Empty;
}

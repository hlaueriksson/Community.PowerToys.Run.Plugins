using Community.PowerToys.Run.Plugin.MobTimer.Models;

namespace Community.PowerToys.Run.Plugin.MobTimer;

internal static class Extensions
{
    public const string Backspace = "\b";

    public const string Stopwatch = "⏱️";

    public const string AlarmClock = "⏰";

    public const string Coffee = "☕";

    public static string Clean(this string value) => value.Replace(Backspace, string.Empty, StringComparison.Ordinal).Trim();

    public static bool IsExport(this string value) => value == "export";

    public static bool IsImport(this string value) => value.StartsWith("import", StringComparison.Ordinal);

    public static bool TryGetDuration(this string value, out int duration) => int.TryParse(value.Trim(), out duration) && duration is >= 1 and <= 99;

    public static bool TryGetBreaks(this string value, out int breaks) => int.TryParse(value.Trim(), out breaks) && breaks is >= 1 and <= 9;

    public static bool TryGetParticipant(this string value, out string name)
    {
        name = value.Trim();
        return !string.IsNullOrWhiteSpace(value) && !TryGetDuration(value, out _);
    }

    public static bool IsValid(this Duration duration) => duration.Value is >= 1 and <= 99;

    public static bool IsValid(this Breaks breaks) => breaks.Value is >= 1 and <= 9;

    public static bool IsValid(this Participant participant) => participant.Name.IsValid();

    public static bool IsValid(this string value) => !string.IsNullOrWhiteSpace(value);

    public static string Format(this double value)
    {
        return value + (value.IsPlural() ? " minutes" : " minute");
    }

    public static string Format(this Duration duration)
    {
        return duration.Value.Format();
    }

    public static string Format(this int value)
    {
        return value.IsPlural() ? $"every {value} rotations" : $"after {value} rotation";
    }

    public static string Format(this Breaks breaks)
    {
        return breaks.Value.Format();
    }

    public static int RotationsPerDay(this double value)
    {
        return (int)(TimeSpan.FromHours(6).TotalMinutes / value);
    }

    public static int RotationsPerDay(this Duration duration)
    {
        return duration.Value.RotationsPerDay();
    }

    public static int BreaksPerDay(this int value, Duration duration)
    {
        return duration.RotationsPerDay() / value;
    }

    public static int BreaksPerDay(this Breaks breaks, Duration duration)
    {
        return breaks.Value.BreaksPerDay(duration);
    }

    public static bool IsPlural(this double value) => value > 1;

    public static bool IsPlural(this int value) => value > 1;

    public static bool IsExpired(this Rotation rotation)
    {
        return rotation.End < DateTime.Today;
    }
}

namespace Community.PowerToys.Run.Plugin.MobTimer.Models;

public enum DriverAssignmentType
{
    Manual,
    Sequential,
    Random,
}

public record Start(Duration Duration);

public record Timer;

public record Duration(double Value);

public record Breaks(int Value);

public record Participant(string Name)
{
    public List<Rotation> Rotations { get; set; } = [];
}

public record Rotation(Duration Duration, DateTime? Begin = null, DateTime? End = null)
{
    public DateTime? End { get; set; } = End;
}

public record Set<T>(T Value);

public record Add<T>(T Value);

public record Export;

public record Import
{
    public Import(string query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Json = query.Replace("import", string.Empty, StringComparison.Ordinal).Trim();
    }

    public string Json { get; }
}

using System.Text.Json;
using Community.PowerToys.Run.Plugin.MobTimer.Models;

namespace Community.PowerToys.Run.Plugin.MobTimer;

/// <summary>
/// Represents a mob session.
/// </summary>
public class MobTimerSession
{
    private static readonly Duration _defaultDuration = new(20);
    private static readonly Breaks _defaultBreaks = new(2);
    private static readonly List<Participant> _defaultParticipants = [new("Alice"), new("Bob"), new("Charlie")];

    /// <summary>
    /// Gets or sets the duration of rotations.
    /// </summary>
    public Duration Duration
    {
        get;
        set => field = value?.IsValid() == true ? value : _defaultDuration;
    }

    = _defaultDuration;

    /// <summary>
    /// Gets or sets the number of rotations before a break.
    /// </summary>
    public Breaks Breaks
    {
        get;
        set => field = value?.IsValid() == true ? value : _defaultBreaks;
    }

    = _defaultBreaks;

    /// <summary>
    /// Gets or sets the participants of the mob session.
    /// </summary>
    public List<Participant> Participants
    {
        get;
        set => field = [.. value.Where(x => x.IsValid())];
    }

    = _defaultParticipants;

    /// <summary>
    /// Gets or sets the current driver.
    /// </summary>
    public Participant? Driver { get; set; }

    /// <summary>
    /// Gets or sets the previous rotations of the mob session.
    /// </summary>
    public List<Rotation> Rotations { get; set; } = [];

    /// <summary>
    /// Clears the mob session of expired rotations.
    /// </summary>
    public void Clear()
    {
        foreach (var participant in Participants)
        {
            participant.Rotations.RemoveAll(x => x.IsExpired());
        }

        Rotations.RemoveAll(x => x.IsExpired());
    }

    /// <summary>
    /// Resets the duration of rotations to the default value.
    /// </summary>
    public void ResetDuration()
    {
        Duration = _defaultDuration;
    }

    /// <summary>
    /// Resets the number of rotations before a break to the default value.
    /// </summary>
    public void ResetBreaks()
    {
        Breaks = _defaultBreaks;
    }

    /// <summary>
    /// Moves the participant up in the list.
    /// </summary>
    /// <param name="participant">The participant.</param>
    public void MoveUp(Participant participant)
    {
        var index = Participants.IndexOf(participant);

        if (index <= 0)
        {
            return;
        }

        Participants.RemoveAt(index);
        Participants.Insert(--index, participant);
    }

    /// <summary>
    /// Moves the participant down in the list.
    /// </summary>
    /// <param name="participant">The participant.</param>
    public void MoveDown(Participant participant)
    {
        var index = Participants.IndexOf(participant);

        if (index < 0 || index >= Participants.Count - 1)
        {
            return;
        }

        Participants.RemoveAt(index);
        Participants.Insert(++index, participant);
    }

    /// <summary>
    /// Adds a participant to the mob session.
    /// </summary>
    /// <param name="participant">The participant.</param>
    public void Add(Participant participant)
    {
        if (!Participants.Contains(participant))
        {
            Participants.Add(participant);
        }
    }

    /// <summary>
    /// Removes a participant from the mob session.
    /// </summary>
    /// <param name="participant">The participant.</param>
    public void Remove(Participant participant)
    {
        Participants.Remove(participant);
    }

    /// <summary>
    /// Begins a new rotation.
    /// </summary>
    /// <param name="rotation">The rotation.</param>
    /// <param name="type">The <see cref="DriverAssignmentType"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="type"/> is not a valid <see cref="DriverAssignmentType"/>.</exception>
    public void Begin(Rotation rotation, DriverAssignmentType type)
    {
        Rotations.Add(rotation);
        Driver = GetNextDriver();

        Participant? GetNextDriver()
        {
            return type switch
            {
                DriverAssignmentType.Manual => null,
                DriverAssignmentType.Sequential => Participants[(Participants.IndexOf(Driver!) + 1) % Participants.Count],
                DriverAssignmentType.Random => Participants[RandomIndex()],
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
        }

        int RandomIndex()
        {
            var random = new Random();
            int result;
            do
            {
                result = random.Next(Participants.Count);
            }
            while (result == Participants.IndexOf(Driver!));

            return result;
        }
    }

    /// <summary>
    /// Ends the current rotation.
    /// </summary>
    /// <param name="rotation">The rotation.</param>
    /// <returns><see langword="true"/> if it is time for a break; <see langword="false"/>, otherwise.</returns>
    public bool End(Rotation rotation)
    {
        ArgumentNullException.ThrowIfNull(rotation);

        var current = Rotations.LastOrDefault();

        if (current == null)
        {
            return false;
        }

        current.End = rotation.End;
        Driver?.Rotations.Add(current);

        return Rotations.Count % Breaks.Value == 0;
    }

    /// <summary>
    /// Exports the mob session to a JSON string.
    /// </summary>
    /// <returns>A JSON string.</returns>
    public string Export()
    {
        return JsonSerializer.Serialize(this);
    }

    /// <summary>
    /// Deserializes a JSON string to a <see cref="MobTimerSession"/>.
    /// </summary>
    /// <param name="json">A JSON string.</param>
    /// <returns>A <see cref="MobTimerSession"/>.</returns>
    public static MobTimerSession? Deserialize(string json)
    {
        return JsonSerializer.Deserialize<MobTimerSession>(json);
    }

    /// <summary>
    /// Imports a JSON string to the mob session.
    /// </summary>
    /// <param name="json">A JSON string.</param>
    public void Import(string json)
    {
        var result = Deserialize(json);

        if (result == null)
        {
            return;
        }

        Duration = result.Duration;
        Breaks = result.Breaks;
        Participants = result.Participants;
        Driver = result.Driver;
        Rotations = result.Rotations;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return
            $"""
            Duration: {Duration.Format()}
            Breaks: {Breaks.Format()}
            Participants: {string.Join(", ", Participants.Select(x => x.Name))}
            Driver: {Driver?.Name}
            Rotations: {Rotations.Count}
            """;
    }
}

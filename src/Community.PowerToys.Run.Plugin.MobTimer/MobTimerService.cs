using System.Diagnostics;
using System.Timers;
using Community.PowerToys.Run.Plugin.MobTimer.Models;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.MobTimer;

/// <summary>
/// Timer service.
/// </summary>
public interface IMobTimerService
{
    /// <summary>
    /// Occurs when the timer has started.
    /// </summary>
    event EventHandler<MobTimerStartedEventArgs>? MobTimerStarted;

    /// <summary>
    /// Occurs when the timer has elapsed.
    /// </summary>
    event EventHandler<MobTimerElapsedEventArgs>? MobTimerElapsed;

    /// <summary>
    /// Gets a value indicating whether the timer is running.
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// Gets a value indicating whether the timer has started.
    /// </summary>
    bool HasStarted { get; }

    /// <summary>
    /// Gets a value indicating whether the timer has elapsed.
    /// </summary>
    bool HasElapsed { get; }

    /// <summary>
    /// Gets the time elapsed.
    /// </summary>
    TimeSpan TimeElapsed { get; }

    /// <summary>
    /// Gets the time left.
    /// </summary>
    TimeSpan TimeLeft { get; }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    /// <param name="duration">The duration of the rotation.</param>
    void Start(Duration duration);

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    void Pause();

    /// <summary>
    /// Resumes the timer.
    /// </summary>
    void Resume();

    /// <summary>
    /// Clears the timer.
    /// </summary>
    void Clear();

    /// <summary>
    /// Disposes of the timer.
    /// </summary>
    void Dispose();
}

/// <summary>
/// Provides data for the <see cref="MobTimerService.MobTimerStarted"/> event.
/// </summary>
/// <param name="begin">Time when the timer started.</param>
/// <param name="duration">The duration of the rotation.</param>
public class MobTimerStartedEventArgs(DateTime begin, Duration duration) : EventArgs
{
    /// <summary>
    /// Gets the time when the timer started.
    /// </summary>
    public DateTime Begin { get; } = begin;

    /// <summary>
    /// Gets the duration of the rotation.
    /// </summary>
    public Duration Duration { get; } = duration;
}

/// <summary>
/// Provides data for the <see cref="MobTimerService.MobTimerElapsed"/> event.
/// </summary>
/// <param name="end">Time when the timer elapsed.</param>
/// <param name="duration">The duration of the rotation.</param>
public class MobTimerElapsedEventArgs(DateTime end, Duration duration) : EventArgs
{
    /// <summary>
    /// Gets the time when the timer elapsed.
    /// </summary>
    public DateTime End { get; } = end;

    /// <summary>
    /// Gets the duration of the rotation.
    /// </summary>
    public Duration Duration { get; } = duration;
}

/// <inheritdoc/>
public class MobTimerService : IMobTimerService, IDisposable
{
    private readonly Stopwatch _stopwatch;
    private readonly System.Timers.Timer _timer;
    private Duration? _duration;

    /// <summary>
    /// Initializes a new instance of the <see cref="MobTimerService"/> class.
    /// </summary>
    public MobTimerService()
    {
        _stopwatch = new Stopwatch();
        _timer = new System.Timers.Timer();
        _timer.Elapsed += OnElapsed;
        _timer.AutoReset = false;
    }

    /// <inheritdoc/>
    public event EventHandler<MobTimerStartedEventArgs>? MobTimerStarted;

    /// <inheritdoc/>
    public event EventHandler<MobTimerElapsedEventArgs>? MobTimerElapsed;

    /// <inheritdoc/>
    public bool IsRunning => _timer.Enabled;

    /// <inheritdoc/>
    public bool HasStarted { get; private set; }

    /// <inheritdoc/>
    public bool HasElapsed { get; private set; }

    /// <inheritdoc/>
    public TimeSpan TimeElapsed => _stopwatch.Elapsed;

    /// <inheritdoc/>
    public TimeSpan TimeLeft => TimeSpan.FromMinutes(_duration?.Value ?? 0) - _stopwatch.Elapsed;

    private bool Disposed { get; set; }

    /// <inheritdoc/>
    public void Start(Duration duration)
    {
        _duration = duration;
        _stopwatch.Restart();
        _timer.Interval = TimeSpan.FromMinutes(_duration.Value).TotalMilliseconds;
        _timer.Start();
        HasStarted = true;
        MobTimerStarted?.Invoke(this, new MobTimerStartedEventArgs(DateTime.Now, _duration));
    }

    /// <inheritdoc/>
    public void Pause()
    {
        _stopwatch.Stop();
        _timer.Stop();
    }

    /// <inheritdoc/>
    public void Resume()
    {
        _stopwatch.Start();
        _timer.Start();
        _timer.Interval = TimeLeft.TotalMilliseconds; // FIX: The raise of Elapsed event can fail
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _stopwatch.Reset();
        _timer.Stop();
        HasStarted = false;
        HasElapsed = false;
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

        _timer.Dispose();

        Disposed = true;
    }

    private void OnElapsed(object? sender, ElapsedEventArgs e)
    {
        Log.Info("OnElapsed: " + e.SignalTime, GetType());
        HasStarted = false;
        HasElapsed = true;
        MobTimerElapsed?.Invoke(this, new MobTimerElapsedEventArgs(e.SignalTime, _duration!));
    }
}

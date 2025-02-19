using Community.PowerToys.Run.Plugin.MobTimer.Models;
using FluentAssertions;

namespace Community.PowerToys.Run.Plugin.MobTimer.UnitTests;

[TestClass]
public class MobTimerServiceTests
{
    [TestMethod]
    public void Ctor()
    {
        var subject = new MobTimerService();

        subject.IsRunning.Should().BeFalse();
        subject.HasStarted.Should().BeFalse();
        subject.HasElapsed.Should().BeFalse();
        subject.TimeElapsed.Ticks.Should().Be(0);

        subject.Dispose();
    }

    [TestMethod]
    public void Start()
    {
        var subject = new MobTimerService();
        subject.Start(new Duration(1));

        subject.IsRunning.Should().BeTrue();
        subject.HasStarted.Should().BeTrue();
        subject.HasElapsed.Should().BeFalse();
        subject.TimeElapsed.Ticks.Should().BePositive();
        subject.TimeLeft.Ticks.Should().BePositive();

        var total = subject.TimeElapsed + subject.TimeLeft;
        (total > TimeSpan.FromSeconds(59)).Should().BeTrue();
        (total < TimeSpan.FromSeconds(61)).Should().BeTrue();

        subject.Dispose();
    }

    [TestMethod]
    public void Pause()
    {
        var subject = new MobTimerService();
        subject.Start(new Duration(1));
        subject.Pause();

        subject.IsRunning.Should().BeFalse();
        subject.HasStarted.Should().BeTrue();
        subject.HasElapsed.Should().BeFalse();
        subject.TimeElapsed.Ticks.Should().BePositive();
        subject.TimeLeft.Ticks.Should().BePositive();

        subject.Dispose();
    }

    [TestMethod]
    public void Resume()
    {
        var subject = new MobTimerService();
        subject.Start(new Duration(1));
        subject.Pause();
        subject.Resume();

        subject.IsRunning.Should().BeTrue();
        subject.HasStarted.Should().BeTrue();
        subject.HasElapsed.Should().BeFalse();
        subject.TimeElapsed.Ticks.Should().BePositive();
        subject.TimeLeft.Ticks.Should().BePositive();

        subject.Dispose();
    }

    [TestMethod]
    public void Clear()
    {
        var subject = new MobTimerService();
        subject.Start(new Duration(1));
        subject.Clear();

        subject.IsRunning.Should().BeFalse();
        subject.HasStarted.Should().BeFalse();
        subject.HasElapsed.Should().BeFalse();
        subject.TimeElapsed.Ticks.Should().Be(0);

        subject.Dispose();
    }
}

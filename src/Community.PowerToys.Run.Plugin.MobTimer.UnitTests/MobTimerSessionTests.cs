using Community.PowerToys.Run.Plugin.MobTimer.Models;
using FluentAssertions;

namespace Community.PowerToys.Run.Plugin.MobTimer.UnitTests;

[TestClass]
public class MobTimerSessionTests
{
    [TestMethod]
    public void Ctor()
    {
        var subject = new MobTimerSession();

        subject.Duration.Should().Be(new Duration(20));
        subject.Breaks.Should().Be(new Breaks(2));
        subject.Participants.Should().HaveCount(3);
        subject.Driver.Should().BeNull();
        subject.Rotations.Should().BeEmpty();
    }

    [TestMethod]
    public void Validation()
    {
        var subject = new MobTimerSession
        {
            Duration = new Duration(0),
            Breaks = new Breaks(0),
            Participants = [new Participant("")],
        };
        subject.Duration.Should().Be(new Duration(20));
        subject.Breaks.Should().Be(new Breaks(2));
        subject.Participants.Should().BeEmpty();
    }

    [TestMethod]
    public void Clear()
    {
        var subject = new MobTimerSession();
        AddRotation(DateTime.Now.AddDays(-1));
        AddRotation(DateTime.Now);

        subject.Clear();

        subject.Participants.Should().AllSatisfy(x =>
        {
            x.Rotations.Should().AllSatisfy(y => y.IsExpired().Should().BeFalse());
        });
        subject.Rotations.Should().AllSatisfy(x => x.IsExpired().Should().BeFalse());

        void AddRotation(DateTime end)
        {
            var rotation = new Rotation(new(1))
            {
                End = end
            };
            foreach (var participant in subject.Participants)
            {
                participant.Rotations.Add(rotation);
            }
            subject.Rotations.Add(rotation);
        }
    }

    [TestMethod]
    public void Participants()
    {
        var subject = new MobTimerSession();
        var alice = subject.Participants.First();
        var charlie = subject.Participants.Last();

        subject.MoveUp(alice);
        subject.Participants.First().Should().Be(alice);

        subject.MoveDown(charlie);
        subject.Participants.Last().Should().Be(charlie);

        subject.MoveDown(alice);
        subject.MoveDown(alice);
        subject.Participants.Last().Should().Be(alice);

        subject.MoveUp(alice);
        subject.MoveUp(alice);
        subject.Participants.First().Should().Be(alice);
    }

    [TestMethod]
    public void Begin_Rotation()
    {
        var subject = new MobTimerSession();

        var rotation = Rotation();
        subject.Begin(rotation, DriverAssignmentType.Manual);
        subject.Driver.Should().BeNull();
        subject.Rotations.Last().Should().Be(rotation);

        rotation = Rotation();
        subject.Begin(rotation, DriverAssignmentType.Sequential);
        subject.Driver.Should().Be(subject.Participants.First());
        subject.Rotations.Last().Should().Be(rotation);

        rotation = Rotation();
        subject.Begin(rotation, DriverAssignmentType.Random);
        subject.Driver.Should().NotBe(subject.Participants.First());
        subject.Rotations.Last().Should().Be(rotation);

        static Rotation Rotation()
        {
            return new Rotation(new Duration(1)) { End = DateTime.Now };
        }
    }

    [TestMethod]
    public void End_Rotation()
    {
        var subject = new MobTimerSession();

        var rotation = Rotation();
        subject.Begin(rotation, DriverAssignmentType.Sequential);
        subject.End(rotation).Should().BeFalse();
        subject.Driver!.Rotations.Last().Should().Be(rotation);

        rotation = Rotation();
        subject.Begin(rotation, DriverAssignmentType.Sequential);
        subject.End(rotation).Should().BeTrue();
        subject.Driver.Rotations.Last().Should().Be(rotation);

        rotation = Rotation();
        subject.Begin(rotation, DriverAssignmentType.Sequential);
        subject.End(rotation).Should().BeFalse();
        subject.Driver.Rotations.Last().Should().Be(rotation);

        static Rotation Rotation()
        {
            return new Rotation(new Duration(1)) { End = DateTime.Now };
        }
    }

    [TestMethod]
    public void Export()
    {
        var session = new MobTimerSession();

        var result = session.Export();
        result.Should().NotBeEmpty();
    }

    [TestMethod]
    public void Import()
    {
        var json = "{}";
        var session = new MobTimerSession();

        session.Import(json);
        session.Should().BeEquivalentTo(new MobTimerSession());
    }
}

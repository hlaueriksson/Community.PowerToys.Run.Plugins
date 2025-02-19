using Community.PowerToys.Run.Plugin.MobTimer.Models;
using FluentAssertions;
using Moq;
using Wox.Plugin;
using Timer = Community.PowerToys.Run.Plugin.MobTimer.Models.Timer;

namespace Community.PowerToys.Run.Plugin.MobTimer.UnitTests;

[TestClass]
public class MainTests
{
    private Mock<IMobTimerService> _mock = null!;
    private Main _subject = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        var settings = new MobTimerSettings();
        var session = new MobTimerSession();
        _mock = new Mock<IMobTimerService>();

        _subject = new Main(settings, session, _mock.Object);
    }

    [TestMethod]
    public void Query_export()
    {
        _subject.Query(new("export"))
            .Should().BeEquivalentTo(
            [
                new Result { Title = "Export", SubTitle = "Export mob session", IcoPath = @"Images\mobtimer.png" },
            ]);
    }

    [TestMethod]
    public void Query_import()
    {
        _subject.Query(new("import {}"))
            .Should().BeEquivalentTo(
            [
                new Result { Title = "Import", SubTitle = "Import mob session", IcoPath = @"Images\mobtimer.png" },
            ]);
    }

    [TestMethod]
    public void Query_()
    {
        _subject.Query(new(""))
            .Should().BeEquivalentTo(
            [
                new Result { Title = "Start timer", SubTitle = "Start timer for new rotation", IcoPath = @"Images\mobtimer.png" },
                new Result { Title = "Duration: 20 minutes", SubTitle = "Duration in minutes for each rotation", IcoPath = @"Images\mobtimer.png" },
                new Result { Title = "Break: every 2 rotations", SubTitle = "Take a break after completing a number of rotations", IcoPath = @"Images\mobtimer.png" },
                new Result { Title = "Alice", SubTitle = "Participant of mob session", IcoPath = @"Images\mobtimer.png" },
                new Result { Title = "Bob", SubTitle = "Participant of mob session", IcoPath = @"Images\mobtimer.png" },
                new Result { Title = "Charlie", SubTitle = "Participant of mob session", IcoPath = @"Images\mobtimer.png" },
            ]);
    }

    [TestMethod]
    public void Query_int()
    {
        _subject.Query(new("3"))
            .Should().ContainEquivalentOf(
                new Result { Title = "Set duration: 3 minutes", SubTitle = "Duration in minutes for each rotation", IcoPath = @"Images\mobtimer.png" }
            )
            .And.ContainEquivalentOf(
                new Result { Title = "Set break: every 3 rotations", SubTitle = "Take a break after completing a number of rotations", IcoPath = @"Images\mobtimer.png" }
            );
    }

    [TestMethod]
    public void Query_string()
    {
        _subject.Query(new("Dave"))
            .Should().ContainEquivalentOf(
                new Result { Title = "Dave", SubTitle = "Add participant to mob session", IcoPath = @"Images\mobtimer.png" }
            );
    }

    [TestMethod]
    public void LoadContextMenus_Start()
    {
        var result = new Result { ContextData = new Start(new(1)) };
        _subject.LoadContextMenus(result)
            .Should().HaveCount(1)
            .And.Contain(x => x.Title == "Start (Enter)");
    }

    [TestMethod]
    public void LoadContextMenus_Timer_IsRunning()
    {
        _mock.Setup(x => x.IsRunning).Returns(true);
        var result = new Result { ContextData = new Timer() };
        _subject.LoadContextMenus(result)
            .Should().HaveCount(2)
            .And.Contain(x => x.Title == "Pause (Enter)")
            .And.Contain(x => x.Title == "Clear (Delete)");
    }

    [TestMethod]
    public void LoadContextMenus_Timer_IsNotRunning()
    {
        _mock.Setup(x => x.IsRunning).Returns(false);
        var result = new Result { ContextData = new Timer() };
        _subject.LoadContextMenus(result)
            .Should().HaveCount(2)
            .And.Contain(x => x.Title == "Resume (Enter)")
            .And.Contain(x => x.Title == "Clear (Delete)");
    }

    [TestMethod]
    public void LoadContextMenus_Duration()
    {
        var result = new Result { ContextData = new Duration(1) };
        _subject.LoadContextMenus(result)
            .Should().HaveCount(1)
            .And.Contain(x => x.Title == "Reset to default (Delete)");
    }

    [TestMethod]
    public void LoadContextMenus_Breaks()
    {
        var result = new Result { ContextData = new Breaks(1) };
        _subject.LoadContextMenus(result)
            .Should().HaveCount(1)
            .And.Contain(x => x.Title == "Reset to default (Delete)");
    }

    [TestMethod]
    public void LoadContextMenus_Participant()
    {
        var result = new Result { ContextData = new Participant("A") };
        _subject.LoadContextMenus(result)
            .Should().HaveCount(4)
            .And.Contain(x => x.Title == "Select as driver (Enter)")
            .And.Contain(x => x.Title == "Move up (Alt + Up)")
            .And.Contain(x => x.Title == "Move down (Alt + Down)")
            .And.Contain(x => x.Title == "Remove (Delete)");
    }

    [TestMethod]
    public void LoadContextMenus_Set_Duration()
    {
        var result = new Result { ContextData = new Set<Duration>(new(1)) };
        _subject.LoadContextMenus(result)
            .Should().HaveCount(1)
            .And.Contain(x => x.Title == "Set duration (Enter)");
    }

    [TestMethod]
    public void LoadContextMenus_Set_Breaks()
    {
        var result = new Result { ContextData = new Set<Breaks>(new(1)) };
        _subject.LoadContextMenus(result)
            .Should().HaveCount(1)
            .And.Contain(x => x.Title == "Set break (Enter)");
    }

    [TestMethod]
    public void LoadContextMenus_Add_Participant()
    {
        var result = new Result { ContextData = new Add<Participant>(new("A")) };
        _subject.LoadContextMenus(result)
            .Should().HaveCount(1)
            .And.Contain(x => x.Title == "Add participant (Enter)");
    }

    [TestMethod]
    public void LoadContextMenus_Export()
    {
        var result = new Result { ContextData = new Export() };
        _subject.LoadContextMenus(result)
            .Should().HaveCount(1)
            .And.Contain(x => x.Title == "Export (Enter)");
    }

    [TestMethod]
    public void LoadContextMenus_Import()
    {
        var result = new Result { ContextData = new Import("import {}") };
        _subject.LoadContextMenus(result)
            .Should().HaveCount(1)
            .And.Contain(x => x.Title == "Import (Enter)");
    }
}

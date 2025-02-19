using Community.PowerToys.Run.Plugin.MobTimer.Models;
using FluentAssertions;

namespace Community.PowerToys.Run.Plugin.MobTimer.UnitTests;

[TestClass]
public class ExtensionsTests
{
    [TestMethod]
    public void Clean_should_remove_Backspace()
    {
        $"{Extensions.Backspace} 25".Clean().Should().Be("25");
    }

    [TestMethod]
    public void IsExport_should_check_query()
    {
        "export".IsExport().Should().BeTrue();
        "import".IsExport().Should().BeFalse();
    }

    [TestMethod]
    public void IsImport_should_check_query()
    {
        "import {}".IsImport().Should().BeTrue();
        "export".IsImport().Should().BeFalse();
    }

    [TestMethod]
    public void TryGetDuration_should_parse_int()
    {
        "25".TryGetDuration(out int duration).Should().BeTrue();
        duration.Should().Be(25);

        "25a".TryGetDuration(out duration).Should().BeFalse();
        duration.Should().Be(0);
    }

    [TestMethod]
    public void TryGetBreaks_should_parse_int()
    {
        "3".TryGetBreaks(out int breaks).Should().BeTrue();
        breaks.Should().Be(3);

        "3a".TryGetBreaks(out breaks).Should().BeFalse();
        breaks.Should().Be(0);
    }

    [TestMethod]
    public void TryGetParticipant_should_parse_string()
    {
        "Dave".TryGetParticipant(out string name).Should().BeTrue();
        name.Should().Be("Dave");

        "".TryGetParticipant(out name).Should().BeFalse();
        name.Should().Be("");
    }

    [TestMethod]
    public void IsValid_Duration_should_validate_duration()
    {
        new Duration(25).IsValid().Should().BeTrue();
        new Duration(0).IsValid().Should().BeFalse();
    }

    [TestMethod]
    public void IsValid_Breaks_should_validate_breaks()
    {
        new Breaks(3).IsValid().Should().BeTrue();
        new Breaks(0).IsValid().Should().BeFalse();
    }

    [TestMethod]
    public void IsValid_Participant_should_validate_name()
    {
        new Participant("Dave").IsValid().Should().BeTrue();
        new Participant("").IsValid().Should().BeFalse();
    }

    [TestMethod]
    public void IsValid_string_should_validate()
    {
        "Dave".IsValid().Should().BeTrue();
        "".IsValid().Should().BeFalse();
    }

    [TestMethod]
    public void Format_double_should_format_time()
    {
        25.0.Format().Should().Be("25 minutes");
        1.0.Format().Should().Be("1 minute");
    }

    [TestMethod]
    public void Format_Duration_should_format_time()
    {
        new Duration(25).Format().Should().Be("25 minutes");
        new Duration(1).Format().Should().Be("1 minute");
    }

    [TestMethod]
    public void Format_int_should_format_periodicity()
    {
        3.Format().Should().Be("every 3 rotations");
        1.Format().Should().Be("after 1 rotation");
    }

    [TestMethod]
    public void Format_Breaks_should_format_periodicity()
    {
        new Breaks(3).Format().Should().Be("every 3 rotations");
        new Breaks(1).Format().Should().Be("after 1 rotation");
    }

    [TestMethod]
    public void RotationsPerDay_double_should_calculate()
    {
        25.0.RotationsPerDay().Should().Be(14);
    }

    [TestMethod]
    public void RotationsPerDay_Duration_should_calculate()
    {
        new Duration(25).RotationsPerDay().Should().Be(14);
    }

    [TestMethod]
    public void BreaksPerDay_int_should_calculate()
    {
        3.BreaksPerDay(new Duration(25)).Should().Be(4);
    }

    [TestMethod]
    public void BreaksPerDay_Breaks_should_calculate()
    {
        new Breaks(3).BreaksPerDay(new Duration(25)).Should().Be(4);
    }

    [TestMethod]
    public void IsPlural_double_should_evaluate()
    {
        2.0.IsPlural().Should().BeTrue();
        1.0.IsPlural().Should().BeFalse();
    }

    [TestMethod]
    public void IsPlural_int_should_evaluate()
    {
        2.IsPlural().Should().BeTrue();
        1.IsPlural().Should().BeFalse();
    }

    [TestMethod]
    public void IsExpired_Rotation_should_evaluate_End()
    {
        var subject = new Rotation(new(1));
        subject.IsExpired().Should().BeFalse();

        subject.End = DateTime.Now;
        subject.IsExpired().Should().BeFalse();

        subject.End = DateTime.Now.AddDays(-1);
        subject.IsExpired().Should().BeTrue();
    }
}
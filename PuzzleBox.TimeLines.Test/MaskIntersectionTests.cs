using NodaTime;
using NUnit.Framework;
using PuzzleBox.Time.Timelines;
using PuzzleBox.TimeLines.Test;

namespace PuzzleBox.Time.Tests
{
  public class MaskIntersectionTests
  {
    [Test]
    public void Intersect_Empty()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local);

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_ToEmpty()
    {
      var timeline1 = new TimelineMask(TimeZones.Local);
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_Before()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_TouchingStart()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 4, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_OverlappingStart()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0))).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_Starting()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0))).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_Inside()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0))).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_Equal()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0))).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_Outside()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 3, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0))).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_Ending()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 6, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 6, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0))).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_OverlappingEnd()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0))).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_TouchingEnd()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 8, 0, 0), new LocalDateTime(2008, 1, 10, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }

    [Test]
    public void Intersect_After()
    {
      var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
      var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 10, 0, 0), new LocalDateTime(2008, 1, 12, 0, 0)));

      var actual = (timeline1 & timeline2).GetIntervals();
      var expected = new TimelineMask(TimeZones.Local).GetIntervals();

      Assert.That(TimeMath.Equals(expected, actual), Is.True);
    }
  }
}

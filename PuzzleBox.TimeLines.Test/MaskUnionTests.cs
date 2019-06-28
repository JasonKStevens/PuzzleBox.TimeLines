using NodaTime;
using NUnit.Framework;
using PuzzleBox.Time.Timelines;
using PuzzleBox.TimeLines.Test;

namespace PuzzleBox.Time.Tests
{
    public class MaskUnionTests
    {
        [Test]
        public void Union_Empty()
        {
            var timeline1 = new TimelineMask(TimeZones.Local);
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_ToEmpty()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local);

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_Before()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)),
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_TouchingStart()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 4, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0))).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_OverlappingStart()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0))).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_Inside()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0))).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_Starting()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0))).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_Equal()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0))).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_Outside()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 3, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 3, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0))).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_Ending()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0))).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_OverlappingEnd()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0))).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_TouchingEnd()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 8, 0, 0), new LocalDateTime(2008, 1, 10, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 10, 0, 0))).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Union_After()
        {
            var timeline1 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)));
            var timeline2 = new TimelineMask(TimeZones.Local, new LocalInterval(new LocalDateTime(2008, 1, 10, 0, 0), new LocalDateTime(2008, 1, 12, 0, 0)));

            var actual = (timeline1 | timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)),
        new LocalInterval(new LocalDateTime(2008, 1, 10, 0, 0), new LocalDateTime(2008, 1, 12, 0, 0)),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }
    }
}

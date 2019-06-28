using NodaTime;
using NUnit.Framework;
using PuzzleBox.Time.Payloads;
using PuzzleBox.Time.Timelines;
using PuzzleBox.TimeLines.Test;

namespace PuzzleBox.Time.Tests
{
    public class StringContentAddTests
    {
        [Test]
        public void Add_Empty()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)).AddPayload(new StringPayload("Craft")));
            var timeline2 = new TimelinePayload<StringPayload>();

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)).AddPayload(new StringPayload("Craft")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_ToEmpty()
        {
            var timeline1 = new TimelinePayload<StringPayload>();
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)).AddPayload(new StringPayload("Craft")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)).AddPayload(new StringPayload("Craft")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Before()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)).AddPayload(new StringPayload("Craft")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)).AddPayload(new StringPayload("Craft")),
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_TouchingStart()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 4, 0, 0)).AddPayload(new StringPayload("Craft")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_OverlappingStart()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("A")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0)).AddPayload(new StringPayload("B")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 4, 0, 0)).AddPayload(new StringPayload("B")),
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("A")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Starting()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("A")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)).AddPayload(new StringPayload("B")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)).AddPayload(new StringPayload("B")),
        new LocalInterval(new LocalDateTime(2008, 1, 6, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("A")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Inside()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("A")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)).AddPayload(new StringPayload("B")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0)).AddPayload(new StringPayload("A")),
        new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)).AddPayload(new StringPayload("B")),
        new LocalInterval(new LocalDateTime(2008, 1, 6, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("A")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Equal()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft"))).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Outside()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("A")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 3, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)).AddPayload(new StringPayload("B")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        // TODO: Hmmm....
        new LocalInterval(new LocalDateTime(2008, 1, 3, 0, 0), new LocalDateTime(2008, 1, 4, 0, 0)).AddPayload(new StringPayload("B")),
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("A")),
        new LocalInterval(new LocalDateTime(2008, 1, 8, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)).AddPayload(new StringPayload("B")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Ending()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("A")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("B")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0)).AddPayload(new StringPayload("A")),
        new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("B")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_EndingEqual()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_OverlappingEnd()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("A")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)).AddPayload(new StringPayload("B")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0)).AddPayload(new StringPayload("A")),
        new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)).AddPayload(new StringPayload("B")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_TouchingEnd()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 8, 0, 0), new LocalDateTime(2008, 1, 10, 0, 0)).AddPayload(new StringPayload("Craft")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 10, 0, 0)).AddPayload(new StringPayload("Craft")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_After()
        {
            var timeline1 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")));
            var timeline2 = new TimelinePayload<StringPayload>(new LocalInterval(new LocalDateTime(2008, 1, 10, 0, 0), new LocalDateTime(2008, 1, 12, 0, 0)).AddPayload(new StringPayload("Craft")));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
        new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new StringPayload("Craft")),
        new LocalInterval(new LocalDateTime(2008, 1, 10, 0, 0), new LocalDateTime(2008, 1, 12, 0, 0)).AddPayload(new StringPayload("Craft")),
      }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }
    }
}

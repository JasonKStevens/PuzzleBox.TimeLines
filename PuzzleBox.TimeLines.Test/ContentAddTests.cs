using NodaTime;
using NUnit.Framework;
using PuzzleBox.Time.Payloads;
using PuzzleBox.Time.Timelines;
using PuzzleBox.TimeLines.Test;
using System.Linq;

namespace PuzzleBox.Time.Tests
{
    public class ContentAddTests
    {
        [Test]
        public void Add_Empty()
        {
            // Arrange
            var timeline1 = TimeBuilder.From(2008, 1, 1).To(2008, 1, 3).Payload("Craft", 1.5m).Timeline;
            var timeline2 = TimeBuilder.Empty;

            // Act
            var actual = (timeline1 + timeline2).GetIntervals();

            // Assert
            var expected = new []
            {
                TimeBuilder.From(2008, 1, 1).To(2008, 1, 3).Payload("Craft", 1.5m).Interval,
            };

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_ToEmpty()
        {
            // Arrange
            var timeline1 = TimeBuilder.Empty;
            var timeline2 = TimeBuilder.From(2008, 1, 1).To(2008, 1, 3).Payload("Craft", 1.5m).Timeline;

            // Act
            var actual = (timeline1 + timeline2).GetIntervals();

            // Assert
            var expected = new []
            {
                TimeBuilder.From(2008, 1, 1).To(2008, 1, 3).Payload("Craft", 1.5m).Interval,
            };

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Before()
        {
            // Arrange
            var timeline1 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));
            var timeline2 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 3, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));

            // Act
            var actual = (timeline1 + timeline2).GetIntervals();

            // Assert
            var expected = new LocalInterval[]
            {
                TimeBuilder.From(2008, 1, 1).To(2008, 1, 3).Payload("Craft", 1.5m).Interval,
                TimeBuilder.From(2008, 1, 4).To(2008, 1, 8).Payload("Craft", 1.5m).Interval,
            };

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_TouchingStart()
        {
            var timeline1 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));
            var timeline2 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 4, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
                new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
            }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_OverlappingStart()
        {
            var timeline1 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));
            var timeline2 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
                new LocalInterval(new LocalDateTime(2008, 1, 1, 0, 0), new LocalDateTime(2008, 1, 4, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
                new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0)).AddPayload(new KeyCountPayload("Craft", 3.0m)),
                new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
            }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Starting()
        {
            var timeline1 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));
            var timeline2 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
                new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)).AddPayload(new KeyCountPayload("Craft", 3.0m)),
                new LocalInterval(new LocalDateTime(2008, 1, 6, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
            }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Inside()
        {
            var timeline1 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));
            var timeline2 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
                new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
                new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 6, 0, 0)).AddPayload(new KeyCountPayload("Craft", 3.0m)),
                new LocalInterval(new LocalDateTime(2008, 1, 6, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
            }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Equal()
        {
            var timeline1 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));
            var timeline2 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 3.0m))).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Outside()
        {
            var timeline1 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));
            var timeline2 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 3, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
                new LocalInterval(new LocalDateTime(2008, 1, 3, 0, 0), new LocalDateTime(2008, 1, 4, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
                new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 3.0m)),
                new LocalInterval(new LocalDateTime(2008, 1, 8, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
            }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_Ending()
        {
            var timeline1 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));
            var timeline2 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
                new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
                new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 3.0m)),
            }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_OverlappingEnd()
        {
            var timeline1 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));
            var timeline2 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
                new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 5, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
                new LocalInterval(new LocalDateTime(2008, 1, 5, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 3.0m)),
                new LocalInterval(new LocalDateTime(2008, 1, 8, 0, 0), new LocalDateTime(2008, 1, 9, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
            }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_TouchingEnd()
        {
            var timeline1 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));
            var timeline2 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 8, 0, 0), new LocalDateTime(2008, 1, 10, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
                new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 10, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
            }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }

        [Test]
        public void Add_After()
        {
            var timeline1 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));
            var timeline2 = new TimelinePayload<KeyCountPayload>(new LocalInterval(new LocalDateTime(2008, 1, 10, 0, 0), new LocalDateTime(2008, 1, 12, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)));

            var actual = (timeline1 + timeline2).GetIntervals();
            var expected = new TimelineMask(TimeZones.Local, new[]
            {
                new LocalInterval(new LocalDateTime(2008, 1, 4, 0, 0), new LocalDateTime(2008, 1, 8, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
                new LocalInterval(new LocalDateTime(2008, 1, 10, 0, 0), new LocalDateTime(2008, 1, 12, 0, 0)).AddPayload(new KeyCountPayload("Craft", 1.5m)),
            }).GetIntervals();

            Assert.That(TimeMath.Equals(expected, actual), Is.True);
        }
    }
}

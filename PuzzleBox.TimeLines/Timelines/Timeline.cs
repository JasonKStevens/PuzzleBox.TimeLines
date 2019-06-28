using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace PuzzleBox.Time.Timelines
{
    public class Timeline : ITimeline
    {
        private readonly TimelineMask mask;
        private readonly TimelinePayload payload;

        public static Timeline operator ~(Timeline t)
        {
            t.mask.Negate();
            return t;
        }

        public static Timeline operator |(Timeline t1, TimelineMask t2)
        {
            t1.mask.Union(t2);
            return t1;
        }

        public static Timeline operator |(Timeline t1, Timeline t2)
        {
            t1.mask.Union(t2.mask);
            t1.payload.Add(t2.payload);
            return t1;
        }

        public static Timeline operator &(Timeline t1, TimelineMask t2)
        {
            t1.mask.Intersection(t2);
            return t1;
        }

        public static Timeline operator &(Timeline t1, Timeline t2)
        {
            t1.mask.Intersection(t2.mask);
            t1.payload.Add(t2.payload);
            return t1;
        }

        public static Timeline operator +(Timeline t1, Timeline t2)
        {
            t1.mask.Union(t2.mask);
            t1.payload.Add(t2.payload);
            return t1;
        }

        public static Timeline operator -(Timeline t1, Timeline t2)
        {
            t1.payload.Subtract(t2.payload);
            return t1;
        }

        public Timeline(TimelineMask mask) : this(mask, null) { }
        public Timeline(TimelinePayload payload) : this(null, payload) { }

        public Timeline(TimelineMask mask = null, TimelinePayload payload = null)
        {
            this.mask = mask ?? new TimelineMask();
            this.payload = payload ?? new TimelinePayload();
        }

        public virtual IEnumerable<LocalInterval> GetIntervals(LocalInterval interval, DateTimeZone destinationTimezone)
        {
            var maskIntervals = mask.GetIntervals(interval, destinationTimezone);
            var contentIntervals = payload.GetIntervals(interval, destinationTimezone);

            return TimeMathOld.Intersection(contentIntervals, maskIntervals).OrderBy(i => i.Start);
        }

        public IEnumerable<LocalInterval> GetIntervals(LocalInterval interval = null)
        {
            var maskIntervals = mask.GetIntervals(interval);
            var contentIntervals = payload.GetIntervals(interval);

            return TimeMathOld.Intersection(contentIntervals, maskIntervals).OrderBy(i => i.Start);
        }
    }
}

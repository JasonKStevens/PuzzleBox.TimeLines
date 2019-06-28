using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace PuzzleBox.Time.Timelines
{
    public class TimelineMask : ITimeline
    {
        private readonly NonOverlappingIntervals mask = new NonOverlappingIntervals();
        private bool isNot;

        private readonly IList<KeyValuePair<Func<List<LocalInterval>, List<LocalInterval>, IEnumerable<LocalInterval>>, TimelineMask>> layers = new List<KeyValuePair<Func<List<LocalInterval>, List<LocalInterval>, IEnumerable<LocalInterval>>, TimelineMask>>();

        public DateTimeZone Timezone { get; set; }

        public static TimelineMask operator ~(TimelineMask t)
        {
            t.Negate();
            return t;
        }

        public static TimelineMask operator |(TimelineMask t1, TimelineMask t2)
        {
            return t1.Union(t2);
        }

        public static TimelineMask operator &(TimelineMask t1, TimelineMask t2)
        {
            return t1.Intersection(t2);
        }

        public TimelineMask(DateTimeZone timezone = null)
        {
            Timezone = timezone;
        }

        public TimelineMask(DateTimeZone timezone, IEnumerable<LocalInterval> intervals) : this(timezone, new NonOverlappingIntervals(intervals))
        {
        }

        public TimelineMask(DateTimeZone timezone, LocalInterval interval) : this(timezone, new NonOverlappingIntervals(interval))
        {
        }

        public TimelineMask(DateTimeZone timezone, LocalDateTime? start = null, LocalDateTime? end = null) : this(timezone, new NonOverlappingIntervals(new LocalInterval(start, end)))
        {
        }

        public TimelineMask(DateTimeZone timezone, LocalDate? start = null, LocalDate? end = null) : this(timezone, new LocalInterval(start, end))
        {
        }

        public TimelineMask(DateTimeZone timezone, NonOverlappingIntervals mask) : this(timezone)
        {
            this.mask = mask;
        }

        private IEnumerable<LocalInterval> GetLocalIntervals(LocalInterval interval = null)
        {
            var ints = GetLocalIntervalsBase(interval);
            ints = isNot ? Not(ints) : ints;
            return ints;
        }

        private static IEnumerable<LocalInterval> Not(IEnumerable<LocalInterval> ints)
        {
            var intList = ints.ToList();
            if (intList.Count == 0) return new[] { new LocalInterval() };
            return TimeMathOld.Not(TimeMathOld.MergeTouching(intList));
        }

        protected virtual IEnumerable<LocalInterval> GetLocalIntervalsBase(LocalInterval interval = null)
        {
            if (interval == null) return mask.Select(i => i.Clone());
            if (!interval.Start.HasValue || !interval.End.HasValue) throw new ArgumentOutOfRangeException("interval", "Cannot request holidays for an unbounded range");

            return TimeMathOld.Intersection(mask, interval);
        }

        public IEnumerable<LocalInterval> GetIntervals(LocalInterval interval, DateTimeZone destinationTimezone)
        {
            var ints = GetIntervals(interval);
            return ints.Select(i => i.Map(Timezone, destinationTimezone));
        }

        public IEnumerable<LocalInterval> GetIntervals(LocalInterval interval = null)
        {
            if (layers.Count == 0) return GetLocalIntervals(interval);

            List<LocalInterval> currIntervals = GetLocalIntervals(interval).ToList();
            var currTimeline = new TimelineMask(Timezone, currIntervals);

            foreach (var layer in layers)
            {
                var nextIntervals = layer.Value.GetIntervals(interval, Timezone).ToList();

                var flat = layer.Key(currIntervals, nextIntervals).ToList();
                currTimeline = new TimelineMask(Timezone, flat);
                currIntervals = currTimeline.GetIntervals(interval, Timezone).ToList();
            }

            return currTimeline.GetIntervals(interval);
        }

        public void Negate()
        {
            isNot = !isNot;
        }

        public TimelineMask Intersection(TimelineMask timeline)
        {
            return Add(TimeMathOld.Intersection, timeline);
        }

        public TimelineMask Union(TimelineMask timeline)
        {
            return Add(TimeMathOld.Union, timeline);
        }

        public TimelineMask Complement(TimelineMask timeline)
        {
            return Add(TimeMathOld.Complement, timeline);
        }

        private TimelineMask Add(Func<List<LocalInterval>, List<LocalInterval>, IEnumerable<LocalInterval>> op, TimelineMask timeline)
        {
            Timezone = Timezone ?? timeline.Timezone;
            layers.Add(new KeyValuePair<Func<List<LocalInterval>, List<LocalInterval>, IEnumerable<LocalInterval>>, TimelineMask>(op, timeline));
            return this;
        }
    }
}

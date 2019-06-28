using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using PuzzleBox.Time.Payloads;

namespace PuzzleBox.Time.Timelines
{
    public class TimelinePayload : TimelinePayload<KeyCountPayload>
    {
        public TimelinePayload(LocalDate? start = null, LocalDate? end = null, TimePayloadBase payload = null, DateTimeZone timezone = null) : base(start, end, payload, timezone)
        {
        }
    }

    public class TimelinePayload<T> : ITimeline where T : TimePayloadBase
    {
        private readonly OverlappingIntervals<T> contentIntervals = new OverlappingIntervals<T>();
        private bool isNegative;
        private readonly IList<KeyValuePair<Func<List<LocalInterval>, List<LocalInterval>, IEnumerable<LocalInterval>>, TimelinePayload<T>>> layers = new List<KeyValuePair<Func<List<LocalInterval>, List<LocalInterval>, IEnumerable<LocalInterval>>, TimelinePayload<T>>>();

        public DateTimeZone Timezone { get; set; }

        public static TimelinePayload<T> operator +(TimelinePayload<T> t1, TimelinePayload<T> t2)
        {
            return t1.Add(t2);
        }

        public static TimelinePayload<T> operator -(TimelinePayload<T> t1, TimelinePayload<T> t2)
        {
            return t1.Subtract(t2);
        }

        public static TimelinePayload<T> operator -(TimelinePayload<T> t)
        {
            t.isNegative = true;
            return t;
        }

        public TimelinePayload()
        {
            Timezone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(TimeZoneInfo.Local.Id);
        }

        public TimelinePayload(IEnumerable<LocalInterval> intervals, DateTimeZone timezone = null) : this(new OverlappingIntervals<T>(intervals), timezone)
        {
        }

        public TimelinePayload(LocalInterval interval, DateTimeZone timezone = null) : this(new OverlappingIntervals<T>(interval), timezone)
        {
        }

        public TimelinePayload(LocalDate? start = null, LocalDate? end = null, TimePayloadBase payload = null, DateTimeZone timezone = null) : this(new LocalInterval(start, end).AddPayload(payload), timezone)
        {
        }

        public TimelinePayload(OverlappingIntervals<T> contentIntervals, DateTimeZone timezone = null)
        {
            Timezone = timezone;
            this.contentIntervals = contentIntervals;
        }

        public TimelinePayload(LocalDateTime? start = null, LocalDateTime? end = null, TimePayloadBase payload = null, DateTimeZone timezone = null) :
            this(new OverlappingIntervals<T>(new LocalInterval(start, end).AddPayload(payload)), timezone)
        {
        }

        public TimelinePayload<T> From(int v1, int v2, int v3)
        {
            return this;
        }

        private IEnumerable<LocalInterval> GetLocalIntervals(LocalInterval interval = null)
        {
            var ints = GetLocalIntervalsBase(interval);
            ints = isNegative ? TimeMathOld.MergeTouching(ints).SelectMany(TimeMathOld.Not) : ints;
            return ints;
        }

        protected virtual IEnumerable<LocalInterval> GetLocalIntervalsBase(LocalInterval interval = null)
        {
            if (interval == null) return contentIntervals.Select(i => i.Clone());
            if (!interval.Start.HasValue || !interval.End.HasValue) throw new ArgumentOutOfRangeException("interval", "Cannot request holidays for an unbounded range");

            return TimeMathOld.Intersection(contentIntervals, interval);
        }

        public IEnumerable<LocalInterval> GetIntervals(LocalInterval interval = null)
        {
            if (layers.Count == 0) return GetLocalIntervals(interval);

            List<LocalInterval> currIntervals = GetLocalIntervals(interval).ToList();
            var currTimeline = new TimelinePayload<T>(currIntervals, Timezone);

            foreach (var layer in layers)
            {
                var nextIntervals = layer.Value.GetIntervals(interval).ToList();

                var flat = layer.Key(currIntervals, nextIntervals).ToList();
                currTimeline = new TimelinePayload<T>(flat, Timezone);
                currIntervals = currTimeline.GetIntervals(interval).ToList();
            }

            return currTimeline.GetIntervals(interval);
        }

        public IEnumerable<LocalInterval> GetIntervals(LocalInterval interval, DateTimeZone destinationTimezone)
        {
            var ints = GetIntervals(interval);
            return ints.Select(i => i.Map(Timezone, destinationTimezone));
        }

        private TimelinePayload<T> Add(Func<List<LocalInterval>, List<LocalInterval>, IEnumerable<LocalInterval>> op, TimelinePayload<T> timeline)
        {
            Timezone = Timezone ?? timeline.Timezone;
            layers.Add(new KeyValuePair<Func<List<LocalInterval>, List<LocalInterval>, IEnumerable<LocalInterval>>, TimelinePayload<T>>(op, timeline));
            return this;
        }

        public TimelinePayload<T> Add(TimelinePayload<T> timeline)
        {
            return Add(Add, timeline);
        }

        public TimelinePayload<T> Subtract(TimelinePayload<T> timeline)
        {
            return Add(Subtract, timeline);
        }

        protected static IEnumerable<LocalInterval> Add(IEnumerable<LocalInterval> intervals1, IEnumerable<LocalInterval> intervals2)
        {
            var intervals = TimeMathOld.Fragment(intervals1, intervals2);

            var grps = intervals
              .GroupBy(i => i.Start)
              .ToList();

            var multiLayer = grps.Where(grp => grp.Count() > 1).SelectMany(grp => grp);
            var singleLayer = grps.Where(grp => grp.Count() == 1).SelectMany(grp => grp);

            return TimeMathOld.MergeTouching(singleLayer).Concat(multiLayer).OrderBy(i => i.Start);
        }

        protected static IEnumerable<LocalInterval> Subtract(IEnumerable<LocalInterval> intervals1, IEnumerable<LocalInterval> intervals2)
        {
            return Add(intervals1, intervals2.Select(i => { var i2 = i.Clone(); i2.Payload.Negate(); return i2; }));
        }
    }
}

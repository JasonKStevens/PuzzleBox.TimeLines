using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace PuzzleBox.Time.Timelines
{
    public class WorkPattern : TimelineMask
    {
        public LocalDate Start { get; set; }
        public LocalDate? End { get; set; }
        public IDictionary<int, DayPattern> Shifts { get; private set; }

        public WorkPattern(DateTimeZone timezone, LocalDate start, LocalDate? end = null) : base(timezone)
        {
            Timezone = timezone;
            Start = start;
            End = end;
            Shifts = new Dictionary<int, DayPattern>();
        }

        public void Add(DayPattern dayPattern)
        {
            Shifts[Shifts.Count] = dayPattern ?? new DayPattern();
        }

        public void AddRange(IEnumerable<DayPattern> days)
        {
            days.ToList().ForEach(Add);
        }

        private IEnumerable<LocalTimeInterval> GetDayPattern(LocalDate date)
        {
            if (date < Start) return new LocalTimeInterval[0];
            if (End.HasValue && date >= End) return new LocalTimeInterval[0];

            var index = Period.Between(Start, date, PeriodUnits.Days).Days;
            return Shifts[(int)index % Shifts.Count];
        }

        protected override IEnumerable<LocalInterval> GetLocalIntervalsBase(LocalInterval interval = null)
        {
            if (interval == null) interval = new LocalInterval(Start, End);
            if (!interval.Start.HasValue || !interval.End.HasValue) throw new ArgumentOutOfRangeException("interval", "Cannot request shift for an unbounded date range");

            LocalDate currDate = (interval.Start.Value.Date < Start) ? Start : interval.Start.Value.Date;

            while (currDate <= interval.End.Value.Date)
            {
                foreach (LocalTimeInterval timeInterval in GetDayPattern(currDate).ToList())
                {
                    var start = currDate + timeInterval.Start;
                    var end = currDate + timeInterval.End;

                    yield return new LocalInterval(start, end, null, timeInterval.Name);
                }

                currDate = currDate.PlusDays(1);
            }
        }
    }
}

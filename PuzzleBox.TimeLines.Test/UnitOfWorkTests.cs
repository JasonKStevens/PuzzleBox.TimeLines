using System;
using System.Collections.Generic;
using NodaTime;
using NUnit.Framework;
using PuzzleBox.Time.Timelines;

namespace PuzzleBox.Time.Tests
{
    public class UnitOfWorkTests
    {
        private static readonly DateTimeZone LocalTimeZone = GetTimeZone();
        private static readonly DateTimeZone NewYorkTimeZone = GetTimeZone("America/New_York");

        [Test]
        public void Example_MoreComplex()
        {
            var alicesEmployment = TimeBuilder.From(2004, 11, 1).TimeZone(LocalTimeZone).Payload("DEV", 1).Timeline;

            // var localHolidays = new Holidays(LocalTimeZone, EnricoApi.Country.NewZealand, EnricoApi.Region.Auckland);
            //var jasonsTimeline = alicesEmployment & JasonsWorkPattern() & ~localHolidays | JasonsUnscheduled();

            var alicesTimeline = alicesEmployment & AlicesWorkPattern() | AlicesUnscheduledHours();
            LogTimeline("Alice's Timeline (PST)", alicesTimeline, LocalTimeZone);

            var billsEmployment =
                TimeBuilder.From(1995, 11, 1).TimeZone(NewYorkTimeZone).Payload("DEV", 1).Timeline +
                TimeBuilder.From(2000, 11, 1).TimeZone(NewYorkTimeZone).Payload("EXEC", 1).Timeline;

            var billsTimeline = billsEmployment & BillsWorkPattern();
            LogTimeline("Bill's Timeline (EST)", billsTimeline, NewYorkTimeZone);

            var meetingTimes = alicesTimeline & billsTimeline;
            LogTimeline("Meeting Times (PST)", meetingTimes, LocalTimeZone);
            LogTimeline("Meeting Times (EST)", meetingTimes, NewYorkTimeZone);
        }

        #region Jason's timelines

        private static TimelineMask AlicesWorkPattern()
        {
            var start = new LocalDate(2014, 12, 22);

            var workDay = new DayPattern
            {
                new LocalTimeInterval(new LocalTime(09, 00), new LocalTime(12, 30)),
                new LocalTimeInterval(new LocalTime(13, 00), new LocalTime(17, 30)),
            };

            var pattern = new WorkPattern(LocalTimeZone, start);
            pattern.Add(null);
            pattern.Add(workDay);
            pattern.Add(workDay);
            pattern.Add(workDay);
            pattern.Add(workDay);
            pattern.Add(workDay);
            pattern.Add(null);

            return pattern;
        }

        private static TimelineMask AlicesUnscheduledHours()
        {
            return new TimelineMask(LocalTimeZone, new LocalDateTime(2014, 12, 26, 10, 0), new LocalDateTime(2014, 12, 26, 12, 30));
        }

        #endregion

        #region Bill's timelines

        private static TimelineMask BillsWorkPattern()
        {
            var start = new LocalDate(2014, 12, 22);

            var workDay = new DayPattern
            {
                new LocalTimeInterval(new LocalTime(09, 00), new LocalTime(12, 00)),
                new LocalTimeInterval(new LocalTime(13, 00), new LocalTime(17, 00)),
                new LocalTimeInterval(new LocalTime(20, 30), new LocalTime(22, 30)),
            };

            var pattern = new WorkPattern(NewYorkTimeZone, start);
            pattern.Add(workDay);
            pattern.Add(workDay);
            pattern.Add(workDay);
            pattern.Add(workDay);
            pattern.Add(workDay);
            pattern.Add(workDay);
            pattern.Add(workDay);

            return pattern;
        }

        #endregion

        #region Utility

        private static void LogTimeline(string heading, IEnumerable<LocalInterval> intervals)
        {
            Console.Out.WriteLine(heading);
            Console.Out.WriteLine("".PadRight(heading.Length, '-'));

            foreach (LocalInterval interval in intervals)
            {
                Console.Out.WriteLine(interval);
            }

            Console.Out.WriteLine();
        }

        private static void LogTimeline(string heading, ITimeline timeline)
        {
            LogTimeline(heading, timeline.GetIntervals());
        }

        private static void LogTimeline(string heading, ITimeline timeline, DateTimeZone timezone)
        {
            var queryInterval = new LocalInterval(new LocalDate(2014, 12, 22), new LocalDate(2014, 12, 28));
            LogTimeline(heading, timeline.GetIntervals(queryInterval, timezone));
        }

        private static DateTimeZone GetTimeZone(string id = null)
        {
            return DateTimeZoneProviders.Tzdb.GetZoneOrNull(id ?? "Pacific/Auckland");
        }

        #endregion
    }
}

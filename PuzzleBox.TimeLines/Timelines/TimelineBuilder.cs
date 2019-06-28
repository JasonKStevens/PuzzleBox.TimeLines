using System;
using NodaTime;
using PuzzleBox.Time.Payloads;

namespace PuzzleBox.Time.Timelines
{
    public class TimeBuilder
    {
        private class TimelineData
        {
            public LocalDate? Start { get; set; }
            public LocalDate? End { get; set; }
            public TimePayloadBase Payload { get; set; }
            public DateTimeZone TimeZone { get; internal set; }
        }

        private TimelineData timelineData;

        private TimeBuilder() {}

        public LocalInterval Interval
        {
            get
            {
                return new LocalInterval(timelineData.Start, timelineData.End, timelineData.Payload);
            }
        }

        public Timeline Timeline
        {
            get
            {
                var mask = new TimelineMask(
                    timelineData.TimeZone,
                    timelineData.Start,
                    timelineData.End);
                var payload = new TimelinePayload(
                    timelineData.Start,
                    timelineData.End,
                    timelineData.Payload,
                    timelineData.TimeZone);
                return new Timeline(mask, payload);
            }
        }

        public static object AddDate(int year, int month, int day)
        {
            return new TimeBuilder
            {
                timelineData = new TimelineData { Start = new LocalDate(year, month, day) }
            };
        }

        public static Timeline Empty { get { return new Timeline(); } }

        public static TimeBuilder From(int year, int month, int day)
        {
            return new TimeBuilder
            {
                timelineData = new TimelineData{ Start = new LocalDate(year, month, day) }
            };
        }

        public TimeBuilder To(int year, int month, int date)
        {
            timelineData.End = new LocalDate(year, month, date);
            return this;
        }

        public TimeBuilder Payload(string tag, decimal hours)
        {
            timelineData.Payload = new KeyCountPayload(tag, hours);
            return this;
        }

        public TimeBuilder TimeZone(DateTimeZone timeZone)
        {
            timelineData.TimeZone = timeZone;
            return this;
        }
    }
}

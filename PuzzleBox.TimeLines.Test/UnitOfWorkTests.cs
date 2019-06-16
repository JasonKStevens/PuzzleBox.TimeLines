using System;
using System.Collections.Generic;
using NodaTime;
using NUnit.Framework;
using PuzzleBox.Time.Timelines;
using PuzzleBox.TimeLines.Test;

namespace PuzzleBox.Time.Tests
{
  public class UnitOfWorkTests
  {
    private static readonly DateTimeZone JasonTimezone = GetTimeZone();
    private static readonly DateTimeZone JackTimezone = GetTimeZone("America/New_York");

    [Test]
    public void Example_MoreComplex()
    {
//      var jasonsTimeline = JasonsEmployement() & JasonsWorkPattern() & ~NzHolidays() | JasonsUnscheduled();
      var jasonsTimeline = JasonsEmployement() & JasonsWorkPattern() | JasonsUnscheduled();
      LogTimeline("Jason's Timeline (PST)", jasonsTimeline, JasonTimezone);

      var jacksTimeline = JacksEmployement() & JacksWorkPattern();
      LogTimeline("Jack's Timeline (EST)", jacksTimeline, JackTimezone);

      var meetingTimes = jasonsTimeline & jacksTimeline;
      LogTimeline("Meeting Times (PST)", meetingTimes, JasonTimezone);
      LogTimeline("Meeting Times (EST)", meetingTimes, JackTimezone);
    }

    #region Jason's timelines

    private static Timeline<CraftHoursPayload> JasonsEmployement()
    {
      var developer = new TimelinePayload<CraftHoursPayload>(new LocalDateTime(2004, 11, 1, 0, 0), null, new CraftHoursPayload("DEV", 1), JasonTimezone);
      var employment = new TimelineMask(JasonTimezone, new LocalDateTime(2008, 11, 1, 0, 0));
      return new Timeline<CraftHoursPayload>(employment, developer);
    }

    private static TimelineMask JasonsWorkPattern()
    {
      var start = new LocalDate(2014, 12, 22);

      var workDay = new DayPattern
      {
        new LocalTimeInterval(new LocalTime(09, 00), new LocalTime(12, 30)),
        new LocalTimeInterval(new LocalTime(13, 00), new LocalTime(17, 30)),
      };
      
      var pattern = new WorkPattern(JasonTimezone, start);
      pattern.Add(null);
      pattern.Add(workDay);
      pattern.Add(workDay);
      pattern.Add(workDay);
      pattern.Add(workDay);
      pattern.Add(workDay);
      pattern.Add(null);

      return pattern;
    }
//
//    private static TimelineMask NzHolidays()
//    {
//      return new Holidays(JasonTimezone, "NZL", "Auckland");
//    }

    private static TimelineMask JasonsUnscheduled()
    {
      return new TimelineMask(JasonTimezone, new LocalDateTime(2014, 12, 26, 10, 0), new LocalDateTime(2014, 12, 26, 12, 30));
    }

    #endregion

    #region Jack's timelines

    private static Timeline<CraftHoursPayload> JacksEmployement()
    {
      var employment = new TimelineMask(JackTimezone, new LocalDateTime(1995, 11, 1, 0, 0));
      var developer = new TimelinePayload<CraftHoursPayload>(new LocalDateTime(1995, 11, 1, 0, 0), null, new CraftHoursPayload("DEV", 1), JackTimezone);
      var exec = new TimelinePayload<CraftHoursPayload>(new LocalDateTime(2000, 11, 1, 0, 0), null, new CraftHoursPayload("EXEC", 1), JackTimezone);
      return new Timeline<CraftHoursPayload>(employment) + developer + exec;
    }

    private static TimelineMask JacksWorkPattern()
    {
      var start = new LocalDate(2014, 12, 22);

      var workDay = new DayPattern
      {
        new LocalTimeInterval(new LocalTime(09, 00), new LocalTime(12, 00)),
        new LocalTimeInterval(new LocalTime(13, 00), new LocalTime(17, 00)),
        new LocalTimeInterval(new LocalTime(20, 30), new LocalTime(22, 30)),
      };
      
      var pattern = new WorkPattern(JackTimezone, start);
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

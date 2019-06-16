using System;
using NodaTime;

namespace PuzzleBox.Time
{
  public static class DateTimeExt
  {
    public static LocalDate ToDateLocal(this DateTime date)
    {
      return new LocalDate(date.Year, date.Month, date.Day);
    }

    public static LocalDate? ToDateLocal(this DateTime? date)
    {
      if (!date.HasValue) return null;
      return new LocalDate(date.Value.Year, date.Value.Month, date.Value.Day);
    }

    public static LocalTime ToTimeLocal(this DateTime time)
    {
      return new LocalTime(time.Hour, time.Minute, time.Second, time.Millisecond);
    }

    public static LocalTime? ToTimeLocal(this DateTime? time)
    {
      if (!time.HasValue) return null;
      return new LocalTime(time.Value.Hour, time.Value.Minute, time.Value.Second, time.Value.Millisecond);
    }

    public static DateTime ToDateTime(this LocalDate date)
    {
      return new DateTime(date.Year, date.Month, date.Day);
    }

    public static DateTime ToDateTime(this LocalTime time, DateTime date)
    {
      return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, time.Millisecond);
    }

    public static DateTime? ToDateTime(this LocalTime? time, DateTime date)
    {
      if (!time.HasValue) return null;
      return new DateTime(date.Year, date.Month, date.Day, time.Value.Hour, time.Value.Minute, time.Value.Second, time.Value.Millisecond);
    }
  }
}
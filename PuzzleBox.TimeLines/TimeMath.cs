using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace PuzzleBox.Time
{
  public static class TimeMath
  {
    public static bool Equals(IEnumerable<LocalInterval> intervals1, IEnumerable<LocalInterval> intervals2)
    {
      var i1 = intervals1.OrderBy(i => i.Start).ToList();
      var i2 = intervals2.OrderBy(i => i.Start).ToList();

      if (i1.Count != i2.Count) return false;

      for (var i = 0; i < i1.Count; i++)
      {
        var l1 = i1[i];
        var l2 = i2[i];

        if (!l1.Equals(l2))
          return false;
      }

      return true;
    }

    public static LocalDate Min(LocalDate val1, LocalDate val2)
    {
      return val1 < val2 ? val1 : val2;
    }

    public static LocalDate Max(LocalDate val1, LocalDate val2)
    {
      return val1 > val2 ? val1 : val2;
    }

    public static LocalDate? Min(LocalDate? val1, LocalDate? val2)
    {
      if (val1 == null || val2 == null) return null;
      return val1 < val2 ? val1 : val2;
    }

    public static LocalDate? Max(LocalDate? val1, LocalDate? val2)
    {
      if (val1 == null || val2 == null) return null;
      return val1 > val2 ? val1 : val2;
    }

    public static LocalDateTime? Min(LocalDateTime? val1, LocalDateTime? val2)
    {
      if (val1 == null || val2 == null) return null;
      return val1 < val2 ? val1 : val2;
    }

    public static LocalDateTime? Max(LocalDateTime? val1, LocalDateTime? val2)
    {
      if (val1 == null || val2 == null) return null;
      return val1 > val2 ? val1 : val2;
    }

    public static DateTime? Min(DateTime? val1, DateTime? val2)
    {
      if (val1 == null || val2 == null) return null;
      return val1 < val2 ? val1 : val2;
    }

    public static DateTime? Max(DateTime? val1, DateTime? val2)
    {
      if (val1 == null || val2 == null) return null;
      return val1 > val2 ? val1 : val2;
    }

    public static LocalDateTime? MinStart(LocalInterval interval1, LocalInterval interval2)
    {
      if (!interval1.Start.HasValue || !interval2.Start.HasValue) return null;
      return Min(interval1.Start.Value, interval2.Start.Value);
    }

    public static LocalDateTime? MaxStart(LocalInterval interval1, LocalInterval interval2)
    {
      if (!interval1.Start.HasValue) return interval2.Start;
      if (!interval2.Start.HasValue) return interval1.Start;
      return Max(interval1.Start.Value, interval2.Start.Value);
    }

    public static LocalDateTime? MinEnd(LocalInterval interval1, LocalInterval interval2)
    {
      if (!interval1.End.HasValue) return interval2.End;
      if (!interval2.End.HasValue) return interval1.End;
      return Min(interval1.End.Value, interval2.End.Value);
    }

    public static LocalDateTime? MaxEnd(LocalInterval interval1, LocalInterval interval2)
    {
      if (!interval1.End.HasValue || !interval2.End.HasValue) return null;
      return Max(interval1.End.Value, interval2.End.Value);
    }

    public static bool Starts(LocalInterval interval1, LocalInterval interval2)
    {
      if (!interval1.Start.HasValue || !interval2.Start.HasValue) return true;
      return Min(interval1.Start.Value, interval2.Start.Value) == interval1.Start.Value;
    }

    public static bool Ends(LocalInterval interval1, LocalInterval interval2)
    {
      if (!interval1.End.HasValue || !interval2.End.HasValue) return true;
      return Max(interval1.End.Value, interval2.End.Value) == interval1.End.Value;
    }
  }
}

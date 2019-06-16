using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using PuzzleBox.Time.Timelines;

namespace PuzzleBox.Time
{
  public static class TimeMathOld
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

    public static List<LocalInterval> Not(LocalInterval interval)
    {
      var neg = new List<LocalInterval>();

      if (interval.Start.HasValue)
      {
        neg.Add(new LocalInterval(null, interval.Start, interval.Payload));
      }

      if (interval.End.HasValue)
      {
        neg.Add(new LocalInterval(interval.End, null, interval.Payload));
      }

      return neg;
    }

    public static IEnumerable<LocalInterval> Not(IEnumerable<LocalInterval> intervals)
    {
      var dates = intervals
        .SelectMany(ToDates)
        .ToList();

      dates.Insert(0, null);
      dates.Add(null);

      return dates
        .PairUp()
        .Select(t => new LocalInterval(t.Item1, t.Item2));
    }

    public static IEnumerable<Tuple<T, T>> PairUp<T>(this IEnumerable<T> source)
    {
      using (var iterator = source.GetEnumerator())
      {
        while (iterator.MoveNext())
        {
          var first = iterator.Current;
          var second = iterator.MoveNext() ? iterator.Current : default(T);
          yield return Tuple.Create(first, second);
        }
      }
    }

    private static IEnumerable<LocalDateTime?> ToDates(LocalInterval interval)
    {
      yield return interval.Start;
      yield return interval.End;
    }

    public static bool AreTouching(LocalInterval int1, LocalInterval int2)
    {
      if (!int2.Start.HasValue && !int1.Start.HasValue) return true;
      return int2.Start.HasValue && int1.Touches(int2.Start.Value) ||
             int1.Start.HasValue && int2.Touches(int1.Start.Value);
    }

    public static IEnumerable<LocalInterval> MergeTouching(IEnumerable<LocalInterval> intervals)
    {
      return intervals
        .OrderBy(i => i.Start)
        .Aggregate(new List<LocalInterval>(), (result, interval) =>
        {
          var last = result.LastOrDefault();
          var toAdd = interval.Clone();

          if (last != null && AreTouching(last, toAdd) && last.Payload.Equals(toAdd.Payload))
          {
            result = result.Take(result.Count - 1).ToList();
            toAdd = new LocalInterval(MinStart(last, toAdd), MaxEnd(last, toAdd), toAdd.Payload.Clone(), toAdd.Name);
          }

          result.Add(toAdd);
          return result;
        });
    }

    public static bool Overlap(LocalInterval interval1, LocalInterval interval2)
    {
      return (interval1.Start == null || interval2.End == null || interval1.Start < interval2.End) &&
             (interval2.Start == null || interval1.End == null || interval1.End >= interval2.Start);
    }

    public static IEnumerable<LocalInterval> Intersection(IEnumerable<LocalInterval> intervals, LocalInterval interval)
    {
      return Intersection(intervals, new List<LocalInterval> { interval });
    }

    public static IEnumerable<LocalInterval> Intersection(IEnumerable<LocalInterval> intervals1, IEnumerable<LocalInterval> intervals2)
    {
      var int2List = intervals2.ToList();

      return
        from interval1 in intervals1
        from interval2 in int2List
        let maxStart = MaxStart(interval1, interval2)
        let minEnd = MinEnd(interval1, interval2)
        where maxStart < minEnd
        select new LocalInterval(maxStart, minEnd, interval1.Payload + interval2.Payload, string.IsNullOrWhiteSpace(interval1.Name) ? interval2.Name : interval1.Name);
    }

    public static IEnumerable<LocalInterval> Union(IEnumerable<LocalInterval> intervals1, IEnumerable<LocalInterval> intervals2)
    {
      var both = intervals1.Concat(intervals2);
      return MergeTouching(both);
    }

    public static IEnumerable<LocalInterval> Complement(IEnumerable<LocalInterval> intervals1, IEnumerable<LocalInterval> intervals2)
    {
      return Intersection(intervals1, intervals2.SelectMany(Not));
    }

    public static IEnumerable<LocalInterval> Fragment(IEnumerable<LocalInterval> intervals1, IEnumerable<LocalInterval> intervals2)
    {
      var int1List = intervals1.ToList();
      var int2List = intervals2.ToList();

      var aggregate = int1List.Select(i => new Edge(i, i.Start, true))
        .Concat(int1List.Select(i => new Edge(i, i.End)))
        .Concat(int2List.Select(i => new Edge(i, i.Start, true)))
        .Concat(int2List.Select(i => new Edge(i, i.End)))
        .OrderBy(dateTime => dateTime.Value)
        .Aggregate(new IntervalAggregator(), (agg, edge) =>
        {
          if (agg.Fragments.Count > 0 && agg.Intervals.Count > 0)
          {
            var close = agg.Fragments.Where(f => !f.End.HasValue).ToList();

            for (int i = 0; i < close.Count; i++)
            {
              close[i] = new LocalInterval(close[i].Start, edge.Value, close[i].Payload);
            }

            agg.Fragments = agg.Fragments.Where(f => f.End.HasValue).Concat(close).ToList();
          }

          if (edge.IsStart)
            agg.Intervals.Add(edge.Interval);
          else
            agg.Intervals.Remove(edge.Interval);

          if (agg.Intervals.Count > 0)
          {
            agg.Intervals
              .GroupBy(i => i.Payload.GetType())
              .ToList()
              .ForEach(grp =>
              {
                var total = grp.Aggregate((TimePayload) new NullPayload(), (_total, interval) => _total + interval.Payload);
                agg.Fragments.Add(new LocalInterval(edge.Value, null, total));
              });
          }

          return agg;
        });

      return aggregate.Fragments.Where(f => f.Start != f.End);
    }

    private class IntervalAggregator
    {
      public readonly IList<LocalInterval> Intervals = new List<LocalInterval>();
      public IList<LocalInterval> Fragments = new List<LocalInterval>();
    }

    private class Edge
    {
      public readonly LocalInterval Interval;
      public readonly LocalDateTime? Value;
      public readonly bool IsStart;

      public Edge(LocalInterval interval, LocalDateTime? value, bool isStart = false)
      {
        Interval = interval;
        Value = value;
        IsStart = isStart;
      }
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

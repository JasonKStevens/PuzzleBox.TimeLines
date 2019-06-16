using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace PuzzleBox.Time.Timelines
{
  public class Timeline<T>: ITimeline where T: TimePayload
  {
    private readonly TimelineMask mask;
    private readonly TimelinePayload<T> payload;

    public static Timeline<T> operator !(Timeline<T> t)
    {
      t.mask.Negate();
      return t;
    }

    public static Timeline<T> operator |(Timeline<T> t1, TimelineMask t2)
    {
      t1.mask.Union(t2);
      return t1;
    }

    public static Timeline<T> operator |(Timeline<T> t1, Timeline<T> t2)
    {
      t1.mask.Union(t2.mask);
      t1.payload.Add(t2.payload);
      return t1;
    }

    public static Timeline<T> operator &(Timeline<T> t1, TimelineMask t2)
    {
      t1.mask.Intersection(t2);
      return t1;
    }

    public static Timeline<T> operator &(Timeline<T> t1, Timeline<T> t2)
    {
      t1.mask.Intersection(t2.mask);
      t1.payload.Add(t2.payload);
      return t1;
    }

    public static Timeline<T> operator +(Timeline<T> t1, TimelinePayload<T> t2)
    {
      t1.payload.Add(t2);
      return t1;
    }

    public static Timeline<T> operator -(Timeline<T> t1, TimelinePayload<T> t2)
    {
      t1.payload.Subtract(t2);
      return t1;
    }

    public Timeline(TimelineMask mask) : this(mask, null) {}
    public Timeline(TimelinePayload<T> payload) : this(null, payload) {}

    public Timeline(TimelineMask mask, TimelinePayload<T> payload)
    {
      this.mask = mask;
      this.payload = payload ?? new TimelinePayload<T>();
    }

    public IEnumerable<LocalInterval> GetIntervals(LocalInterval interval, DateTimeZone destinationTimezone)
    {
      var maskIntervals = mask.GetIntervals(interval, destinationTimezone);
      var contentIntervals = payload.GetIntervals(interval, destinationTimezone);
      
      return TimeMathOld.Intersection(contentIntervals, maskIntervals).OrderBy(i => i.Start);
    }

    public IEnumerable<LocalInterval> GetIntervals(LocalInterval interval)
    {
      var maskIntervals = mask.GetIntervals(interval);
      var contentIntervals = payload.GetIntervals(interval);

      return TimeMathOld.Intersection(contentIntervals, maskIntervals).OrderBy(i => i.Start);
    }
  }
}

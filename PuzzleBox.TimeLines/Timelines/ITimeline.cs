using System.Collections.Generic;
using NodaTime;

namespace PuzzleBox.Time.Timelines
{
  public interface ITimeline
  {
    IEnumerable<LocalInterval> GetIntervals(LocalInterval interval = null);
    IEnumerable<LocalInterval> GetIntervals(LocalInterval interval, DateTimeZone destinationTimezone);
  }
}
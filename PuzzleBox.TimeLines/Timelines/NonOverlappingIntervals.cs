using System.Collections.Generic;

namespace PuzzleBox.Time.Timelines
{
  public class NonOverlappingIntervals : List<LocalInterval>
  {
    public NonOverlappingIntervals() {}

    public NonOverlappingIntervals(LocalInterval interval)
    {
      Add(interval);
      EnsureNoOverlappingPeriods();
    }

    public NonOverlappingIntervals(IEnumerable<LocalInterval> intervals)
    {
      AddRange(intervals);
      EnsureNoOverlappingPeriods();
    }

    private void EnsureNoOverlappingPeriods()
    {
      // Forgiving for now
      TimeMathOld.MergeTouching(this);
    }
  }
}
using System.Collections.Generic;

namespace PuzzleBox.Time.Timelines
{
    public class OverlappingIntervals<T> : List<LocalInterval>
    {
        public OverlappingIntervals() { }

        public OverlappingIntervals(LocalInterval interval)
        {
            Add(interval);
        }

        public OverlappingIntervals(IEnumerable<LocalInterval> intervals)
        {
            AddRange(intervals);
        }
    }
}
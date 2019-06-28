using System.Collections.Generic;
using NodaTime;

namespace PuzzleBox.Time
{
    public class DayPattern : List<LocalTimeInterval>
    {
        public string Name { get; set; }

        public DayPattern() { }

        public DayPattern(string name, IEnumerable<LocalTimeInterval> intervals)
        {
            Name = name;
            AddRange(intervals);
        }

        public void Add(string name, LocalTime start, LocalTime stop)
        {
            Add(new LocalTimeInterval(name, start, stop));
        }
    }
}
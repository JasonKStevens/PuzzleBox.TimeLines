using System;
using NodaTime;

namespace PuzzleBox.Time
{
    public class LocalTimeInterval
    {
        public string Name { get; set; }
        public LocalTime Start { get; set; }
        public LocalTime End { get; set; }

        public LocalTimeInterval(string name, LocalTime start, LocalTime stop)
        {
            Name = name;
            Start = start;
            End = stop;
        }

        public LocalTimeInterval(LocalTime start, LocalTime stop) : this(null, start, stop) { }

        public LocalTimeInterval(DateTime start, DateTime stop) : this(null, new LocalTime(start.Hour, start.Minute, start.Second), new LocalTime(stop.Hour, stop.Minute, stop.Second)) { }

        public LocalTimeInterval(string name, DateTime start, DateTime stop) : this(name, new LocalTime(start.Hour, start.Minute, start.Second), new LocalTime(stop.Hour, stop.Minute, stop.Second)) { }

        public override string ToString()
        {
            var prefix = string.IsNullOrWhiteSpace(Name) ? "" : Name + ": ";
            return prefix + Start + " - " + End;
        }
    }
}
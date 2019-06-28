using NodaTime;
using PuzzleBox.Time.Payloads;

namespace PuzzleBox.Time
{
    public class LocalInterval
    {
        public string Name { get; private set; }
        public LocalDateTime? Start { get; private set; }
        public LocalDateTime? End { get; private set; }
        public TimePayloadBase Payload { get; private set; }

        protected LocalInterval() { }

        public LocalInterval(LocalDateTime? start = null, LocalDateTime? end = null, TimePayloadBase payload = null, string name = null)
        {
            Name = name;
            Start = start;
            End = end;
            Payload = payload;
        }

        public LocalInterval(LocalDate? start, LocalDate? end, TimePayloadBase payload = null, string name = null)
        {
            Name = name;
            Start = start.HasValue ? start.Value + new LocalTime(0, 0) : (LocalDateTime?)null;
            End = end.HasValue ? end.Value + new LocalTime(0, 0) : (LocalDateTime?)null;
            Payload = payload;
        }

        private LocalInterval(string name, LocalDateTime? start, LocalDateTime? end, TimePayloadBase payload)
        {
            Name = name;
            Start = start;
            End = end;
            Payload = payload;
        }

        public LocalInterval(string name, LocalDate date, TimePayloadBase payload = null) : this(date + new LocalTime(0, 0), date.PlusDays(1) + new LocalTime(0, 0), payload)
        {
            Name = name;
        }

        public LocalInterval Map(DateTimeZone from, DateTimeZone to)
        {
            LocalDateTime? start = Start.HasValue && from != null ? from.AtLeniently(Start.Value).ToInstant().InZone(to).LocalDateTime : (LocalDateTime?)null;
            LocalDateTime? end = End.HasValue && to != null ? from.AtLeniently(End.Value).ToInstant().InZone(to).LocalDateTime : (LocalDateTime?)null;

            return new LocalInterval(start, end, Payload, Name);
        }

        public decimal GetHours()
        {
            if (!Start.HasValue || !End.HasValue) return decimal.MaxValue;
            var period = Period.Between(Start.Value, End.Value);
            return period.Hours + period.Minutes / 60m;
        }

        public override string ToString()
        {
            var prefix = string.IsNullOrWhiteSpace(Name) ? "" : Name + ": ";
            return prefix + (Start.HasValue ? Start.ToString() : "null") + " - " + (End.HasValue ? End.ToString() : "null") + (Payload is NullPayload ? "" : " | " + Payload.ToString(Start, End));
        }

        public bool Includes(LocalDateTime dateTime)
        {
            return Start <= dateTime && dateTime < End;
        }

        public bool Touches(LocalDateTime dateTime)
        {
            return Start <= dateTime && dateTime <= End;
        }

        public LocalInterval Clone()
        {
            return new LocalInterval(Name, Start, End, Payload == null ? null : Payload.Clone());
        }

        public override bool Equals(object obj)
        {
            var other = obj as LocalInterval;
            return other != null && Equals(other);
        }

        protected bool Equals(LocalInterval other)
        {
            return string.Equals(Name, other.Name) &&
              Start.Equals(other.Start) &&
              End.Equals(other.End) &&
              (
                  Payload == null && other.Payload == null ||
                  Payload != null && Payload.Equals(other.Payload)
              );
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Start.GetHashCode();
                hashCode = (hashCode * 397) ^ End.GetHashCode();
                hashCode = (hashCode * 397) ^ (Payload != null ? Payload.GetHashCode() : 0);
                return hashCode;
            }
        }

        public LocalInterval AddPayload(TimePayloadBase payload = null)
        {
            Payload += payload;
            return this;
        }
    }
}

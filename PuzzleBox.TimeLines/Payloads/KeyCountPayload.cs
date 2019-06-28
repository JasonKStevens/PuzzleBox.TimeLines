using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace PuzzleBox.Time.Payloads
{
    public class KeyCountPayload : TimePayload<IDictionary<string, decimal>>
    {
        private readonly IDictionary<string, decimal> craftHours;

        public KeyCountPayload()
        {
            craftHours = new Dictionary<string, decimal>();
        }

        public KeyCountPayload(string craft, decimal hours)
        {
            craftHours = new Dictionary<string, decimal> { { craft, hours } };
        }

        public KeyCountPayload(IEnumerable<KeyValuePair<string, decimal>> hours)
        {
            craftHours = hours.ToDictionary(h => h.Key, h => h.Value);
        }

        public override IDictionary<string, decimal> GetValue()
        {
            return craftHours;
        }

        public override void Add(TimePayload<IDictionary<string, decimal>> payload)
        {
            foreach (KeyValuePair<string, decimal> keyValue in payload.GetValue())
            {
                if (craftHours.ContainsKey(keyValue.Key))
                {
                    craftHours[keyValue.Key] += keyValue.Value;
                }
                else
                {
                    craftHours.Add(new KeyValuePair<string, decimal>(keyValue.Key, keyValue.Value));
                }
            }
        }

        public override void Subtract(TimePayload<IDictionary<string, decimal>> payload)
        {
            foreach (KeyValuePair<string, decimal> keyValue in payload.GetValue())
            {
                if (craftHours.ContainsKey(keyValue.Key))
                {
                    craftHours[keyValue.Key] -= keyValue.Value;
                }
                else
                {
                    craftHours.Add(new KeyValuePair<string, decimal>(keyValue.Key, -keyValue.Value));
                }
            }
        }

        public override string ToString() { return string.Join(", ", craftHours.Select(c => c.Key + ": " + c.Value)); }
        public override string ToString(LocalDateTime? start, LocalDateTime? end)
        {
            if (!start.HasValue || !end.HasValue) return ToString();
            var period = Period.Between(start.Value, end.Value);
            var factor = period.Hours + period.Minutes / 60m;
            return string.Join(", ", craftHours.Select(c => c.Key + ": " + c.Value * factor));
        }

        public override TimePayloadBase Clone() { return new KeyCountPayload(craftHours); }
        public override bool Equals(TimePayloadBase obj) { var other = obj as KeyCountPayload; return other != null && other.craftHours.Count == craftHours.Count && !other.craftHours.Except(craftHours).Any(); }

        public override bool Equals(object obj) { return Equals(obj as TimePayloadBase); }
        public override int GetHashCode() { unchecked { return craftHours.Aggregate(17, (i, pair) => pair.Key.GetHashCode() * 13); } }

        public override TimePayloadBase Plus(TimePayloadBase timePayload) { var clone = (KeyCountPayload)Clone(); var other = timePayload as KeyCountPayload; if (other != null) clone.Add(other); return clone; }
        public override TimePayloadBase Negate() { return new KeyCountPayload(craftHours.ToDictionary(c => c.Key, c => -c.Value)); }
        public override TimePayloadBase Minus(TimePayloadBase timePayload) { var clone = (KeyCountPayload)Clone(); var other = timePayload as KeyCountPayload; if (other != null) clone.Subtract(other); return clone; }
    }
}

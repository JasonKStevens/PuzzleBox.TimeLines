using System.Collections.Generic;
using System.Linq;

namespace PuzzleBox.Time.Payloads
{
    public class StringPayload : TimePayload<IList<string>>
    {
        private readonly List<string> value = new List<string>();

        public StringPayload(string value)
        {
            this.value.Add(value);
        }

        public StringPayload(IEnumerable<string> value)
        {
            this.value.AddRange(value);
        }

        public override IList<string> GetValue() { return value; }
        public override void Add(TimePayload<IList<string>> payload) { }
        public override void Subtract(TimePayload<IList<string>> payload) { }
        public override TimePayloadBase Clone() { return new StringPayload(value); }
        public override bool Equals(TimePayloadBase obj) { return Equals((object)obj); }

        public override bool Equals(object obj)
        {
            if (!(obj is StringPayload sp)) return false;
            var firstNotSecond = sp.GetValue().Except(value).ToList();
            var secondNotFirst = value.Except(sp.GetValue()).ToList();
            return !firstNotSecond.Any() && !secondNotFirst.Any();
        }
        public override int GetHashCode() { unchecked { return 17 + ((value != null) ? value.GetHashCode() * 13 : 0); } }

        public override TimePayloadBase Plus(TimePayloadBase timePayload) { return timePayload.Clone(); }
        public override TimePayloadBase Negate() { return Clone(); }
        public override TimePayloadBase Minus(TimePayloadBase timePayload) { return timePayload.Clone().Negate(); }
    }
}

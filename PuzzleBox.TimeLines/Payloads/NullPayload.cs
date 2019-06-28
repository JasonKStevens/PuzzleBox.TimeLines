namespace PuzzleBox.Time.Payloads
{
    public class NullPayload : TimePayload<object>
    {
        public override object GetValue() { return null; }
        public override void Add(TimePayload<object> payload) { }
        public override void Subtract(TimePayload<object> payload) { }
        public override TimePayloadBase Clone() { return new NullPayload(); }
        public override bool Equals(TimePayloadBase obj) { return obj is NullPayload; }

        public override bool Equals(object obj) { return Equals(obj as TimePayloadBase); }
        public override int GetHashCode() { unchecked { return 0; } }

        public override TimePayloadBase Plus(TimePayloadBase timePayload) { return timePayload.Clone(); }
        public override TimePayloadBase Negate() { return Clone(); }
        public override TimePayloadBase Minus(TimePayloadBase timePayload) { return timePayload.Clone().Negate(); }
    }
}

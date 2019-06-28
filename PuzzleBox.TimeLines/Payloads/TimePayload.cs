namespace PuzzleBox.Time.Payloads
{
    public abstract class TimePayload<T> : TimePayloadBase
    {
        public abstract T GetValue();
        public abstract void Add(TimePayload<T> payload);
        public abstract void Subtract(TimePayload<T> payload);
    }
}

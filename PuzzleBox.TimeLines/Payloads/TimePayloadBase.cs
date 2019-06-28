using NodaTime;

namespace PuzzleBox.Time.Payloads
{
    public abstract class TimePayloadBase
    {
        public abstract TimePayloadBase Clone();
        public abstract bool Equals(TimePayloadBase obj);

        public static TimePayloadBase operator +(TimePayloadBase t1, TimePayloadBase t2)
        {
            if (t1 == null && t2 == null) return null;
            if (t1 == null) return t2;
            if (t2 == null) return t1;
            return t1.Plus(t2);
        }

        public static TimePayloadBase operator -(TimePayloadBase t1)
        {
            if (t1 == null) return null;
            t1.Negate();
            return t1;
        }

        public static TimePayloadBase operator -(TimePayloadBase t1, TimePayloadBase t2)
        {
            if (t1 == null && t2 == null) return null;
            if (t1 == null) return -t2;
            if (t2 == null) return t1;
            return t1.Minus(t2);
        }

        public abstract TimePayloadBase Plus(TimePayloadBase timePayload);
        public abstract TimePayloadBase Negate();
        public abstract TimePayloadBase Minus(TimePayloadBase timePayload);
        public virtual string ToString(LocalDateTime? start, LocalDateTime? end) { return ToString(); }
    }
}

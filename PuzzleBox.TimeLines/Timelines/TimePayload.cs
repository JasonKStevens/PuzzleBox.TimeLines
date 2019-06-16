using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace PuzzleBox.Time.Timelines
{
  public abstract class TimePayload
  {
    public abstract TimePayload Clone();
    public abstract bool Equals(TimePayload obj);

    public static TimePayload operator +(TimePayload t1, TimePayload t2)
    {
      if (t1 == null && t2 == null) return null;
      if (t1 == null) return t2;
      if (t2 == null) return t1;
      return t1.Plus(t2);
    }

    public static TimePayload operator -(TimePayload t1)
    {
      if (t1 == null) return null;
      t1.Negate();
      return t1;
    }

    public static TimePayload operator -(TimePayload t1, TimePayload t2)
    {
      if (t1 == null && t2 == null) return null;
      if (t1 == null) return -t2;
      if (t2 == null) return t1;
      return t1.Minus(t2);
    }

    public abstract TimePayload Plus(TimePayload timePayload);
    public abstract TimePayload Negate();
    public abstract TimePayload Minus(TimePayload timePayload);
    public virtual string ToString(LocalDateTime? start, LocalDateTime? end) { return ToString(); }
  }

  public abstract class TimePayload<T> : TimePayload
  {
    public abstract T GetValue();
    public abstract void Add(TimePayload<T> payload);
    public abstract void Subtract(TimePayload<T> payload);
  }

  public class NullPayload : TimePayload<object>
  {
    public override object GetValue() { return null; }
    public override void Add(TimePayload<object> payload) {}
    public override void Subtract(TimePayload<object> payload) {}
    public override TimePayload Clone() { return new NullPayload(); }
    public override bool Equals(TimePayload obj) { return obj is NullPayload; }

    public override bool Equals(object obj) { return Equals(obj as TimePayload); }
    public override int GetHashCode() { unchecked { return 0; } }

    public override TimePayload Plus(TimePayload timePayload) { return timePayload.Clone(); }
    public override TimePayload Negate() { return Clone(); }
    public override TimePayload Minus(TimePayload timePayload) { return timePayload.Clone().Negate(); }
  }

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
    public override void Add(TimePayload<IList<string>> payload) {}
    public override void Subtract(TimePayload<IList<string>> payload) {}
    public override TimePayload Clone() { return new StringPayload(value); }
    public override bool Equals(TimePayload obj) { return Equals((object) obj); }

    public override bool Equals(object obj)
    {
        if (!(obj is StringPayload sp)) return false;
        var firstNotSecond = sp.GetValue().Except(value).ToList();
        var secondNotFirst = value.Except(sp.GetValue()).ToList();
        return !firstNotSecond.Any() && !secondNotFirst.Any();
    }
    public override int GetHashCode() { unchecked { return 17 + ((value != null) ? value.GetHashCode() * 13 : 0); } }

    public override TimePayload Plus(TimePayload timePayload) { return timePayload.Clone(); }
    public override TimePayload Negate() { return Clone(); }
    public override TimePayload Minus(TimePayload timePayload) { return timePayload.Clone().Negate(); }
  }

  public class CraftHoursPayload : TimePayload<IDictionary<string, decimal>>
  {
    private readonly IDictionary<string, decimal> craftHours;

    public CraftHoursPayload(string craft, decimal hours)
    {
      craftHours = new Dictionary<string, decimal> {{craft, hours}};
    }

    public CraftHoursPayload(IEnumerable<KeyValuePair<string, decimal>> hours)
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

    public override TimePayload Clone() { return new CraftHoursPayload(craftHours); }
    public override bool Equals(TimePayload obj) { var other = obj as CraftHoursPayload; return other != null && other.craftHours.Count == craftHours.Count && !other.craftHours.Except(craftHours).Any(); }

    public override bool Equals(object obj) { return Equals(obj as TimePayload); }
    public override int GetHashCode() { unchecked { return craftHours.Aggregate(17, (i, pair) => pair.Key.GetHashCode() * 13); } }

    public override TimePayload Plus(TimePayload timePayload) { var clone = (CraftHoursPayload) Clone(); var other = timePayload as CraftHoursPayload; if (other != null) clone.Add(other); return clone; }
    public override TimePayload Negate() { return new CraftHoursPayload(craftHours.ToDictionary(c => c.Key, c => -c.Value)); }
    public override TimePayload Minus(TimePayload timePayload) { var clone = (CraftHoursPayload) Clone(); var other = timePayload as CraftHoursPayload; if (other != null) clone.Subtract(other); return clone; }
  }
}

using System;
using NodaTime;

namespace PuzzleBox.Time
{
    public static class TimeZoneHelper
    {
        public static DateTimeZone LocalTimeZone()
        {
            return DateTimeZoneProviders.Tzdb.GetZoneOrNull(TimeZoneInfo.Local.Id);
        }

        public static DateTimeZone GetTimeZone(string id)
        {
            return DateTimeZoneProviders.Tzdb.GetZoneOrNull(id);
        }
    }
}
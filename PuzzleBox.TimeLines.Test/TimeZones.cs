using NodaTime;
using System;

namespace PuzzleBox.TimeLines.Test
{
    public static class TimeZones
    {
        public static DateTimeZone Local => DateTimeZoneProviders.Tzdb.GetSystemDefault();
    }
}

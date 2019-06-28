//using EnricoApi;
//using NodaTime;
//using PuzzleBox.Time;
//using PuzzleBox.Time.Timelines;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace PuzzleBox.TimeLines.Timelines
//{
//    public class Holidays : Timeline
//    {
//        private readonly DateTimeZone timeZone;
//        private readonly Country country;
//        private readonly Region region;

//        public Holidays(DateTimeZone timeZone, Country country, Region region)  // TODO: Don't leak Country/Region
//        {
//            this.timeZone = timeZone;
//            this.country = country;
//            this.region = region;
//        }

//        public DateTimeZone Timezone { get; set; }

//        public override IEnumerable<LocalInterval> GetIntervals(LocalInterval interval = null)
//        {
//            if (interval.Start == null || interval.End == null)
//                throw new ArgumentNullException("Holiday interval must not be unbounded.");

//            var list = Enrico.GetPublicHolidaysForDateRangeAsync(
//                interval.Start.Value.ToDateTimeUnspecified(),
//                interval.End.Value.ToDateTimeUnspecified(),
//                country,
//                region)
//                .GetAwaiter().GetResult();  // TEMP:

//            // Console.WriteLine($"Date: {holiday.DateTime}, Local Name: {holiday.LocalName}, English Name: {holiday.EnglishName}");

//            var result = list
//                .Select(x => new LocalInterval(
//                    new LocalDate(x.DateTime.Year, x.DateTime.Month, x.DateTime.Day),
//                    new LocalDate(x.DateTime.Year, x.DateTime.Month, x.DateTime.Day).PlusDays(1)));
//            return result;
//        }
//    }
//}

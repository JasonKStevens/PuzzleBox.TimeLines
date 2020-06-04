# PuzzleBox.Timelines

[![Build Status](https://dev.azure.com/jasonkstevens/PuzzleBox/_apis/build/status/PuzzleBox.Timelines?branchName=master)](https://dev.azure.com/jasonkstevens/PuzzleBox/_build/latest?definitionId=5&branchName=master)

_This library is in beta._

**PuzzleBox.Timelines** is [.Net library](https://www.nuget.org/packages/PuzzleBox.Timelines/) for timeline arithmetic.

I was inspired to write this library after trying to use [Time Period Library for .Net](https://www.codeproject.com/Articles/168662/Time-Period-Library-for-NET). It's a very good library and a well written article but unfortunatelly, for my purposes, I couldn't make use of it.

I wanted a library I could use to easily work out employees' availability from work patterns, public holidays and leave.  Not only that, but to be able to aggregate this time by craft and qualification and be able to ask questions like, do we have 60 journeyman carpenter hours available next Tuesday?

This library can perform timezone-aware, timeline arithmetic with varying kinds of time. For example in planning and scheduling, how many person-hours are available for a particular craft, and if those hours were assigned how many are left for another craft.

## Timeline Operations

Consider two timelines A & B that overlap. The results of various and (&), or (|) and not (~) operations applied are shown below.

![Timeline operations](https://photos.google.com/share/AF1QipPCokRoqQviiSyae-8lkfgdYK2hLr_QLgwkHcNHCmmL1F5S8KYSLBBu8_dCoOP1hg/photo/AF1QipPaJd9gqWZKnz_X3QNQrE8m1oev0DF4svhZTUny?key=ME8wSXc1a1g1OVFFOTNLZHBJRVhGaEJGOFlSY0dR)

More complex operations can be performed.  For example, to work out the timeline when an employee was on the clock, take their employment period with respect to with their work pattern, remove holidays and leave, then finally add overtime.

```c#
var employmentPeriod = TimeBuilder
    .From(2004, 11, 1)
    .Timeline;

var workPattern = Pattern.Shift_24_48;
var publicHolidays = new Holidays("NZL", "Auckland");
var leave = LoadLeave();
var overtime = LoadOvertime();

var worked = employmentPeriod
    & workPattern
    & ~publicHolidays
    & ~leave
    | overtime;
```

This can be pictured as follows:

![Time worked](https://photos.google.com/share/AF1QipM26brdLIQLJXbvMlb5h5BACcaSQe9UNSKbUJvoonUZVf7etU8BWmwbMnOeTDpofA/photo/AF1QipNsw8i7PbkwVheHnDeEmS6jWL-ovPNt6XPuzesE?key=ZW1sNEl1TVRyeGtmMFFaX0w0aWRLalhOMWR3clFn)

Another example is to work out the possible meeting times of several people who may be in different timezones.
... To write up.

## Timeline Payloads

... To write up.

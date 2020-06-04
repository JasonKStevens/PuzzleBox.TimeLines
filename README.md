# PuzzleBox.Timelines

[![Build Status](https://dev.azure.com/jasonkstevens/PuzzleBox/_apis/build/status/PuzzleBox.Timelines?branchName=master)](https://dev.azure.com/jasonkstevens/PuzzleBox/_build/latest?definitionId=5&branchName=master)

_This library is in beta._

**PuzzleBox.Timelines** is [.Net library](https://www.nuget.org/packages/PuzzleBox.Timelines/) for timeline arithmetic.

I was inspired to write this library after trying to use [Time Period Library for .Net](https://www.codeproject.com/Articles/168662/Time-Period-Library-for-NET). It's a very good library and a well written article but unfortunatelly, for my purposes, I couldn't make use of it.

I wanted a library I could use to easily work out employees' availability from work patterns, public holidays and leave.  Not only that, but to be able to aggregate this time by craft and qualification and be able to ask questions like, do we have 60 journeyman carpenter hours available next Tuesday?

This library can perform timezone-aware, timeline arithmetic with varying kinds of time. For example in planning and scheduling, how many person-hours are available for a particular craft, and if those hours were assigned how many are left for another craft.

## Timeline Operations

Consider two timelines A & B that overlap. The results of various and (&), or (|) and not (~) operations applied are shown below.

![Timeline operations](https://lh3.googleusercontent.com/W4dFG5Y7fi-eQKxenzHI2SzSsygkCPhMS66Xvjcqi4nZM8B03EeYgo8xGFo8YQYl7Ko6SYOAU9hj-4V8iY1QZTXdD4LpiQem0UOC_23VpfdRZffLP2nKbkEyJGiD8wfEB4NoFm-_kKdbps92tTd5SjMkbxfUSbEk9iI6dIT4jwE2_HtB955bdjjkY4gT6mKsOJ04sBHiyoAtlkm1wJ2yX5Tz_Uqv3qXmh9DMJH7FyWyOI4ZeFmbZC9hetJmJaedtp2tKQGTBm0nTEL8Qlo-XVoXhUhxQb3bcv-M76UStWCUFdciyoxJnl7tcYhA3H6DU3slDQq9qUpXNJKB7ekowvX5kdLCcZmH4q406zSIf-i6m0by_yP-kV3avesrT7yTKxEegUmh_sxIpXqik-H-8ICwWNt-8P8hWXkw3YLI52eQkTCFxeRC5CrghtvQPEn1DFwZ4ogNpyepPh-qTt2th6tN-VD7FiRBO8VXpE4MHFqcstuGThbce6Ulf4YQEOYyUzI8thUWOCkQn2Tb27i5PEGQ2qBB8EDuxWD6F0G0sfqZ02ZO_yRFmDeuIQXhKBLeoTiEIfV_9bi4X1yE5uCaYLfcJTW8Aej6NprzfaTMPULyTboiWH2XrSyw7x1rW7itBUwagrojV3F_ju3oULJ7XyfKKmybxxtlH45SkqlK9EETWJzPVmQUkr8Nms-Ugvw=w1000-no-tmp.jpg)


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

![Time worked](https://lh3.googleusercontent.com/fuOrpW8pZ1U6TaG0Hlof5aU3HqwxGTY6oNBT2A2dax9jJKGhqIGTr6nzjf3mHQKT0VXa5xDG6hyVMBiY9ZhNoiYJ_Un55QZlctJJ3f9D5HImstHaLDVZzEO7M7HxGKo75RW42aRSlQBzm3HWfTuLOl-EAGzmQZux6hCLV-x0qyF-kwyrtupLax4Bn4XvI130jNJeXXpKDlJ2t-8SZDV3oMF9W7a92oNB3dsz3l55tafvknIvsb0lHdkvqX1xD4Xu0cCAHurdINa5BEwyWzvrPTiGlL86GG_mC6X6tjSRkZiHTIC9YsF7MlPO6bLIMy3Jeg89FqOreLPY17KduUg1erkfv5X6O6ni17DUYtoSiDT5xUBHuwKyEvDHB48cxmGmn0ArOUwARvMryLzCIYvG1572qEseTDhfYNJqtQNxLurmpG1wVoeUxSDFkTuWP3HnpUvPZMN9ZnTjY3h3K8HcgQWID4BQ2-lEIfjRzKv2t7zkSverUVM-ONQk1x9LNIBshdaJiIXVG4sUoOQEaLdtawVevS83XfFUIvbpz-cwoW1RhxwtsXlxsgExvYSoo3qD7A6rdPitmJOhoOrMic87GKY7Gff7-gbEqH05FB7HqzfBAMJYoQgSkW-QDlbwVn4AQV68sF_Ortpc7R2-KA1IuW3P6vQzQHPZVLjR7TcD7PnJJaFgApzRVnTxmfXFaQ=w1000-no-tmp.jpg)

Another example is to work out the possible meeting times of several people who may be in different timezones.
... To write up.

## Timeline Payloads

... To write up.

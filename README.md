# PuzzleBox.Timelines

[![Build Status](https://dev.azure.com/jasonkstevens/PuzzleBox/_apis/build/status/PuzzleBox.Timelines?branchName=master)](https://dev.azure.com/jasonkstevens/PuzzleBox/_build/latest?definitionId=5&branchName=master)

_This library is in beta._

**PuzzleBox.Timelines** is [.Net library](https://www.nuget.org/packages/PuzzleBox.Timelines/) for timeline arithmetic.

I was inspired to write this library after trying to use [Time Period Library for .Net](https://www.codeproject.com/Articles/168662/Time-Period-Library-for-NET). It's a very good library and a well written article but unfortunatelly, for my purposes, I couldn't make use of it.

I wanted a library I could use to easily work out employees' availability from work patterns, public holidays and leave.  Not only that, but to be able to aggregate this time by craft and qualification and be able to ask questions like, do we have 60 journeyman carpenter hours available next Tuesday?

This library can perform timezone-aware, timeline arithmetic with varying kinds of time. For example in planning and scheduling, how many person-hours are available for a particular craft, and if those hours were assigned how many are left for another craft.

## Timeline Operations

Consider two timelines A & B that overlap. The results of various and (&), or (|) and not (~) operations applied are shown below.

![Timeline operations](https://lh3.googleusercontent.com/KUtZlWXmliWidqRNAXxaIDvknOqmSpuGWLVn4PINfs-bXpXPwkZkcmORSQPAQ3RwH7SUjkoqP83FFfJ5M7Gjel58CjKbJJxB9YR9n3yhMNZz_EQiqv5BeOlc7TOEaz7ZvLHxznJiUyGhzvGGg_CW0ng69K7u3a7kwwfWOzHcSkuyJ_5fiZWt51GQPyRE0YoXisXsdpBH-2BqdwbthGGzOqZEaIuyNXHzpoJOiuQO3nWxktRWh_4C5SrmvLwfsUZqSXAlFOPtFmMPjdMalgUiydwNN7sx1Pr4SNKzIaZ7gME4vkLDCc429e-S07VUXKM0UKfrPHA563l1VXf_P6fqh0J0U4xFk9ak3RUa_H3cav0PRdk1yRcWgjrd4eVHusnEHWGBT6uNxo1gdF-ZqtL0ftY-wQQhOeiuulbsNwW5QF0FNSf9AI8ZgATEsEdlvxlmZlnnAEFEBmfyR2ZNeMpZ24o53Dbcbr854ZR5vYg9k8FORrSm1xrTeI6VWFYVdEE7lORFPVJ87Q1Xeek6RUR1f1ukz3ooANDu8fke2pdUPtGPiyVnGA3uQjFQEm6fRkfl1hqPP4lnl_vEPfAGWmLqU6ob7zyiB0fexjuZPwX3Eammb1fQJH36GP51q0D5mHZ3-ks1de3ugDy0peSsxgB0ZkKq4v0OcXAo=w406-h308-no)

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

![Time worked](https://lh3.googleusercontent.com/HlfgHpfMD1nwOOvSDD4ocIDfULmjnrqYTPPsre1SgQ1cYfkgofXGSfY6GHXQgt2nF07ML2EVNrX9P8Ob1Wx01K4A4rlC_ZRaSmZtjXf6ikrR4xPAKm7fxxOhk5DmB9esw9HU2GzJBQC4n7BAAE1aeMEO0qHKGdFZuJo2xgOaBQr3qgTNrYKEzJO_o2mkV1SRvrRWt6EWI7XXa6PyOjp9nG2f14gzWV8pkzDCQ5PU2biWEIzpeCMXDuewCUXmubUyndWwFhptrieEhUoreczIkWW1hhSWrwn6TiU2Sj8HXZwEhdjjq_AgPsY8NYeI3P4o7hVGjgEnDh_ZwjbcqQswhdKJf3Tjj603poHx-xlxNGEicerx-H1rEW_fnmELFCAxctXazoYBTsGPsRKV62j4UjlTJdfCLDFD8MNC6GRRPoGAdzz44blY4PMKKqV_O9nKwc-CUhSmafH-Iv117I2go3Vs93atq73YjZNalf991TOCJvcoiM_iA8xlU2b_mNTS7a00Igdwj8OwOyeywEJmA5SeKP9SquiEFOq5dR8dG7HQjzuMZSQ1-WmJCKOPjSE9vnYmFE4yOdKnVOhLPL5S9p0jo5sWnk3dD9zIKy4hxZU-367VqozawAq-udKu22dNYQc0YjkGVehBc8MV1_uFpjDSzvfzPUkN=w575-h205-no)

Another example is to work out the possible meeting times of several people who may be in different timezones.
... To write up.

## Timeline Payloads

... To write up.

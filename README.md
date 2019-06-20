# PuzzleBox.Timelines

[![Build Status](https://dev.azure.com/jasonkstevens/PuzzleBox/_apis/build/status/PuzzleBox.Timelines?branchName=master)](https://dev.azure.com/jasonkstevens/PuzzleBox/_build/latest?definitionId=5&branchName=master)

_This library is in alpha._

**PuzzleBox.Timelines** is [.Net library](https://www.nuget.org/packages/PuzzleBox.Timelines/) for timeline arithmetic.

I was inspired to write this library after trying to use [Time Period Library for .Net](https://www.codeproject.com/Articles/168662/Time-Period-Library-for-NET). It's a very good library and a well written article but unfortunatelly, for my purposes, I couldn't make use of it.

I wanted a library I could use to easily work out employees' availability from work patterns, public holidays and leave.  Not only that, but to be able to aggregate this time by craft and qualification and be able to ask questions like, do we have 60 journeyman carpenter hours available next Tuesday?

This library can perform timezone-aware, timeline arithmetic with varying kinds of time. For example in planning and scheduling, how many person-hours are available for a particular craft, and if those hours were assigned how many are left for another craft.

## Timeline Operations

Consider two timelines A & B that overlap. The results of various and (&), or (|) and not (~) operations applied are shown below.

![Timeline operations](https://lh3.googleusercontent.com/ipogJJce_SQEJbACaUwSQEwpJ_tMW7ehA-9q4le5ICaagI2Mc6pVJFjoKOCa9QUxjvPTg_EOHhPslkfbDjGN8BzLflDLFusaYcHzCR0aJ4Ku4DysnfMzusn5YBQA3TpFjDIy70nGTX41cd0qRxe_pYXO0TTLGQgzDfcIIVavEu6U006pdhLin3DzngiYYqTpiOTzjGMT5L7e4H-4jDylRe-1Z0ql0hdanxCTyOtmonVvguWWBHeiLD6gCXHRcliNf9MZSOWaFBBcsGABhiDe8lMpc8YcDxe6RQejaXxzNCFpablGmAAsVMjpL73kXHGFb-1YVq7XTvh3s1CbUTLLxQrrooy2-gZAkVtBeBUCsPs_mNyfN9Oaldli0lfldyqHkS2FJVRxo0nGfdtfo8j89mNiV2qV6ScoIea1hzX_pHgRvu5n6EZI-ihZEQkUrmkpHtQDMj_kpDkR0FSCS0rrB4OHJa5EWOyBizG2rEnlV4olQ_ATGBttThm6jE74suTOVtPUMO33hk-oAHwdwbGs6ybKqkdE4z5oViS4wBJacenvbFTaA2oVMUfd1wjpG8NAXZit7nQJlEsWCDpVh-Qh9mUxugA8mb0jQZMw6ZJZoeqPv1jPaWQmjLYFNw5x0PN1UX9_Mzf5Vf4jPXWRY1vutf916UJxEkj2=w551-h417-no)

More complex operations can be performed.  For example, to work out the timeline when an employee was on the clock, take their employment period with respect to with their work pattern, remove holidays and leave, then finally add overtime.

```c#
var worked = employmentPeriod
    & workPattern
    & ~publicHolidays
    & ~leave
    | overtime;
```

This can be pictured as follows:

![Time worked](https://lh3.googleusercontent.com/rpQiLFgTrC9AkVONilJRoD7tomJUXldduXm8geGxl0Kh7ZRr589BeDggC3ERSZ89USMD1gMvj82zvXbxvvdykE4SuiGcWzprSYBW24EM0R0XigC4G2vps3L5Ny-cu_15ni_d5JDub8ueTaSMiHsLuHFMHc0SAm7TBYISRcqR0jI0OGkVlFLZCEIlJEdZVYGMUI8NgmC0obkBcacxFJfPwLDZuaMPyjVb7OkGUrTnwxbbQSNx8nJuVmq65Jm_wvBgVCU37kJm2EgSHxfJb6bzF61qfx87MlcG0HQCmjOASaoGgXY5iAzRgNp3uIulj8u0gMLuWvRILvD6AKkWeCtvPApcjVPobkGlhxdzSi1_95mh3zdTXLSJhJZi2taWgwMESKLhkKnFTMij7Xxk6ccDtax-7SEVdwd9f1BmK0PCz7n2XUEFecDGpfPtXHTcMSIBPQDkskF1iK4XV9smK7uWzDTFNeEj1g-Qz_iKKQpKJa5UQkx9k1cmq_W8OhVWc9OOCyatCW7gBoIAdtRB47ml0dwTppaJutYMTgtqs20kb99O0mUDxT8hyA8T4kdIysxAL7LBMf7t7VjSkZDkwU7KQG456s0uGctdTwpI0DdiI_feXgj3hhwpPsSqlz5joj85hTKxGqwc-Aiq_cyAVrCL7zGO4qdchVlx=w784-h286-no)

Another example is to work out the possible meeting times of several people who may be in different timezones.
... To write up.

## Timeline Payloads

... To write up.

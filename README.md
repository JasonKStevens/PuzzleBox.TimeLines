# PuzzleBox.Timelines

[![Build Status](https://dev.azure.com/jasonkstevens/PuzzleBox.Timeline/_apis/build/status/JasonKStevens.PuzzleBox.Timelines?branchName=master)](https://dev.azure.com/jasonkstevens/PuzzleBox.Timeline/_build/latest?definitionId=2&branchName=master)

_This library is in alpha._

**PuzzleBox.Timelines** is .Net library for timeline arithmetic.

I was inspired to write this library after trying to use [Time Period Library for .Net](https://www.codeproject.com/Articles/168662/Time-Period-Library-for-NET). It's a very good library and a well written article but unfortunatelly, for my purposes, I couldn't make use of it.

I wanted a library that I could use to perform arbitrary, timezone aware timeline arithmetic. Where you could, for example, find out how many craft-hours (453 electrician hours, 342 plumber hours, etc) you have available over a certain period; and do this by adding up all the timelines of individuals which are in turn added up by the timelines of their work pattern, leave, etc.

## Timeline Operations

Consider two timelines A & B that overlap. The results of various and (&), or (|) and not (~) operations applied are shown below.

![Timeline operations](https://lh3.googleusercontent.com/kgqxP5j7H_sinkhoejqLRWsjgR92SaRAnsrBtbffsIDiig2PqzWUj8u_CMud_n7I2vGsgZUM4hBFV-KWQhOIXBDfCaE0UgBnqooB9h6tAa1rkdD75GZmTTI_QcZKlyKawZ9MGI3JswGve286aJXg2Sv9WkqIE5BwNt6eoYzJeilAgrl5kSYMm67dkcSKxxj5MKLW-RgsbzAQInvqAWL5fJx_JThHoSfqM_eg36wjmWwVjyLeI5d0HGFtSyGCjzPFOT8ALpBpBSu6FDIu5h9w0C2nnOFd9DohhkiYaSYnVQhjbRHyfEpcGgazqE28Lwzqvw_btndrNi9VoB20A6OFpgwQA8lg7b1DwQCe3quEX-T0naX2qVZQnoqTzZB3leop6JTJc_fpcSyCxP3kNkN0bza-1NlLDnWxVoe3qd6mhIx9mkMFDsYE0AYa68zuoXh70jtUMIWSKG59m0kDdws_-PBYhdIrvxs8TORXTAlT38GQWJ3CAx3kPHDCMnNyg-jjvXs1Vzp0-689hUh_NM0xQEkIZPxADOveb3h7jQA591Dy7K4LSD1RiqgdlznC6NeJoMe_ZjgMmmZHCFOT0jAsze41PYcdlAMVcB4RVif3_rB0-BifsfsH1V43mZk0vaUKlLxf7TB2gzlpHHluWUQka3LgvjxWRprH=w551-h417-no)

More complex operations can be performed.  For example, to work out the timeline when an employee was on the clock, take their employment period with respect to with their work pattern, remove holidays and leave, then finally add overtime.

```c#
var worked = employmentPeriod
    & workPattern
    & ~publicHolidays
    & ~leave
    | overtime;
```

This can be pictured as follows:

![Total hours worked](https://lh3.googleusercontent.com/j7yqKrunHY5jt_NdSoiRGBOBsbK1VgYVz5HNIh5nehUvCU6SGMILAYSxjfUvROpALuGu37L7OzYmWHtyxOdQhglf9hzDV3QAamZRbjYdzXUYhDAnYbV_eTlt3oDrjl83w7OM4kzdYI8Ldce3gdBkEbvwgLpaVPdJjtVjLn7Wcp2l2QPyFwGRjd5OWfJ6TNs69APetTB6QcD7EyqG-qJSZS6CkzJ4_Js1hPq6UYVddg2_lyI3snP2uTNYOl6FRFO8vabMm4_aqYw0ZS44o6u2S_Nz0mUAjHsAPFnSbaaP4B5s2FZzfSMvpB1Ph18OOFO3kTwA5lD4BXxmr0eGC9WxAAEIFqMnXo3k9dOgrG3iS_17Z4NZfDADtrw2T636uBFE0MaJvbXgNfI_pbY5HpmHUoSdBUT9FWdZR3XOjl4TgpuoRMKZt8mGV7SMThX1wNvUiFfYH0EO5hyRNMogQ06MnOk3hyhZUxunVVPhKcfAtD0xAQd4VCG3AktLecm5RA5v37Fcm1R1VHbmht4a8j97yCdGJKVVd-XAvbiGy77KgWevzRhPtV4WSVeAp33S_1S1aQeUd96Vt3orC5gUAQX_SXkfaTPGlmd0szMOEA3Ma9RTec1l_a33apIFglviy-5xIJ0TRpsLn-AMMp3IiGBDsCbTqiIm8G06=w790-h267-no)

Another example is to work out the possible meeting times of several people who may be in different timezones.
... To write up.

## Timeline Payloads

... To write up.

<!-- Template in [Documenting architecture decisions - Michael Nygard](http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions). -->

# ADR 001 Start with WPF
<!-- Short Title -->

## Status
<!-- What is the status, such as proposed, accepted, rejected, deprecated, superseded, etc.? -->
Accepted, 03-21-2021

## Context
<!-- What is the issue that we're seeing that is motivating this decision or change? -->
According to [Choose your Windows app platform](https://docs.microsoft.com/de-de/windows/apps/desktop/choose-your-platform#use-the-windows-ui-library-with-windows-apps)
we can choose from: UWP, WPF and Windows Forms.

## Decision
<!-- What is the change that we're proposing and/or doing? -->
I start with WPF. Reasons:
* Windows Forms seems outdated for me.
* A few years ago I have tried a few things with UWP, and I didn't like it.

## Consequences
<!-- What becomes easier or more difficult to do because of this change? -->
At the moment [WinUi](https://docs.microsoft.com/de-de/windows/apps/winui/winui3/) is in Preview-State.
Perhaps I can switch to WinUi later.

Another alternative may be using Xamarin. If I understand it right, this will be named MAUI in the near future.
For 2D graphics one could use SkiaSharp, which seems to be deeply integrated in Xamarin. It seems also possible
to use SkiaSharp together with WPF. But as I understand it is only possible to fill bitmaps with SkiaSharp 
(see this [example](https://docs.microsoft.com/en-us/answers/questions/87925/using-skiasharp-for-making-graphs-in-wpf.html))
and I can not use SkiaSharp-Controls as it is possible in Xamarin (see [here](https://docs.microsoft.com/de-de/xamarin/xamarin-forms/user-interface/graphics/skiasharp/basics/animation))


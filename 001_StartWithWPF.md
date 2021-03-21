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
I hope I can use WinUi later.

# App tour / product tour

## Overview

FinSkew should provide an in-app guided product tour that helps users quickly understand the most important parts of the interface. The tour is intended to reduce initial confusion, improve discoverability of navigation and results, and give users confidence when using the calculators for the first time.

The first version of this feature is limited to documenting the requirements for a shared, guided walkthrough. It does not yet define the implementation details of any specific library or framework.

## Goals

- Help users discover the most important areas of the app without reading external documentation.
- Explain how to navigate between calculators and how to interpret a calculator page.
- Highlight the core page regions that appear throughout the app, such as:
  - navigation,
  - input fields,
  - summary results,
  - charts,
  - and growth visualizations when present.
- Make the tour easy to restart at any time from a clearly visible help entry point.

## Entry point

- A `?` **help icon** should appear in the top app bar.
- Clicking the help icon opens a small menu anchored to that icon.
- The menu currently contains one item only:
  - **Take a tour**
- Selecting **Take a tour** starts the guided tour immediately from the first step.

## First-version scope

The first version of the product tour should be a **global shell + generic calculator-page tour**.

This means:

- The tour should explain shared application elements that are common across the app.
- The tour should also explain the common structure of a typical calculator page.
- The tour should not yet provide separate, custom tours for each individual calculator.

## Tour coverage

The initial tour should cover the following areas when they are present on the current page:

1. **App bar**
   - Introduce the main shell.
   - Explain the presence of the navigation toggle and help entry point.
2. **Navigation drawer**
   - Explain that calculators are grouped into sections and can be reached from the drawer.
3. **Page heading and contextual navigation**
   - Highlight the page title and breadcrumb area, when available.
4. **Input section**
   - Explain that calculator inputs are edited directly in the form.
   - Indicate that inputs validate immediately and that results update without a separate submit button.
5. **Summary results**
   - Explain that the main computed values are shown in a compact summary panel.
6. **Chart area**
   - Explain that charts visualize the relationship between invested value, gains, withdrawals, or other calculator-specific results.
7. **Growth section**
   - When the current calculator includes growth charts or tables, explain that they show how values evolve over time.

If a region is not available on the current page, the tour should skip that step instead of showing a broken or irrelevant step.

## Tour behavior

- The first version of the tour should be **manual only**.
  - It should not automatically start for first-time users.
- The first version should be **informational only**.
  - It may point to interactive UI elements.
  - It should not require the user to perform actions in order to continue.
- Each run of the tour starts at the first step.
- The tour should expose clear navigation controls:
  - **Next**
  - **Back**
  - **Skip/End tour**
  - **Done** on the final step
- The user should be able to dismiss the tour at any point.
- The user should be able to start the tour again at any time from the help menu.

## Tour presentation

The tour may use one or more of the following guidance surfaces:

- anchored tooltip/callout,
- spotlight overlay,
- modal/dialog for introduction or completion,
- lightweight overlay treatment to dim non-active areas.

Presentation requirements:

- Only one active step is shown at a time.
- The active target should remain visually obvious.
- The explanation text should be short, plain-language, and task-oriented.
- The highlighted step should feel connected to the target element rather than appearing detached from it.
- The experience should remain consistent with the app's existing visual language and accessibility expectations.

## Persistence and restart behavior

- Completion of the tour may be remembered on the current device/browser so the app can treat the user as having already completed the tour.
- Dismissing or skipping the tour should **not** count as completion.
- Regardless of whether completion is remembered, the **Take a tour** menu item must always remain available so the user can restart the tour on demand.

## Responsive behavior

- The tour must adapt to desktop, tablet, and mobile layouts.
- On mobile/tablet, the tour should account for vertically stacked calculator sections.
- Navigation-drawer steps should account for the drawer being collapsed by default on smaller screens.
- If an anchored tooltip or popover does not fit well on a smaller viewport, the explanatory content may switch to a more suitable surface such as a dialog or sheet while preserving the tour context.

## Accessibility requirements

- The help icon, help menu, and all tour controls must be keyboard accessible.
- Guided-tour content must support screen readers with appropriate labels and readable structure.
- Visible focus states must remain clear throughout the tour.
- Tour messaging must not rely on color alone.
- Overlay, tooltip, and dialog text must maintain WCAG AA contrast.
- When the tour is dismissed or completed, focus should return to the help icon or another logical trigger element.

## Out of scope for the first version

- Calculator-specific custom tours for each individual page.
- Mandatory or auto-start onboarding flows.
- Progress checkpoints that resume from the middle of a previously abandoned tour.
- Account-based or cloud-synced tour history.
- A help menu with multiple entries beyond **Take a tour**.

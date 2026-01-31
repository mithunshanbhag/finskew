# Frontend Architecture

> **Document Purpose**: Detailed technical documentation of the FinSkew Blazor WebAssembly frontend architecture.

## Table of Contents

- [Overview](#overview)
- [Blazor WebAssembly Application Structure](#blazor-webassembly-application-structure)
- [Component Hierarchy](#component-hierarchy)
- [State Management](#state-management)
- [Routing Strategy](#routing-strategy)
- [MudBlazor Integration](#mudblazor-integration)
- [Rendering and Lifecycle](#rendering-and-lifecycle)
- [Performance Considerations](#performance-considerations)

## Overview

FinSkew's frontend is built using **Blazor WebAssembly**, which compiles C# code to WebAssembly for execution in the browser. This architecture enables:

- Writing UI logic in C# instead of JavaScript
- Sharing code between client and server (future backend)
- Strong typing and compile-time checking
- Rich .NET ecosystem and tooling

### Application Startup

The application entry point is `Program.cs`:

```csharp
var host = WebAssemblyHostBuilder
    .CreateDefault(args)
    .ConfigureApp()
    .ConfigureServices()
    .Build();

await host.RunAsync();
```

This bootstrap process:
1. Creates a WebAssembly host builder
2. Configures the application settings
3. Registers services in the DI container
4. Builds and runs the application

### Target Framework

- **.NET 10.0**: Latest version with improved performance and WASM optimizations
- **C# 13**: Modern language features including nullable reference types
- **ImplicitUsings**: Reduced boilerplate with globally imported namespaces

## Blazor WebAssembly Application Structure

### Application Root

The application uses a standard Blazor WASM structure:

- **`App.razor`**: Root component that contains the router
- **`_Imports.razor`**: Global using statements for all Razor components
- **`wwwroot/`**: Static assets (CSS, images, fonts, favicon)
- **`Program.cs`**: Application entry point and service configuration

### Component Organization

Components are organized by function:

```
Components/
├── Layout/           # Application layout components
│   └── MainLayout.razor
├── Pages/            # Routable page components (calculators)
│   ├── SimpleInterestCalculator.razor
│   ├── CompoundInterestCalculator.razor
│   ├── SIPCalculator.razor
│   ├── SWPCalculator.razor
│   ├── STPCalculator.razor
│   ├── LumpsumCalculator.razor
│   └── NotFound.razor
└── Shared/           # Reusable shared components
    └── PageHeader.razor
```

**Design Principle**: Each calculator page is self-contained, managing its own state and rendering logic.

### Global Imports

`_Imports.razor` provides common namespaces to all components:

- Blazor framework namespaces
- MudBlazor components
- Application models and services
- System namespaces (via implicit usings)

## Component Hierarchy

### Layout Structure

```
App (Router)
    │
    └─── MainLayout
            │
            ├─── App Bar (MudAppBar)
            │       ├─── Hamburger Menu Icon
            │       ├─── FinSkew Logo/Text
            │       └─── GitHub Icon
            │
            ├─── Navigation Drawer (MudDrawer)
            │       └─── Calculator Menu Items
            │
            └─── Main Content Area
                    │
                    └─── @Body (current page)
                            │
                            ├─── PageHeader (breadcrumbs)
                            │
                            └─── Calculator Page
                                    ├─── Input Section
                                    │       └─── MudForm
                                    │               ├─── MudNumericField
                                    │               ├─── MudNumericField
                                    │               └─── ...
                                    │
                                    └─── Results Section
                                            ├─── MudChart (visualization)
                                            └─── Summary Panel
                                                    ├─── Label-Value Pairs
                                                    └─── Formatted Results
```

### Component Responsibilities

#### MainLayout.razor
- Defines the overall page structure
- Manages navigation drawer state (collapsed/expanded)
- Provides consistent app bar and navigation across all pages
- Handles responsive behavior (drawer auto-collapse on mobile)

#### PageHeader.razor
- Displays breadcrumb navigation
- Shows action buttons (optional, hidden on most calculators)
- Provides context for current page location

#### Calculator Pages (e.g., SimpleInterestCalculator.razor)
- Manages calculator-specific state (input and result view models)
- Handles form validation
- Triggers calculations on input changes
- Renders input fields and results
- Implements debouncing for real-time updates

## State Management

### State Management Approach

FinSkew uses **component-local state** rather than global state management. Each calculator page maintains its own state using:

1. **Private fields** in the component code-behind
2. **View models** for structured data (input/result separation)
3. **Two-way data binding** with `@bind-Value`

### Why Component-Local State?

**Advantages**:
- ✅ Simple and straightforward
- ✅ No boilerplate or ceremony
- ✅ Easy to reason about
- ✅ Each calculator is independent
- ✅ No risk of state leaks between calculators

**Trade-offs**:
- ❌ State doesn't persist across navigation
- ❌ No shared state between components
- ❌ Re-calculation required on return to page

**Justification**: Since calculators don't share data and users typically use one calculator at a time, component-local state is sufficient and simpler than global state management (e.g., Fluxor, Redux pattern).

### State Flow Pattern

```
User Input
    │
    ├─── @bind-Value updates InputViewModel property
    │
    ▼
InputViewModel Property Change
    │
    ├─── Debounce timer resets
    │
    ▼
Debounce Timer Expires (e.g., 300ms)
    │
    ├─── Validation runs (data annotations)
    │
    ▼
Service.Calculate(inputViewModel)
    │
    ├─── Business logic executes
    │
    ▼
ResultViewModel Updated
    │
    ├─── StateHasChanged() called
    │
    ▼
UI Re-renders
    │
    └─── Results section updates (chart + summary)
```

### View Model Pattern

Each calculator uses two view models:

**InputViewModel**:
- Contains user-editable fields (principal, rate, time)
- Includes data validation attributes (`[Range]`, `[Required]`)
- May inherit from base view models for common fields

**ResultViewModel**:
- Contains calculated outputs (interest, total amount)
- Includes chart data (for MudChart components)
- Read-only properties (no user editing)

This separation provides:
- Clear contract between UI and business logic
- Type-safe data binding
- Centralized validation rules
- Testable calculation inputs/outputs

### Example: Simple Interest Calculator State

```csharp
// Component state (private fields)
private SimpleInterestInputViewModel _inputViewModel = new();
private SimpleInterestResultViewModel _resultViewModel = new();
private bool _isFormValid;

// User types in input field
<MudNumericField @bind-Value="_inputViewModel.PrincipalAmount" />

// Debounced calculation triggered
_resultViewModel = CalculatorService.Calculate(_inputViewModel);

// UI updates automatically via data binding
```

## Routing Strategy

### Blazor Router

The application uses Blazor's built-in router configured in `App.razor`:

```razor
<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
    </Found>
    <NotFound>
        <PageTitle>404 Not Found</PageTitle>
        <LayoutView Layout="@typeof(MainLayout)">
            <NotFound />
        </LayoutView>
    </NotFound>
</Router>
```

### Multi-Route Aliases

Each calculator registers **multiple URL paths** using multiple `@page` directives:

```csharp
@page "/"
@page "/simpleinterestcalculator"
@page "/simpleinterestcalc"
@page "/simple-interest-calculator"
@page "/simple-interest-calc"
@page "/sic"
```

**Benefits**:
- **SEO**: More discoverable via different search terms
- **Usability**: Users can type short forms or full names
- **Flexibility**: Marketing materials can use branded URLs
- **Backwards compatibility**: Old URLs continue working

### Default Route

The Simple Interest Calculator is the **default home page** (`@page "/"`). There is no separate landing page; users start directly with a functional calculator.

**Rationale**: Reduces clicks and gets users to value immediately.

### Route Precedence

When multiple routes match, Blazor selects the most specific route:
1. Exact literal matches
2. Routes with fewer segments
3. Routes with more constraints

Since all calculator routes are exact literal matches, precedence isn't typically an issue.

### Navigation Mechanisms

**Drawer Navigation**:
- Click items in the left navigation drawer
- Drawer closes automatically on mobile after selection

**Breadcrumbs**:
- Not clickable by default (informational only)
- Could be enhanced for navigation in the future

**Browser Back/Forward**:
- Works naturally with Blazor routing
- Each page has its own URL for bookmarking/sharing

### Route Constants

Route paths are defined in `Constants/RouteConstants.cs` for centralized management and refactoring safety.

## MudBlazor Integration

### Why MudBlazor?

MudBlazor was chosen for:
- **Material Design**: Modern, professional aesthetic
- **Comprehensive Components**: Forms, charts, navigation, layouts
- **Accessibility**: Built-in ARIA support and keyboard navigation
- **Responsive**: Mobile-first design patterns
- **Active Development**: Regular updates and community support
- **Blazor-Native**: No JavaScript interop required

### MudBlazor Configuration

MudBlazor is registered in `Program.cs`:

```csharp
builder.Services.AddMudServices();
```

And included globally via `_Imports.razor`:

```razor
@using MudBlazor
```

### Key MudBlazor Components Used

#### Layout Components
- **MudMainContent**: Defines the main content area
- **MudAppBar**: Top navigation bar
- **MudDrawer**: Collapsible side navigation
- **MudPaper**: Card-like containers with elevation

#### Form Components
- **MudForm**: Form container with validation
- **MudNumericField<T>**: Numeric input with min/max/step
- **MudTextField<T>**: Text input fields
- **Adornments**: Icons (₹, %, clock) for visual clarity

#### Data Visualization
- **MudChart**: Charts for result visualization (donut, bar, line)

#### Utility Components
- **MudStack**: Flexbox-based layout container
- **MudBreadcrumbs**: Navigation breadcrumbs
- **MudTooltip**: Contextual help on hover

### MudBlazor Theming

Currently using **default MudBlazor theme** with color customizations:

- **Primary Color**: Used for app bar, buttons, primary actions
- **Tertiary Color**: Used for input adornments (₹, %, icons)
- **Variant.Outlined**: Consistent outlined style for input fields

**Future Enhancement**: Custom theme using FinSkew brand colors (documented in specs).

### Responsive Design with MudBlazor

MudBlazor provides responsive utilities:

**MudStack with `Row="true"`**:
```razor
<MudStack Row="true">
    <!-- Desktop: side-by-side -->
    <!-- Mobile: stacks vertically (automatic) -->
</MudStack>
```

**Drawer Behavior**:
- Desktop: Expanded by default, toggle with hamburger
- Mobile/Tablet: Collapsed by default, overlay when opened

**Chart Sizing**:
- Charts use percentage-based widths
- Automatically resize to fit parent container

## Rendering and Lifecycle

### Component Lifecycle

Key lifecycle methods used in calculator pages:

1. **OnInitialized()**: Initialize view models, set default values
2. **OnParametersSet()**: React to parameter changes (not heavily used)
3. **OnAfterRender()**: Perform JavaScript interop if needed (minimal usage)

### Rendering Triggers

Components re-render when:
- State changes (via `StateHasChanged()`)
- Parameters change
- Parent component re-renders
- User interactions (events)

### Debounced Rendering

To prevent excessive re-renders during rapid input:

**Timer-Based Debouncing**:
```csharp
private System.Timers.Timer _debounceTimer;

private void OnInputChanged()
{
    _debounceTimer?.Stop();
    _debounceTimer = new System.Timers.Timer(300); // 300ms delay
    _debounceTimer.Elapsed += (sender, args) => {
        InvokeAsync(() => {
            CalculateResults();
            StateHasChanged();
        });
    };
    _debounceTimer.Start();
}
```

This ensures calculations run only after the user pauses typing, improving performance and user experience.

### Rendering Optimization

**Minimal Re-Rendering**:
- Only the calculator page re-renders on input changes
- Layout components (MainLayout) remain static
- MudBlazor components have internal optimization

**Future Enhancements**:
- Consider `@key` directives for list rendering
- Use `ShouldRender()` override for conditional rendering
- Implement virtualization for large data sets (if applicable)

## Performance Considerations

### Initial Load Performance

**Challenge**: Blazor WASM requires downloading the .NET runtime and application DLL files (~2-3 MB) before the app starts.

**Mitigation**:
- Compression (Brotli/Gzip) enabled by Azure Static Web Apps
- Loading indicator displayed during initial load
- Caching headers for repeat visits

**Future Enhancements**:
- Lazy loading for calculator pages
- Tree shaking to reduce bundle size
- AOT (Ahead-of-Time) compilation for faster startup

### Runtime Performance

**Current State**:
- Calculations are nearly instantaneous (simple arithmetic)
- Debouncing reduces unnecessary computation
- No network calls during usage (fully client-side)

**Bottlenecks**:
- Excessive re-renders during rapid input (mitigated by debouncing)
- Large chart rendering (not an issue with current data sizes)

### Memory Management

- .NET garbage collector handles memory automatically
- View models are lightweight (primitive types, small arrays)
- No memory leaks observed from component lifecycle
- Disposal of timers and event handlers managed properly

### Browser Compatibility

**Supported Browsers**:
- Chrome/Edge (Chromium-based)
- Firefox
- Safari 14+

**Requirements**:
- WebAssembly support (all modern browsers)
- JavaScript enabled (for Blazor bootstrapping)

**Testing**:
- Primary testing on Chromium (via Playwright E2E tests)
- Manual testing on major browsers recommended

### Network Performance

**Static Asset Delivery**:
- Azure CDN reduces latency globally
- Static files cached by browser
- No API calls for calculator functionality

**Future Considerations**:
- Service workers for offline support
- Progressive Web App (PWA) capabilities
- Background sync for future backend features

## Summary

FinSkew's frontend architecture leverages Blazor WebAssembly for a rich, interactive client-side experience:

- **Component-based**: Modular, reusable Razor components
- **Local state management**: Simple, sufficient for calculator use cases
- **Multi-route aliases**: SEO-friendly and user-friendly routing
- **MudBlazor integration**: Modern UI with minimal custom styling
- **Performance optimized**: Debouncing, lazy updates, efficient rendering

This architecture provides a solid foundation for the current feature set and can evolve to support future backend integration when needed.

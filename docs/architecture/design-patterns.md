# Design Patterns

> **Document Purpose**: Catalog of design patterns, architectural patterns, and coding conventions used throughout FinSkew.

## Table of Contents

- [Overview](#overview)
- [Architectural Patterns](#architectural-patterns)
- [Component Patterns](#component-patterns)
- [Data Patterns](#data-patterns)
- [UI Patterns](#ui-patterns)
- [Testing Patterns](#testing-patterns)

## Overview

FinSkew employs consistent design patterns across the codebase to ensure maintainability, testability, and developer productivity. These patterns emerged from practical needs and are documented here to guide future development.

## Architectural Patterns

### 1. Frontend-Only Architecture

**Pattern**: Single-page application with no backend dependencies for core functionality.

**Implementation**:

- All calculations run client-side
- No API calls during calculator usage
- State exists only in browser memory
- No data persistence (current version)

**Benefits**:

- Zero server costs
- Instant user feedback
- Simplified deployment
- Works offline (with service worker in future)

**Drawbacks**:

- No data persistence
- Limited to client-side capabilities
- Initial load time for WebAssembly

**When to Use**: Educational tools, calculators, data visualization tools where data persistence isn't required.

---

### 2. Layered Architecture

**Pattern**: Clear separation of concerns into logical layers.

**Layers**:

```
Presentation Layer (Razor Components)
       ↓
Business Logic Layer (Services)
       ↓
Data Layer (ViewModels)
       ↓
Infrastructure Layer (MudBlazor, .NET)
```

**Benefits**:

- Clear boundaries between concerns
- Easy to test each layer independently
- Easier to refactor and maintain

**Implementation Example**:

```csharp
// Presentation: SimpleInterestCalculator.razor
<MudNumericField @bind-Value="_inputViewModel.PrincipalAmount" />

// Business Logic: InterestCalculatorService.cs
public SimpleInterestResultViewModel Calculate(SimpleInterestInputViewModel input)
{
    // Calculation logic
}

// Data: SimpleInterestInputViewModel.cs
public class SimpleInterestInputViewModel
{
    [Range(10000, 100000000)]
    public int PrincipalAmount { get; set; }
}
```

---

### 3. Convention Over Configuration

**Pattern**: Use sensible defaults and naming conventions to reduce boilerplate configuration.

**Examples**:

- Calculator pages always have two view models: `*InputViewModel` and `*ResultViewModel`
- Service interfaces follow `I*Service` naming
- Constants classes use `*Constants` suffix
- Route constants match component names

**Benefits**:

- Predictable code structure
- Faster development
- Easier onboarding for new developers

---

## Component Patterns

### 1. Input/Result ViewModel Pattern

**Pattern**: Separate view models for user inputs and calculated results.

**Structure**:

```
Calculator Component
    ├── *InputViewModel (user-editable data)
    │       ├── Properties with validation attributes
    │       └── Two-way data binding with UI
    │
    └── *ResultViewModel (calculated data)
            ├── Read-only properties
            ├── Chart data
            └── Formatted display values
```

**Example**:

```csharp
// Input: User provides these values
public class SimpleInterestInputViewModel
{
    [Range(10000, 100000000)]
    public int PrincipalAmount { get; set; } = 100000;

    [Range(0.01, 50)]
    public double RateOfInterest { get; set; } = 5;

    [Range(1, 30)]
    public int TimePeriodInYears { get; set; } = 5;
}

// Result: System calculates these values
public class SimpleInterestResultViewModel
{
    public required int PrincipalAmount { get; init; }
    public required int InterestEarned { get; init; }
    public required int TotalAmount { get; init; }
    public required double[] ChartData { get; init; }
}
```

**Benefits**:

- Clear separation of inputs and outputs
- Type-safe contracts
- Validation is centralized in input model
- Results are immutable (using `required` and `init`)

**When to Use**: Any feature with user input and calculated output.

---

### 2. Real-Time Calculation with Debouncing

**Pattern**: Update results automatically as user types, but debounce rapid changes.

**Implementation**:

```csharp
private System.Timers.Timer? _debounceTimer;
private const int DebounceDelayMs = 300;

private void OnInputChanged()
{
    // Reset the timer on each input change
    _debounceTimer?.Stop();
    _debounceTimer?.Dispose();

    _debounceTimer = new System.Timers.Timer(DebounceDelayMs);
    _debounceTimer.AutoReset = false;
    _debounceTimer.Elapsed += async (sender, args) =>
    {
        await InvokeAsync(() =>
        {
            RecalculateResults();
            StateHasChanged();
        });
    };
    _debounceTimer.Start();
}
```

**Benefits**:

- Smooth user experience (no "Calculate" button)
- Reduces unnecessary computations
- Prevents UI flicker from rapid updates

**Alternatives Considered**:

- ❌ Calculate on every keystroke: Too many re-renders
- ❌ Require "Calculate" button: Extra click, less modern UX
- ✅ Debouncing: Best balance of responsiveness and performance

---

### 3. Component Lifecycle Management

**Pattern**: Proper initialization and cleanup in component lifecycle methods.

**Lifecycle Hooks Used**:

```csharp
protected override void OnInitialized()
{
    // Initialize view models with default values
    _inputViewModel = new SimpleInterestInputViewModel();
    _resultViewModel = CalculateInitialResult();
}

protected override void OnAfterRender(bool firstRender)
{
    if (firstRender)
    {
        // Perform one-time setup (e.g., focus first input)
    }
}

public void Dispose()
{
    // Clean up timers, event handlers
    _debounceTimer?.Stop();
    _debounceTimer?.Dispose();
}
```

**Best Practices**:

- Initialize state in `OnInitialized()`
- Clean up resources in `Dispose()`
- Use `firstRender` flag to avoid duplicate operations
- Always dispose of timers and event subscriptions

---

## Data Patterns

### 1. Immutable Result Models

**Pattern**: Result view models use `required init` properties to ensure immutability.

**Example**:

```csharp
public class SimpleInterestResultViewModel
{
    public required int PrincipalAmount { get; init; }
    public required int InterestEarned { get; init; }
    public required int TotalAmount { get; init; }
}

// Usage: Must be initialized with all properties
var result = new SimpleInterestResultViewModel
{
    PrincipalAmount = 100000,
    InterestEarned = 25000,
    TotalAmount = 125000
};

// Cannot be modified after creation
// result.PrincipalAmount = 200000; // Compile error!
```

**Benefits**:

- Results cannot be accidentally modified
- Clear intent: results are outputs, not mutable state
- Safer multi-threaded scenarios (future)
- Better reasoning about data flow

---

### 2. Validation Attributes

**Pattern**: Use data annotations for declarative validation in input view models.

**Example**:

```csharp
public class SimpleInterestInputViewModel
{
    [Range(10000, 100000000, ErrorMessage = "Amount must be between ₹10,000 and ₹10,00,00,000")]
    public int PrincipalAmount { get; set; } = 100000;

    [Range(0.01, 50, ErrorMessage = "Rate must be between 0.01% and 50%")]
    public double RateOfInterest { get; set; } = 5;

    [Range(1, 30, ErrorMessage = "Time period must be between 1 and 30 years")]
    public int TimePeriodInYears { get; set; } = 5;
}
```

**Integration with MudBlazor**:

```razor
<MudForm @bind-IsValid="_isFormValid" Model="_inputViewModel">
    <MudNumericField @bind-Value="_inputViewModel.PrincipalAmount"
                     Min="10000"
                     Max="100000000" />
</MudForm>
```

**Benefits**:

- Validation logic lives with the data model
- Automatic validation by MudForm
- Consistent error messages
- Easy to test validation rules

---

### 3. View Model Inheritance

**Pattern**: Inherit common properties from base view models to reduce duplication.

**Example**:

```csharp
// Base input model with common interest calculator fields
public class SimpleInterestInputViewModel
{
    public int PrincipalAmount { get; set; }
    public double RateOfInterest { get; set; }
    public int TimePeriodInYears { get; set; }
}

// Compound interest extends simple interest with additional field
public class CompoundInterestInputViewModel : SimpleInterestInputViewModel
{
    public int CompoundingFrequency { get; set; } = 1; // Annual by default
}
```

**Benefits**:

- Reduces code duplication
- Ensures consistent field names and types
- Easy to add new calculator types
- Shared validation rules

**When Not to Use**: If inheritance creates tight coupling or obscures meaning.

---

## UI Patterns

### 1. Responsive Layout with MudStack

**Pattern**: Use `MudStack` with `Row="true"` for responsive side-by-side layouts.

**Implementation**:

```razor
<MudStack Row="true">
    <!-- Input Section -->
    <MudPaper class="pa-2 ma-2">
        <MudForm>
            <!-- Input fields -->
        </MudForm>
    </MudPaper>

    <!-- Results Section -->
    <MudPaper class="pa-2 ma-2">
        <MudChart />
        <!-- Summary -->
    </MudPaper>
</MudStack>
```

**Behavior**:

- **Desktop**: Items displayed side-by-side (row layout)
- **Mobile/Tablet**: Automatically stacks vertically
- **Responsive**: No media queries needed

---

### 2. Input Adornments for Context

**Pattern**: Use leading icons in input fields to indicate the unit or type of data.

**Implementation**:

```razor
<!-- Currency input -->
<MudNumericField Adornment="Adornment.Start"
                 AdornmentIcon="@Icons.Material.Filled.CurrencyRupee"
                 AdornmentColor="Color.Tertiary"
                 Label="Principal Amount" />

<!-- Percentage input -->
<MudNumericField Adornment="Adornment.Start"
                 AdornmentIcon="@Icons.Material.Filled.Percent"
                 AdornmentColor="Color.Tertiary"
                 Label="Rate of Interest" />

<!-- Time period input -->
<MudNumericField Adornment="Adornment.Start"
                 AdornmentIcon="@Icons.Material.Filled.Schedule"
                 AdornmentColor="Color.Tertiary"
                 Label="Time Period (Years)" />
```

**Benefits**:

- Immediate visual context
- Reduces cognitive load
- Accessibility (screen readers announce icons)
- Consistent visual language

---

### 3. Multiple Route Aliases

**Pattern**: Register multiple URLs for the same component to improve discoverability and usability.

**Implementation**:

```razor
@page "/"
@page "/simpleinterestcalculator"
@page "/simpleinterestcalc"
@page "/simple-interest-calculator"
@page "/simple-interest-calc"
@page "/sic"

<PageTitle>Simple Interest Calculator</PageTitle>
```

**Route Patterns**:

- Full name with hyphens: `/simple-interest-calculator`
- Full name without hyphens: `/simpleinterestcalculator`
- Short form: `/sic`
- Abbreviations: `/simple-interest-calc`

**Benefits**:

- Better SEO (multiple keywords)
- User convenience (memorable short forms)
- Backwards compatibility if URLs change

---

### 4. Consistent Typography Hierarchy

**Pattern**: Use MudBlazor's typography components consistently across all pages.

**Type Scale**:

```razor
<!-- Page title -->
<MudText Typo="Typo.h5">Simple Interest Calculator</MudText>

<!-- Section header -->
<MudText Typo="Typo.h6">Results</MudText>

<!-- Hero number (chart center) -->
<MudText Typo="Typo.h4" Class="font-weight-bold">₹ 1,25,000</MudText>

<!-- Summary label -->
<MudText Typo="Typo.body2" Class="font-weight-bold">Principal Amount:</MudText>

<!-- Summary value -->
<MudText Typo="Typo.body1">₹ 1,00,000</MudText>

<!-- Chart legend / footnotes -->
<MudText Typo="Typo.caption">*Interest calculated annually</MudText>
```

**Benefits**:

- Visual consistency across pages
- Clear information hierarchy
- Accessibility (proper heading levels)

---

### 5. Accessibility-First Design

**Pattern**: Include ARIA attributes and semantic HTML in all interactive components.

**Implementation**:

```razor
<!-- Section with role and label -->
<MudPaper role="region" aria-label="Input parameters">
    <MudForm aria-label="Simple interest calculation inputs">
        <!-- Input with descriptive label -->
        <MudNumericField Label="Principal Amount"
                         aria-label="Principal amount in Indian Rupees"
                         HelperText="Enter amount between ₹10,000 and ₹10,00,00,000" />
    </MudForm>
</MudPaper>

<!-- Results with clear labeling -->
<div role="region" aria-label="Calculation results">
    <MudChart aria-label="Breakdown of principal and interest earned" />
</div>
```

**Best Practices**:

- All interactive elements have labels
- Sections have roles and aria-labels
- Charts have descriptive aria-labels
- Color is not the only indicator of meaning
- WCAG AA color contrast maintained

---

## Testing Patterns

### 1. AAA Pattern (Arrange-Act-Assert)

**Pattern**: Structure tests into three clear sections.

**Example**:

```csharp
[Fact]
public void CalculateResult_WithValidInputs_ReturnsCorrectCalculation()
{
    // Arrange
    var input = new SimpleInterestInputViewModel
    {
        PrincipalAmount = 100000,
        RateOfInterest = 5,
        TimePeriodInYears = 5
    };
    var service = new SimpleInterestCalculatorService();

    // Act
    var result = service.Calculate(input);

    // Assert
    result.InterestEarned.Should().Be(25000);
    result.TotalAmount.Should().Be(125000);
}
```

**Benefits**:

- Clear test structure
- Easy to understand intent
- Isolates each concern

---

### 2. Fluent Assertions

**Pattern**: Use FluentAssertions for readable, expressive test assertions.

**Example**:

```csharp
// Instead of:
Assert.Equal(25000, result.InterestEarned);

// Use:
result.InterestEarned.Should().Be(25000);
result.TotalAmount.Should().BeGreaterThan(result.PrincipalAmount);
result.ChartData.Should().HaveCount(2);
```

**Benefits**:

- More readable test failures
- Natural language-like syntax
- Better error messages

---

### 3. Parameterized Tests with Theory

**Pattern**: Use `[Theory]` with `[InlineData]` to test multiple scenarios concisely.

**Example**:

```csharp
[Theory]
[InlineData(100000, 5, 5, 25000)]
[InlineData(50000, 10, 3, 15000)]
[InlineData(200000, 7.5, 10, 150000)]
public void CalculateResult_WithVariousInputs_ReturnsCorrectInterest(
    int principal, double rate, int years, int expectedInterest)
{
    // Arrange
    var input = new SimpleInterestInputViewModel
    {
        PrincipalAmount = principal,
        RateOfInterest = rate,
        TimePeriodInYears = years
    };
    var service = new SimpleInterestCalculatorService();

    // Act
    var result = service.Calculate(input);

    // Assert
    result.InterestEarned.Should().Be(expectedInterest);
}
```

**Benefits**:

- Tests multiple scenarios with one method
- Easy to add new test cases
- Clear relationship between inputs and expected outputs

---

### 4. Test Data Generation with Bogus

**Pattern**: Use Bogus library to generate random valid test data.

**Example**:

```csharp
private static SimpleInterestInputViewModel GenerateValidInput()
{
    var faker = new Faker();
    return new SimpleInterestInputViewModel
    {
        PrincipalAmount = faker.Random.Int(10000, 1000000),
        RateOfInterest = faker.Random.Double(0.01, 20),
        TimePeriodInYears = faker.Random.Int(1, 30)
    };
}

[Fact]
public void CalculateResult_WithRandomValidInputs_DoesNotThrow()
{
    // Arrange
    var input = GenerateValidInput();
    var service = new SimpleInterestCalculatorService();

    // Act
    Action act = () => service.Calculate(input);

    // Assert
    act.Should().NotThrow();
}
```

**Benefits**:

- Discovers edge cases automatically
- Reduces test data boilerplate
- Ensures code handles variety of inputs

---

### 5. E2E Page Object Pattern

**Pattern**: Encapsulate page interactions in reusable methods for Playwright tests.

**Example**:

```csharp
public class SimpleInterestCalculatorPage
{
    private readonly IPage _page;

    public SimpleInterestCalculatorPage(IPage page)
    {
        _page = page;
    }

    public async Task NavigateAsync()
    {
        await _page.GotoAsync("https://localhost:5000/");
    }

    public async Task FillPrincipalAmount(int amount)
    {
        await _page.FillAsync("[aria-label='Principal amount in Indian Rupees']", amount.ToString());
    }

    public async Task<string> GetTotalAmount()
    {
        return await _page.TextContentAsync("[data-testid='total-amount']");
    }
}
```

**Benefits**:

- Reusable page interactions
- Easier to maintain E2E tests
- Abstraction from implementation details

---

## Summary

FinSkew's design patterns provide:

- **Consistency**: Predictable structure across components
- **Maintainability**: Clear separation of concerns
- **Testability**: Patterns designed for easy testing
- **Developer Experience**: Convention over configuration reduces boilerplate
- **User Experience**: Real-time updates, responsive design, accessibility

These patterns should guide the implementation of new features and calculators to ensure the codebase remains coherent and maintainable.

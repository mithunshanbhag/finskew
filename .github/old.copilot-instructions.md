# FinSkew Development Guide

FinSkew is a Blazor WebAssembly financial calculator app targeting .NET 10.0, with Azure Static Web Apps deployment.

## Build, Test, and Run Commands

### Building
```powershell
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Build specific project
dotnet build src\FinSkew.Ui\FinSkew.Ui.csproj
```

### Testing
```powershell
# Run all tests (unit + E2E)
dotnet test

# Run only unit tests (fast, ~2-3 seconds)
dotnet test tests\FinSkew.Ui.UnitTests\FinSkew.Ui.UnitTests.csproj

# Run specific test class
dotnet test tests\FinSkew.Ui.UnitTests --filter "FullyQualifiedName~SimpleInterestCalculationTests"

# Run single test
dotnet test tests\FinSkew.Ui.UnitTests --filter "Name=CalculateResult_WithValidInputs_ReturnsCorrectCalculation"

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"

# E2E tests (requires app running at http://localhost:5000)
# Terminal 1:
.\run-local.ps1
# Terminal 2:
dotnet test tests\FinSkew.Ui.E2ETests\FinSkew.Ui.E2ETests.csproj
```

### Running Locally
```powershell
# Start the Blazor WASM app on http://localhost:5000
.\run-local.ps1
```

### Linting
```powershell
# Verify formatting (does not modify files)
dotnet format --verify-no-changes

# Auto-format code
dotnet format
```

## Architecture Overview

### Tech Stack
- **Frontend**: Blazor WebAssembly (.NET 10.0) with MudBlazor UI components
- **Planned Backend**: Azure Function Apps (not yet implemented)
- **Planned Database**: Azure Cosmos DB NoSQL API (not yet implemented)
- **Deployment**: Azure Static Web Apps (configured via Bicep in `infra/`)

### Project Structure
```
src/FinSkew.Ui/               # Blazor WASM client application
  Components/
    Layout/                   # MainLayout, NavMenu, AppBar
    Pages/                    # Calculator pages (.razor files)
    Shared/                   # Reusable components (PageHeader, etc.)
  Models/
    ViewModels/
      InputModels/            # Input view models with validation
      ResultModels/           # Calculation result models
  Services/
    Interfaces/               # Service contracts
    Implementations/          # Service implementations
  Constants/                  # App-wide constants
  Repositories/               # Data access (currently minimal)

tests/
  FinSkew.Ui.UnitTests/       # XUnit tests (86 passing tests)
  FinSkew.Ui.E2ETests/        # Playwright browser tests

docs/
  specs/                      # Functional specs for each calculator
  architecture/               # Architecture diagrams (placeholder)

infra/                        # Azure Bicep deployment files
```

### Key Architectural Patterns

**Input/Result View Model Pattern**: Each calculator has two view models:
- `*InputViewModel`: Holds user inputs with validation attributes
- `*ResultViewModel`: Holds calculation outputs and chart data

**Multiple Route Aliases**: Calculator pages register multiple `@page` directives for SEO and usability (e.g., `/`, `/sic`, `/simple-interest-calculator` all route to SimpleInterestCalculator).

**Real-time Calculation with Debouncing**: Calculators update results instantly as users type (no "Calculate" button), but debounce rapid changes to avoid excessive computation.

**Component-Based Layout**: Uses MudBlazor's responsive grid system. Desktop shows inputs and results side-by-side; mobile/tablet stacks vertically.

**Accessibility-First**: All interactive elements include ARIA labels, keyboard navigation support, and WCAG AA color contrast.

## Code Conventions

### C# Naming and Style
- **PascalCase** for public members, types, namespaces
- **camelCase** for private fields prefixed with `_` (e.g., `_inputViewModel`)
- **Explicit types** over `var` when the type isn't immediately obvious
- **Async/await** for all I/O operations
- Keep methods small and focused (single responsibility)

### View Models
- Input models inherit shared base properties when applicable (e.g., `CompoundInterestInputViewModel` extends `SimpleInterestInputViewModel`)
- Result models use **required properties** for mandatory calculations
- Use data annotations (`[Range]`, `[Required]`) for validation

### Blazor Razor Files
- Use **code-behind partial classes** for complex logic (`.razor.cs` files)
- Keep Razor markup focused on presentation
- Use `@bind-Value` for two-way data binding with MudBlazor components
- Include accessibility attributes (`aria-label`, `role`, `aria-describedby`)

### Testing Conventions
- **Unit test names**: `MethodName_Scenario_ExpectedBehavior`
  - Example: `CalculateResult_WithValidInputs_ReturnsCorrectCalculation`
- **E2E test names**: `ComponentName_Action_ExpectedResult`
  - Example: `SimpleInterestCalculator_PageLoads_Successfully`
- Use **AAA pattern** (Arrange, Act, Assert)
- Use **FluentAssertions** for readable assertions (`.Should().Be()`, `.Should().BeInRange()`)
- Use **Bogus** for generating random valid test data
- Use **[Theory]** with **[InlineData]** for parameterized tests

### UI/UX Patterns
- **Indian numbering system**: Format currency with commas every 2 digits after the first 3 (e.g., ₹10,00,000)
- **Currency symbol**: Use ₹ (Indian Rupee) in all money-related fields
- **Input adornments**: Always show leading icon adornments (₹ for money, % for rates, clock for time)
- **Chart legends**: Include legends for all charts to clarify visual elements
- **Breadcrumbs**: Display breadcrumb trail at top of each calculator page for navigation context
- **Tooltips**: Provide contextual help on hover for interactive elements

### Typography Scale
- **Page titles**: Heading 5 (Medium 500)
- **Section headers**: Heading 6 (Medium 500)
- **Hero numbers** (chart centers): Heading 4 (Bold 700)
- **Input labels**: Subtitle 2 (Medium 500)
- **Summary labels**: Body 2 (Bold 700)
- **Summary values**: Body 1 (Regular 400)
- **Chart legends**: Caption (Regular 400)

### MudBlazor Component Usage
- Use **Variant.Outlined** for input fields
- Use **Color.Tertiary** for input adornments
- Use **Color.Primary** for action buttons and app bar
- Use **MudStack** for responsive layouts (with `Row="true"` on desktop)
- Use **MudPaper** for section containers with `pa-2 ma-2` padding/margin

## Calculator Specifications

All calculator specifications are documented in `docs/specs/`:
- `simple-interest-calculator.md` - Simple interest calculator (fully implemented and tested)
- `compound-interest-calculator.md` - Compound interest calculator (basic implementation)
- `sip-calculator.md` - Systematic Investment Plan calculator (UI exists, needs implementation)
- Plus SWP, STP, and Lumpsum calculators (planned)

When adding or modifying calculators, refer to these specs for:
- Input field constraints (min/max, step values)
- Calculation formulas
- Expected output format and charts
- Accessibility requirements

## Important Notes

### E2E Test Requirements
- E2E tests **require the app to be running** at `http://localhost:5000` before execution
- Playwright browsers must be installed: `pwsh tests\FinSkew.Ui.E2ETests\bin\Debug\net10.0\playwright.ps1 install`
- E2E tests run in **headless Chromium** for CI/CD compatibility

### Contributions
- This project accepts contributions **selectively** - always open an issue first for discussion before submitting PRs
- PRs must link to an approved issue
- All new functionality must include unit tests

### Deployment
- Deployment to Azure is configured via GitHub Actions (`.github/workflows/deploy.yml`)
- Infrastructure is defined as Bicep templates in `infra/`
- The app is deployed as an Azure Static Web App

## Related Documentation
- **Tests**: See `tests/README.md` for comprehensive test documentation
- **Quick start**: See `tests/QUICK_START.md` for rapid test execution
- **Contributing**: See `CONTRIBUTING.md` for contribution guidelines
- **Tech stack preferences**: See `AGENTS.md` for author's preferred stack (Blazor WASM, Azure Functions, Cosmos DB)

# Project Structure

> **Document Purpose**: Comprehensive guide to FinSkew's directory organization, file naming conventions, and module boundaries.

## Table of Contents

- [Overview](#overview)
- [Repository Root Structure](#repository-root-structure)
- [Source Code Structure](#source-code-structure)
- [Test Project Structure](#test-project-structure)
- [Documentation Structure](#documentation-structure)
- [Infrastructure Structure](#infrastructure-structure)
- [Naming Conventions](#naming-conventions)
- [Module Boundaries](#module-boundaries)
- [Dependencies](#dependencies)

## Overview

FinSkew follows a well-organized directory structure that separates concerns and makes navigation intuitive. The structure is designed to scale as new calculators and features are added.

## Repository Root Structure

```
finskew/
├── .github/                    # GitHub-specific files
│   ├── workflows/              # CI/CD pipeline definitions
│   ├── ISSUE_TEMPLATE/         # Issue templates
│   ├── agents/                 # Custom Copilot agents
│   ├── prompts/                # AI prompt templates
│   ├── PULL_REQUEST_TEMPLATE.md
│   └── copilot-instructions.md
├── docs/                       # Documentation
│   ├── architecture/           # Architecture documentation
│   └── specs/                  # Functional specifications
├── infra/                      # Infrastructure as Code (Bicep)
├── samples/                    # Sample files (e.g., test data)
├── src/                        # Source code
│   └── FinSkew.Ui/             # Main Blazor WASM project
├── tests/                      # Test projects
│   ├── FinSkew.Ui.UnitTests/   # Unit tests
│   └── FinSkew.Ui.E2ETests/    # End-to-end tests
├── .gitignore                  # Git ignore rules
├── AGENTS.md                   # Agent development guide
├── CODE_OF_CONDUCT.md          # Community guidelines
├── CONTRIBUTING.md             # Contribution guidelines
├── FinSkew.slnx                # Solution file
├── LICENSE                     # MIT License
├── README.md                   # Project readme
├── run-local.ps1               # Local development script
└── SECURITY.md                 # Security policy
```

### Key Root Files

- **`FinSkew.slnx`**: Visual Studio solution file containing all projects
- **`run-local.ps1`**: PowerShell script to run the app locally on port 5000
- **`CONTRIBUTING.md`**: Guidelines for contributing (selective contributions, issue-first approach)
- **`AGENTS.md`**: Documentation for GitHub Copilot custom agents

## Source Code Structure

### FinSkew.Ui Project Layout

```
src/FinSkew.Ui/
├── Components/                 # Blazor components
│   ├── Layout/                 # Layout components
│   │   └── MainLayout.razor
│   ├── Pages/                  # Routable page components
│   │   ├── SimpleInterestCalculator.razor
│   │   ├── CompoundInterestCalculator.razor
│   │   ├── SIPCalculator.razor
│   │   ├── SWPCalculator.razor
│   │   ├── STPCalculator.razor
│   │   ├── LumpsumCalculator.razor
│   │   └── NotFound.razor
│   └── Shared/                 # Reusable shared components
│       └── PageHeader.razor
├── Constants/                  # Application constants
│   ├── ConfigKeys.cs
│   ├── HttpClientNameConstants.cs
│   ├── RouteConstants.cs
│   └── UrlConstants.cs
├── Misc/                       # Miscellaneous files
├── Models/                     # Data models
│   ├── CsvModels/              # CSV import/export models (placeholder)
│   ├── MapperProfile.cs        # AutoMapper configuration (if used)
│   └── ViewModels/             # View models
│       ├── InputModels/        # Input view models with validation
│       │   ├── SimpleInterestInputViewModel.cs
│       │   └── CompoundInterestInputViewModel.cs
│       └── ResultModels/       # Calculation result models
│           ├── SimpleInterestResultViewModel.cs
│           ├── SimpleInterestGrowthViewModel.cs
│           ├── CompoundInterestResultViewModel.cs
│           └── CompoundInterestGrowthViewModel.cs
├── Properties/                 # Project properties
│   └── launchSettings.json     # Development launch profiles
├── Repositories/               # Data access layer (minimal, future use)
├── Services/                   # Business logic services
│   ├── Interfaces/             # Service contracts
│   │   └── IService1.cs        # Placeholder interface
│   └── Implementations/        # Service implementations
│       └── Service1.cs         # Placeholder implementation
├── wwwroot/                    # Static web assets
│   ├── css/                    # Stylesheets
│   ├── images/                 # Images and icons
│   ├── favicon.svg             # Favicon
│   └── index.html              # HTML entry point
├── App.razor                   # Root application component
├── _Imports.razor              # Global Razor imports
├── FinSkew.Ui.csproj           # Project file
├── GlobalUsings.cs             # Global C# using statements
└── Program.cs                  # Application entry point
```

### Component Organization

**Layout Components** (`Components/Layout/`):
- Contain application-wide layout components
- Currently: `MainLayout.razor` (main layout with app bar and drawer)

**Page Components** (`Components/Pages/`):
- Routable components with `@page` directives
- One component per calculator type
- Convention: `{CalculatorType}Calculator.razor`
- Examples: `SimpleInterestCalculator.razor`, `SIPCalculator.razor`

**Shared Components** (`Components/Shared/`):
- Reusable components used across multiple pages
- Currently: `PageHeader.razor` (breadcrumbs and action buttons)
- Future: chart components, result summaries, input field wrappers

### Model Organization

**ViewModels** (`Models/ViewModels/`):

1. **InputModels/**:
   - Contains user input view models
   - Include validation attributes (`[Range]`, `[Required]`)
   - Naming: `{CalculatorType}InputViewModel.cs`
   - Examples: `SimpleInterestInputViewModel.cs`

2. **ResultModels/**:
   - Contains calculation result view models
   - Immutable (using `required init` properties)
   - May include chart data and growth models
   - Naming: `{CalculatorType}ResultViewModel.cs`
   - Examples: `SimpleInterestResultViewModel.cs`, `SimpleInterestGrowthViewModel.cs`

**CsvModels** (`Models/CsvModels/`):
- Placeholder for future CSV import/export functionality
- Currently empty

### Service Organization

**Interfaces** (`Services/Interfaces/`):
- Service contracts (interfaces)
- Naming: `I{ServiceName}.cs`
- Example: `ISimpleInterestCalculatorService.cs`

**Implementations** (`Services/Implementations/`):
- Concrete service implementations
- Naming: `{ServiceName}.cs` (matches interface without 'I')
- Example: `SimpleInterestCalculatorService.cs`

**Current State**: Contains placeholder `IService1.cs` and `Service1.cs`. Real calculator services will be added here.

### Constants Organization

**Purpose**: Centralize magic strings and configuration values.

- **`ConfigKeys.cs`**: Configuration key names
- **`HttpClientNameConstants.cs`**: Named HttpClient identifiers
- **`RouteConstants.cs`**: Route paths for navigation
- **`UrlConstants.cs`**: External URLs (e.g., GitHub repo)

**Benefits**:
- Refactoring safety (string literals in one place)
- IntelliSense support
- Avoids typos

### Static Assets (`wwwroot/`)

```
wwwroot/
├── css/
│   └── app.css                 # Global styles
├── images/
│   ├── logo.svg                # Application logo
│   └── ...                     # Other images
├── favicon.svg                 # Favicon
├── index.html                  # HTML host page
└── ...                         # Other static files
```

**Purpose**: Static files served directly by the web server, not processed by .NET.

## Test Project Structure

### Unit Tests Project

```
tests/FinSkew.Ui.UnitTests/
├── Services/                   # Service tests
│   └── SimpleInterestCalculatorServiceTests.cs
├── ViewModels/                 # ViewModel tests
│   ├── InputModels/
│   └── ResultModels/
├── FinSkew.Ui.UnitTests.csproj
└── GlobalUsings.cs
```

**Characteristics**:
- Fast-running tests (~2-3 seconds for all tests)
- No external dependencies
- Uses xUnit, FluentAssertions, Bogus

**Naming Convention**:
- Test class: `{ClassUnderTest}Tests.cs`
- Test method: `{MethodName}_{Scenario}_{ExpectedBehavior}`

### E2E Tests Project

```
tests/FinSkew.Ui.E2ETests/
├── Tests/                      # E2E test classes
│   └── SimpleInterestCalculatorTests.cs
├── PageObjects/                # Page object models (future)
├── FinSkew.Ui.E2ETests.csproj
└── GlobalUsings.cs
```

**Characteristics**:
- Requires app running on localhost:5000
- Uses Playwright for browser automation
- Tests in headless Chromium

**Naming Convention**:
- Test class: `{PageName}Tests.cs`
- Test method: `{ComponentName}_{Action}_{ExpectedResult}`

## Documentation Structure

```
docs/
├── architecture/               # Architecture documentation
│   ├── README.md               # Main architecture overview
│   ├── frontend-architecture.md
│   ├── design-patterns.md
│   ├── project-structure.md    # This file
│   └── deployment.md
└── specs/                      # Functional specifications
    ├── README.md               # Calculator specs overview
    ├── simple-interest-calculator.md
    ├── compound-interest-calculator.md
    ├── sip-calculator.md
    ├── swp-calculator.md       # (planned)
    ├── stp-calculator.md       # (planned)
    └── lumpsum-calculator.md   # (planned)
```

**Architecture Docs**:
- High-level system design
- Technical implementation details
- Living documents updated with code changes

**Spec Docs**:
- Functional requirements
- UI/UX specifications
- Calculation formulas
- Input constraints

## Infrastructure Structure

```
infra/
├── createResourceGroups.bicep  # Resource group definitions
├── createResources.bicep       # Main resource provisioning
└── createExtensionResources.bicep # Extension/add-on resources
```

**Purpose**: Bicep templates for Azure infrastructure provisioning.

**Deployment Target**: Azure Static Web Apps

**Usage**: Referenced by GitHub Actions workflow for automated deployment.

## Naming Conventions

### File Naming

**Razor Components**:
- PascalCase
- Descriptive names
- Examples: `SimpleInterestCalculator.razor`, `PageHeader.razor`, `MainLayout.razor`

**C# Classes**:
- PascalCase
- Match file name exactly
- Examples: `SimpleInterestInputViewModel.cs`, `SimpleInterestCalculatorService.cs`

**Constants Classes**:
- Suffix with `Constants`
- Examples: `RouteConstants.cs`, `ConfigKeys.cs`

**Test Classes**:
- Suffix with `Tests`
- Match class under test
- Examples: `SimpleInterestCalculatorServiceTests.cs`

### C# Naming Conventions

**Classes/Interfaces/Records**:
- PascalCase
- Interfaces prefixed with `I`
- Examples: `SimpleInterestInputViewModel`, `ICalculatorService`

**Methods/Properties**:
- PascalCase
- Examples: `Calculate()`, `PrincipalAmount`

**Private Fields**:
- camelCase with `_` prefix
- Examples: `_inputViewModel`, `_debounceTimer`

**Local Variables**:
- camelCase
- Examples: `result`, `inputViewModel`

**Constants**:
- PascalCase (for public) or UPPER_SNAKE_CASE (for private)
- Examples: `public const int MaxPrincipal = 100000000;`

### Namespace Conventions

**Pattern**: `FinSkew.Ui.{FolderPath}`

**Examples**:
- `FinSkew.Ui.Components.Pages`
- `FinSkew.Ui.Models.ViewModels.InputModels`
- `FinSkew.Ui.Services.Implementations`

**Benefits**:
- Namespaces mirror folder structure
- Easy to locate files
- IntelliSense-friendly

## Module Boundaries

### Presentation Module
**Responsibility**: UI rendering and user interaction

**Components**:
- `Components/` directory
- Razor files (`.razor`)

**Dependencies**:
- Services (via dependency injection)
- ViewModels (for data binding)
- MudBlazor components

**Rules**:
- No business logic in components
- Delegate calculations to services
- Only handle UI state and events

---

### Business Logic Module
**Responsibility**: Calculation algorithms and business rules

**Components**:
- `Services/` directory
- Calculator service implementations

**Dependencies**:
- ViewModels (input/result contracts)
- No UI dependencies

**Rules**:
- Pure functions preferred
- Stateless services
- No knowledge of UI framework

---

### Data Module
**Responsibility**: Data structures and validation

**Components**:
- `Models/ViewModels/` directory
- Input and result view models

**Dependencies**:
- System libraries only
- Data annotation attributes

**Rules**:
- No business logic
- Validation via attributes
- Immutable results

---

### Infrastructure Module
**Responsibility**: Framework integration and utilities

**Components**:
- `Program.cs` (DI configuration)
- `Constants/` directory
- `wwwroot/` static assets

**Dependencies**:
- All other modules

**Rules**:
- Configure, don't implement
- Provide cross-cutting concerns

## Dependencies

### Package Dependencies

```xml
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="10.0.2" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.DevServer" Version="10.0.2" />
<PackageReference Include="MudBlazor" Version="8.15.0" />
<PackageReference Include="MithunShanbhag.Nucleus" Version="1.0.0-alpha.4" />
```

**Core Dependencies**:
- **Blazor WebAssembly**: Framework runtime
- **MudBlazor**: UI component library
- **MithunShanbhag.Nucleus**: Custom utility library (author's personal package)

### Inter-Project Dependencies

```
FinSkew.Ui (no dependencies)

FinSkew.Ui.UnitTests → FinSkew.Ui

FinSkew.Ui.E2ETests → (external: requires running app)
```

**Dependency Rules**:
- Test projects reference main project
- Main project is self-contained
- No circular dependencies

### Internal Module Dependencies

```
Components → Services → ViewModels
     ↓           ↓
Constants   (no dependencies)
```

**Flow**:
1. Components depend on Services and ViewModels
2. Services depend on ViewModels
3. ViewModels have no internal dependencies
4. Constants are used by all modules but depend on nothing

## Future Structure Considerations

### When Backend is Added

```
src/
├── FinSkew.Ui/                 # Frontend (existing)
├── FinSkew.Api/                # Azure Functions backend
│   ├── Functions/              # HTTP-triggered functions
│   ├── Models/                 # DTOs for API contracts
│   ├── Services/               # Backend business logic
│   └── Repositories/           # Cosmos DB data access
└── FinSkew.Shared/             # Shared DTOs and contracts
    ├── Models/
    └── Constants/
```

### When More Calculators are Added

**Scalability Considerations**:
- Consider grouping calculators by category in subdirectories
- Example: `Components/Pages/InterestCalculators/`, `Components/Pages/InvestmentCalculators/`
- Extract common calculator components to `Components/Shared/`
- Consider base classes for common calculator logic

## Summary

FinSkew's project structure provides:

- **Clear Separation**: Distinct folders for concerns (components, services, models)
- **Discoverability**: Intuitive naming and organization
- **Scalability**: Structure supports growth (more calculators, backend addition)
- **Maintainability**: Conventions reduce cognitive load
- **Testability**: Parallel test structure mirrors source code

Following these conventions ensures the codebase remains navigable and maintainable as it evolves.

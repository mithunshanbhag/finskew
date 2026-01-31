# FinSkew Tests

This directory contains the test suite for the FinSkew application.

## Test Structure

The tests are organized into two main categories:

### Unit Tests (`FinSkew.Ui.UnitTests`)

Unit tests verify the correctness of individual components, models, and calculation logic in isolation.

**Test Coverage:**
- **Calculation Logic Tests**
  - `SimpleInterestCalculationTests.cs` - Tests simple interest calculations with various inputs
  - `CompoundInterestCalculationTests.cs` - Tests compound interest calculations with different compounding frequencies

- **View Model Tests**
  - Input Models:
    - `SimpleInterestInputViewModelTests.cs` - Tests simple interest input validation and defaults
    - `CompoundInterestInputViewModelTests.cs` - Tests compound interest input validation and defaults
  - Result Models:
    - `SimpleInterestResultViewModelTests.cs` - Tests simple interest result model properties
    - `CompoundInterestResultViewModelTests.cs` - Tests compound interest result model properties

**Technologies:**
- XUnit - Testing framework
- FluentAssertions 7.2.x - Fluent assertion library
- Moq - Mocking framework
- Bogus - Fake data generator

### End-to-End Tests (`FinSkew.Ui.E2ETests`)

E2E tests verify the complete user workflows and interactions in a browser environment.

**Test Coverage:**
- `SimpleInterestCalculatorE2ETests.cs` - Tests the simple interest calculator UI
  - Page loading and navigation
  - Input validation and calculation
  - Chart and result display
  - Responsive design
- `CompoundInterestCalculatorE2ETests.cs` - Tests the compound interest calculator UI
  - Page loading and navigation
  - Breadcrumb navigation
- `AppNavigationE2ETests.cs` - Tests overall app navigation
  - Route aliases and URL routing
  - Responsive design across devices (mobile, tablet, desktop)
  - Keyboard navigation
  - App bar visibility

**Technologies:**
- Playwright - Browser automation library (running in headless mode)
- XUnit - Testing framework with Collection Fixtures for application lifecycle management

## Running Tests

### Prerequisites

1. Ensure you have .NET 10 SDK installed
2. For E2E tests, install Playwright browsers:
   ```powershell
   pwsh bin\Debug\net10.0\playwright.ps1 install
   ```

### Running Unit Tests

From the repository root:

```powershell
dotnet test tests\FinSkew.Ui.UnitTests\FinSkew.Ui.UnitTests.csproj
```

### Running E2E Tests

E2E tests now **automatically launch the application** using the `WebServerFixture`. No manual setup required!

Simply run:
```powershell
dotnet test tests\FinSkew.Ui.E2ETests\FinSkew.Ui.E2ETests.csproj
```

**How it works:**
- The `WebServerFixture` builds and starts the Blazor WASM app before any tests run
- Tests execute against `http://localhost:5000`
- The fixture waits up to 60 seconds for the app to be ready
- After all tests complete, the app is automatically stopped

**Manual Mode (Optional):**
If you prefer to run the app manually (e.g., for debugging):
1. Start the application: `.\run-local.ps1`
2. Run tests: `dotnet test tests\FinSkew.Ui.E2ETests\FinSkew.Ui.E2ETests.csproj`

### Running All Tests

To run all tests (unit + E2E):

```powershell
dotnet test
```

**Note:** E2E tests will automatically launch the application, so no manual setup is needed.

## Test Configuration

### E2E Test Configuration

- **Base URL:** `http://localhost:5000` (configured in `PlaywrightTest` base class)
- **Browser:** Chromium (headless mode)
- **Viewport Sizes:**
  - Mobile: 375x667 (iPhone SE)
  - Tablet: 768x1024 (iPad)
  - Desktop: 1920x1080 (Full HD)
- **Application Lifecycle:** Managed by `WebServerFixture` using XUnit Collection Fixtures
- **Startup Timeout:** 60 seconds (configurable in `WebServerFixture.cs`)

## CI/CD Integration

These tests are designed to run in CI/CD pipelines:

- **Unit Tests:** No special requirements, run in any .NET environment
- **E2E Tests:** 
  - Require Playwright browsers to be installed
  - Run in headless mode for CI/CD compatibility
  - **Automatically build and start the application** (no manual intervention needed)
  - The `WebServerFixture` handles the complete application lifecycle

## Writing New Tests

### Unit Test Guidelines

1. Follow the AAA pattern (Arrange, Act, Assert)
2. Use descriptive test names that explain the scenario
3. Use `Theory` with `InlineData` for parameterized tests
4. Use Bogus for generating random test data
5. Use FluentAssertions for readable assertions

### E2E Test Guidelines

1. Always extend `PlaywrightTest` base class for browser management
2. Add `[Collection("E2E Tests")]` attribute to your test class to use the shared `WebServerFixture`
2. Close pages in `finally` blocks to prevent resource leaks
3. Use `WaitForLoadStateAsync(LoadState.NetworkIdle)` after navigation
4. Use semantic selectors (AriaRole, labels) over CSS selectors when possible
5. Test responsive design by setting viewport sizes
6. Keep tests focused on user workflows, not implementation details

## Test Naming Conventions

- **Unit Tests:** `MethodName_Scenario_ExpectedBehavior`
  - Example: `CalculateResult_WithValidInputs_ReturnsCorrectCalculation`
  
- **E2E Tests:** `ComponentName_Action_ExpectedResult`
  - Example: `SimpleInterestCalculator_PageLoads_Successfully`

## Code Coverage

To generate code coverage reports:

```powershell
dotnet test --collect:"XPlat Code Coverage"
```

Coverage reports will be generated in the `TestResults` directory.

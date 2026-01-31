# Quick Start Guide - Running Tests

## Prerequisites

- .NET 10 SDK installed
- For E2E tests: Playwright browsers (installation instructions below)

## Quick Commands

### Run All Unit Tests
```powershell
dotnet test tests\FinSkew.Ui.UnitTests
```

### Run Specific Test Class
```powershell
dotnet test tests\FinSkew.Ui.UnitTests --filter "FullyQualifiedName~SimpleInterestCalculationTests"
```

### Run Single Test
```powershell
dotnet test tests\FinSkew.Ui.UnitTests --filter "Name=CalculateResult_WithValidInputs_ReturnsCorrectCalculation"
```

### Run with Detailed Output
```powershell
dotnet test tests\FinSkew.Ui.UnitTests --logger "console;verbosity=detailed"
```

### Run with Code Coverage
```powershell
dotnet test tests\FinSkew.Ui.UnitTests --collect:"XPlat Code Coverage"
```

## E2E Tests Setup

### 1. Install Playwright Browsers (First Time Only)
```powershell
# Build the E2E project first
dotnet build tests\FinSkew.Ui.E2ETests

# Install Playwright browsers
pwsh tests\FinSkew.Ui.E2ETests\bin\Debug\net10.0\playwright.ps1 install
```

### 2. Run E2E Tests
```powershell
# Terminal 1: Start the application
.\run-local.ps1

# Terminal 2: Run E2E tests
dotnet test tests\FinSkew.Ui.E2ETests
```

## Troubleshooting

### Unit Tests Fail to Restore Packages
```powershell
# Clear NuGet cache and restore
dotnet nuget locals all --clear
dotnet restore tests\FinSkew.Ui.UnitTests
```

### E2E Tests Fail - "Browser Not Found"
```powershell
# Reinstall Playwright browsers
pwsh tests\FinSkew.Ui.E2ETests\bin\Debug\net10.0\playwright.ps1 install chromium
```

### E2E Tests Fail - "Connection Refused"
- Make sure the application is running at `http://localhost:5000`
- Check that no other process is using port 5000

### Tests Pass Locally but Fail in CI/CD
- Ensure .NET 10 SDK is installed in CI environment
- For E2E: Install Playwright browsers in CI pipeline
- For E2E: Start application before running tests

## Watch Mode (Continuous Testing)

Run tests automatically when code changes:

```powershell
dotnet watch test --project tests\FinSkew.Ui.UnitTests
```

## Test Results

Test results are written to:
- Console output (standard)
- `TestResults` folder (when using coverage or custom logger)

## IDE Integration

### Visual Studio
- Open Test Explorer: View > Test Explorer
- Tests appear automatically
- Run/debug from Test Explorer

### Visual Studio Code
- Install ".NET Core Test Explorer" extension
- Tests appear in Test Explorer side panel
- Run/debug tests with CodeLens

### JetBrains Rider
- Tests appear in Unit Tests window
- Run/debug with context menu or keyboard shortcuts

## Best Practices

1. **Run unit tests frequently** - They're fast (~2-3 seconds)
2. **Run E2E tests before committing** - Catch integration issues early
3. **Check code coverage periodically** - Ensure adequate test coverage
4. **Use watch mode during development** - Immediate feedback on changes
5. **Run full test suite before PR** - Ensure nothing is broken

## Need Help?

See `README.md` in the tests folder for comprehensive documentation.

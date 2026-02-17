# LEARNINGS

## 2026-02-17T09:06:00Z

- Project uses .NET 10; the main app is at `src\FinSkew.Ui\FinSkew.Ui.csproj` (Blazor WebAssembly).
- Run locally with `dotnet run --project .\src\FinSkew.Ui\FinSkew.Ui.csproj` or `.\run-local.ps1`.
- Run all tests with `dotnet test .\FinSkew.slnx` or `.\run-local.ps1 tests`.
  - Run only unit tests: `dotnet test .\tests\FinSkew.Ui.UnitTests\FinSkew.Ui.UnitTests.csproj` or `.\run-local.ps1 unit-tests`.
  - Run only E2E tests: `dotnet test .\tests\FinSkew.Ui.E2ETests\FinSkew.Ui.E2ETests.csproj` or `.\run-local.ps1 e2e-tests`.
- Current automated test projects:
  - `tests\FinSkew.Ui.UnitTests\FinSkew.Ui.UnitTests.csproj`
  - `tests\FinSkew.Ui.E2ETests\FinSkew.Ui.E2ETests.csproj`

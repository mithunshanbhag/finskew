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

## 2026-02-17T09:45:43Z

- Adding a new calculator in `FinSkew.Ui` follows a repeatable wiring pattern:
  - add route key in `src\FinSkew.Ui\Constants\RouteConstants.cs`
  - add page in `src\FinSkew.Ui\Components\Pages\`
  - register calculator service in `src\FinSkew.Ui\Misc\ExtensionMethods\WebAssemblyHostBuilderExtensions.cs`
  - add navigation link in `src\FinSkew.Ui\Components\Shared\NavMenu.razor`
- Validation commands used for this feature:
  - `dotnet build --nologo`
  - `dotnet test .\tests\FinSkew.Ui.UnitTests\FinSkew.Ui.UnitTests.csproj --nologo -v minimal`
  - `dotnet test .\tests\FinSkew.Ui.E2ETests\FinSkew.Ui.E2ETests.csproj --nologo -v minimal`

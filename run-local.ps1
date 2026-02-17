param(
    [ValidateSet("app", "tests", "e2e-tests", "unit-tests")]
    [string]$target = "app"
)

switch ($target) {
    "app" {
        dotnet run --project ./src/FinSkew.Ui/FinSkew.Ui.csproj
    }
    "tests" {
        dotnet test ./FinSkew.slnx
    }
    "e2e-tests" {
        dotnet test ./tests/FinSkew.Ui.E2ETests/FinSkew.Ui.E2ETests.csproj
    }
    "unit-tests" {
        dotnet test ./tests/FinSkew.Ui.UnitTests/FinSkew.Ui.UnitTests.csproj
    }
    default {
        Write-Host "Invalid target specified. Please use one of: app, tests, e2e-tests, unit-tests."
    }
}

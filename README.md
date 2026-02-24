# FinSkew: Your Financial Calculators

[![Build Status](https://github.com/mithunshanbhag/finskew/actions/workflows/deploy.yml/badge.svg)](https://github.com/mithunshanbhag/finskew/actions/workflows/deploy.yml)
![License](https://img.shields.io/badge/license-MIT-blue)

![FinSkew Demo](./docs/assets/images/reference-screenshot.png)

## Usage

FinSkew is a Blazor WebAssembly application that provides various financial calculators and tools:

| Category               | Calculators                                      |
| ---------------------- | ------------------------------------------------ |
| Income calculators     | Gratuity                                         |
| Interest calculators   | Simple interest, Compound interest, CAGR, XIRR   |
| Investment calculators | Lump sum investment, SIP, Step-Up SIP, STP, SCSS |
| Loan calculators       | EMI                                              |
| Retirement calculators | SWP                                              |

## Build and Run Locally

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/mithunshanbhag/finskew.git
   cd finskew
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Build the project:

   ```bash
   dotnet build
   ```

4. Run the application:

   ```powershell
   dotnet run --project .\src\FinSkew.Ui\FinSkew.Ui.csproj
   ```

   Or use the convenience script:

   ```bash
   .\run-local.ps1
   ```

5. Open your browser and navigate to the URL displayed in the console (typically `http://localhost:5000`), where the root route (`/`) loads the landing page

6. Optionally, run the tests:

   ```bash
   dotnet test .\tests\FinSkew.Ui.UnitTests\FinSkew.Ui.UnitTests.csproj
   dotnet test .\tests\FinSkew.Ui.E2ETests\FinSkew.Ui.E2ETests.csproj
   dotnet test .\FinSkew.slnx
   ```

   Or use the convenience script:

   ```bash
   .\run-local.ps1 unit-tests
   .\run-local.ps1 e2e-tests
   .\run-local.ps1 tests
   ```

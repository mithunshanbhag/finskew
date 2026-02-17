# AGENTS.md

This is my opinionated checklist for building indie-SaaS, micro-SaaS apps. It is not exhaustive, but it covers some important aspects of my app building process. This is a living document and will be updated / tweaked as required.

## PREFERRED TECH STACK

My preferred framework for building apps is .NET (currently .NET 10 is the latest version):

- Frontend: Blazor WebAssembly (with MudBlazor controls).
- Backend: Azure Function Apps.
- Database: Azure Cosmos DB (NOSQL API, formerly known as Core SQL API).

I prefer to host my apps and related infra on Azure using serverless/PaaS. This is to keep things relatively simple, low maintenance and low cost.

## GROUND RULES

| Key files & folders     | Purpose                                                           |
| ----------------------- | ----------------------------------------------------------------- |
| `/.github/LEARNINGS.md` | All learnings & notes will be documented here by the agent (you). |
| `/docs/ui-mockups`      | All UI mockups will live under this folder.                       |
| `/README.md`            | The main documentation file for the project.                      |
| `/run-local.ps1`        | A convenience PowerShell script to run the app locally.           |
| `/src`                  | All source code will live under this folder.                      |
| `/tests`                | All unit and E2E tests will live under this folder.               |

## GENERAL WORKFLOW

1. See if there are any prior learnings documented in `/.github/LEARNINGS.md` that can be helpful for the current task.

2. Start with the specifications in the `/docs/specs` folder. This will give you a clear understanding of the requirements and features of the app.
   - Start with `/docs/specs/README.md`.

3. If explicitly asked, only then create (or update) UI mockups in the `/docs/ui-mockups` folder based on the specifications.
   - Always use any existing UI mock ups in the `/docs/ui-mockups` folder as general reference. This will help you visualize the app and its user interface.

4. Implement the features in the `/src` folder based on the specifications and UI mockups.

5. Write tests in the `/tests` folder to verify that the features work as expected.

6. Update the documentation in the `README.md` file as needed to reflect the current state of the project.

7. Document any new, relevant learnings and notes by updating the `/.github/LEARNINGS.md` file.

## UI MOCKUP GUIDELINES

- Create UI mockups only using plain HTML, CSS, and if needed, a little JavaScript. Each UI mockup should include:
  - an `index.html` file that contains the HTML structure.
  - CSS styles should either be inline (in index.html) or in a separate `styles.css` file.
  - If needed, a separate `script.js` file should contain any necessary JavaScript.

- Each UI mockup should be in its own subfolder under `/docs/ui-mockups` with a descriptive name. For example:

  ```text
  /docs/ui-mockups
    /UserRegistrationForm
    /DashboardOverview
    /ProductDetailsPage
  ```

- The file `/docs/specs/ui.md` will be particularly useful for authoring UI mockups since it contains detailed specifications about the UI.

- The idea is to create simple, static mockups that can be easily viewed in a web browser. These mockups should focus on the layout and design of the UI components rather than complex functionality.

- Using F12 developer tools in the browser, one should be able to inspect various aspects of the UI elements/controls: their dimensions, colors, fonts, box model, other CSS properties.

- MudBlazor controls should be used as a reference for the design and layout of the UI components, but the mockups should be implemented using plain HTML and CSS.

## DEVELOPMENT GUIDELINES

- Each source project will be in its own subfolder under `/src` with a descriptive name. For example:

  ```text
  /src
    /MyApp.Api
    /MyApp.Application
    /MyApp.Domain
    /MyApp.Infrastructure
  ```

- Ensure that the code is clean, well-structured, and follows best practices for the programming language and framework being used.

- If you encounter any ambiguities or have questions about the specifications, please ask for clarification before proceeding with the implementation.

- Do not declare success until you've actually verified that the changes work. Verification can be done by:
  - Running the application and testing the feature visually. OR
  - Writing and running existing automated tests to ensure the feature works as expected.
  - If there are no existing automated tests, you can consider writing new tests to verify the feature. These tests should be added to the appropriate test project under the `/tests` folder.

## TESTING GUIDELINES

- There will be two types of tests: Unit Tests and End-to-End Tests.

- Each test project (in the `/tests` folder) will mirror a source project (in the `/src` folder). See examples below.

  ```text
  /src
    /MyApp.Api
    /MyApp.Application
    /MyApp.Domain
    /MyApp.Infrastructure

  /tests
    /MyApp.Api.IntegrationTests
    /MyApp.Application.UnitTests
    /MyApp.Domain.UnitTests
    /MyApp.Infrastructure.IntegrationTests
    /MyApp.Infrastructure.UnitTests
  ```

- Always ensure that the tests are building, running, and passing before declaring success.

### Unit Tests

- For .NET source projects, you should ideally author unit tests using XUnit, Moq, FluentAssertions and Bogus.
  - For FluentAssertion, please use the latest, stable `7.2.x` version. Do not attempt to use the `8.x` or later versions.
  - Some references for writing good unit tests in .NET:
    - [Unit testing best practices for .NET](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

### End-to-End Tests

- Please author all end-to-end tests using Playwright (with .NET SDK) and XUnit (using the `Microsoft.Playwright.XUnit` nuget package).
- For details on getting started with Playwright using XUnit: [Playwright .NET SDK](https://playwright.dev/dotnet/docs/intro).
- Playwright best practices are [documented here](https://playwright.dev/docs/best-practices).
- For consistency, please use the same testing libraries and frameworks as mentioned in the Unit Tests section.
  - But, if possible, use Playwright's own assertion library for end-to-end tests over FluentAssertions.
- It is preferable to run Playwright in Headless mode, especially since these tests will be running in CI/CD pipelines too.
- The file `/docs/specs/ui.md` will be particularly useful for authoring end-to-end tests since it contains detailed specifications about the UI.

## DOCUMENTATION GUIDELINES

- The README.md file should always be up-to-date with the following sections:
  - A title with the project name.
  - Status badges (e.g., build status, test coverage) at the top. You can use placeholders if none exist.
  - A screenshot or GIF demonstrating the project in action. You can use a placeholder if none exist.
  - An installation instructions section with clear steps on how to install the project.
  - A usage instructions section that explains how to use the project.
  - A section on how to build and run the project locally, with clear instructions.
  - A section on how to run the tests, with clear instructions.

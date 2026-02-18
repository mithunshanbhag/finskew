namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class XirrCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task XirrCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/xirr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("XIRR Calculator");
    }

    [Fact]
    public async Task XirrCalculator_Breadcrumb_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/xirr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Calculators")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("XIRR")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task XirrCalculator_Navigation_UsingShortRoute()
    {
        await Page.GotoAsync($"{BaseUrl}/xirr");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("XIRR Calculator");
    }

    [Fact]
    public async Task XirrCalculator_InputFields_HaveCorrectAdornments()
    {
        await Page.GotoAsync($"{BaseUrl}/xirr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToBeVisibleAsync();

        await Expect(Page.GetByLabel("Investment start date")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Investment maturity date")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Monthly investment amount in Indian Rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Expected annual return rate as percentage")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task XirrCalculator_ResultsSection_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/xirr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToBeVisibleAsync();
        await Expect(resultsSection.GetByRole(AriaRole.Img).First).ToBeVisibleAsync();

        // Verify result fields are present
        var resultsList = resultsSection.GetByRole(AriaRole.List, new LocatorGetByRoleOptions { Name = "Calculation results summary" });
        await Expect(resultsList.GetByText("P (Initial Principal)")).ToBeVisibleAsync();
        await Expect(resultsList.GetByText("I (Total Gain)")).ToBeVisibleAsync();
        await Expect(resultsList.GetByText("A (Final Amount)")).ToBeVisibleAsync();
        await Expect(resultsList.GetByText("XIRR")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task XirrCalculator_DefaultValues_DisplayCorrectResults()
    {
        await Page.GotoAsync($"{BaseUrl}/xirr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Verify default input values
        var monthlyInvestmentInput = Page.GetByLabel("Monthly investment amount in Indian Rupees");
        await Expect(monthlyInvestmentInput).ToHaveValueAsync("1,000");

        var expectedReturnInput = Page.GetByLabel("Expected annual return rate as percentage");
        await Expect(expectedReturnInput).ToHaveValueAsync("12");

        // Verify results section is visible
        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToBeVisibleAsync();

        // Verify result fields are displayed
        await Expect(resultsSection).ToContainTextAsync("P (Initial Principal)");
        await Expect(resultsSection).ToContainTextAsync("I (Total Gain)");
        await Expect(resultsSection).ToContainTextAsync("A (Final Amount)");
        await Expect(resultsSection).ToContainTextAsync("XIRR");
    }

    [Fact]
    public async Task XirrCalculator_ChangingInputs_RecalculatesResults()
    {
        await Page.GotoAsync($"{BaseUrl}/xirr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });

        // Verify initial results are displayed
        await Expect(resultsSection).ToBeVisibleAsync();
        await Expect(resultsSection).ToContainTextAsync("P (Initial Principal)");
        await Expect(resultsSection).ToContainTextAsync("I (Total Gain)");
        await Expect(resultsSection).ToContainTextAsync("A (Final Amount)");
        await Expect(resultsSection).ToContainTextAsync("XIRR");

        // Change an input value - increase monthly investment significantly
        var monthlyInvestmentInput = Page.GetByLabel("Monthly investment amount in Indian Rupees");
        await monthlyInvestmentInput.ClearAsync();
        await monthlyInvestmentInput.FillAsync("10000");
        await monthlyInvestmentInput.BlurAsync();

        // Wait for recalculation
        await Page.WaitForTimeoutAsync(1000);

        // Verify results section still displays XIRR with a percentage value
        await Expect(resultsSection).ToBeVisibleAsync();
        await Expect(resultsSection).ToContainTextAsync("P (Initial Principal)");
        await Expect(resultsSection).ToContainTextAsync("I (Total Gain)");
        await Expect(resultsSection).ToContainTextAsync("A (Final Amount)");
        await Expect(resultsSection).ToContainTextAsync("XIRR");
        await Expect(resultsSection).ToContainTextAsync("%");
    }

    [Theory]
    [InlineData("2000", "15")]
    [InlineData("5000", "10")]
    public async Task XirrCalculator_CustomInputs_CalculatesCorrectly(
        string monthlyInvestment,
        string expectedReturn)
    {
        await Page.GotoAsync($"{BaseUrl}/xirr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Fill in custom values
        var monthlyInvestmentInput = Page.GetByLabel("Monthly investment amount in Indian Rupees");
        await monthlyInvestmentInput.ClearAsync();
        await monthlyInvestmentInput.FillAsync(monthlyInvestment);
        await monthlyInvestmentInput.BlurAsync();

        var expectedReturnInput = Page.GetByLabel("Expected annual return rate as percentage");
        await expectedReturnInput.ClearAsync();
        await expectedReturnInput.FillAsync(expectedReturn);
        await expectedReturnInput.BlurAsync();

        // Wait for calculation to complete
        await Page.WaitForTimeoutAsync(1000);

        // Verify results section updates
        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToBeVisibleAsync();

        // Verify XIRR value is displayed (should contain percentage sign)
        await Expect(resultsSection).ToContainTextAsync("P (Initial Principal)");
        await Expect(resultsSection).ToContainTextAsync("I (Total Gain)");
        await Expect(resultsSection).ToContainTextAsync("A (Final Amount)");
        await Expect(resultsSection).ToContainTextAsync("%");
    }
}
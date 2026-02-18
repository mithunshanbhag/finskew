namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class CagrCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task CagrCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/cagr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("CAGR Calculator");
    }

    [Fact]
    public async Task CagrCalculator_Breadcrumb_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/cagr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Calculators")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("CAGR")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task CagrCalculator_Navigation_UsingShortRoute()
    {
        await Page.GotoAsync($"{BaseUrl}/cagr");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("CAGR Calculator");
    }

    [Fact]
    public async Task CagrCalculator_InputFields_HaveCorrectAdornments()
    {
        await Page.GotoAsync($"{BaseUrl}/cagr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToBeVisibleAsync();

        await Expect(Page.GetByLabel("Initial principal amount in Indian Rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Final amount in Indian Rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Investment time period in years")).ToBeVisibleAsync();
    }

    [Theory]
    [InlineData("100000", "150000", "5", "50,000", "8.45%")]
    [InlineData("500000", "1000000", "10", "5,00,000", "7.18%")]
    [InlineData("1000000", "2000000", "8", "10,00,000", "9.05%")]
    public async Task CagrCalculator_CustomInputs_CalculatesCorrectly(
        string initialPrincipal,
        string finalAmount,
        string years,
        string expectedTotalGain,
        string expectedCagr)
    {
        await Page.GotoAsync($"{BaseUrl}/cagr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Fill in custom values
        var initialPrincipalInput = Page.GetByLabel("Initial principal amount in Indian Rupees");
        await initialPrincipalInput.ClearAsync();
        await initialPrincipalInput.FillAsync(initialPrincipal);
        await initialPrincipalInput.BlurAsync();

        var finalAmountInput = Page.GetByLabel("Final amount in Indian Rupees");
        await finalAmountInput.ClearAsync();
        await finalAmountInput.FillAsync(finalAmount);
        await finalAmountInput.BlurAsync();

        var timePeriodInput = Page.GetByLabel("Investment time period in years");
        await timePeriodInput.ClearAsync();
        await timePeriodInput.FillAsync(years);
        await timePeriodInput.BlurAsync();

        // Wait for calculation to complete
        await Page.WaitForTimeoutAsync(1000);

        // Verify results
        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync(expectedTotalGain);
        await Expect(resultsSection).ToContainTextAsync(expectedCagr);
    }

    [Fact]
    public async Task CagrCalculator_Chart_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/cagr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var chart = Page.GetByRole(AriaRole.Img).First;
        await Expect(chart).ToBeVisibleAsync();
    }

    [Fact]
    public async Task CagrCalculator_ResultsSection_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/cagr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToBeVisibleAsync();

        // Verify CAGR result is present
        var resultsList = resultsSection.GetByRole(AriaRole.List, new LocatorGetByRoleOptions { Name = "Calculation results summary" });
        await Expect(resultsList.GetByText("P (Initial Principal)")).ToBeVisibleAsync();
        await Expect(resultsList.GetByText("I (Total Gain)")).ToBeVisibleAsync();
        await Expect(resultsList.GetByText("A (Final Amount)")).ToBeVisibleAsync();
        await Expect(resultsList.GetByText("R (CAGR)")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task CagrCalculator_DefaultValues_DisplayCorrectResults()
    {
        await Page.GotoAsync($"{BaseUrl}/cagr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Verify default input values
        var initialPrincipalInput = Page.GetByLabel("Initial principal amount in Indian Rupees");
        await Expect(initialPrincipalInput).ToHaveValueAsync("10,000");

        var finalAmountInput = Page.GetByLabel("Final amount in Indian Rupees");
        await Expect(finalAmountInput).ToHaveValueAsync("12,000");

        var timePeriodInput = Page.GetByLabel("Investment time period in years");
        await Expect(timePeriodInput).ToHaveValueAsync("3");

        // Verify results section is visible
        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToBeVisibleAsync();

        await Expect(resultsSection).ToContainTextAsync("10,000");
        await Expect(resultsSection).ToContainTextAsync("2,000");
        await Expect(resultsSection).ToContainTextAsync("12,000");
        await Expect(resultsSection).ToContainTextAsync("6.27%");
    }

    [Fact]
    public async Task CagrCalculator_ChangingInputs_RecalculatesResults()
    {
        await Page.GotoAsync($"{BaseUrl}/cagr-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });

        // Get initial result value
        var initialResultText = await resultsSection.TextContentAsync();

        // Change an input value
        var initialPrincipalInput = Page.GetByLabel("Initial principal amount in Indian Rupees");
        await initialPrincipalInput.ClearAsync();
        await initialPrincipalInput.FillAsync("500000");
        await initialPrincipalInput.BlurAsync();

        // Wait for recalculation
        await Page.WaitForTimeoutAsync(1000);

        // Verify result has changed
        var updatedResultText = await resultsSection.TextContentAsync();
        Assert.NotEqual(initialResultText, updatedResultText);
    }
}

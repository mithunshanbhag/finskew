namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class SwpCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task SwpCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/swp-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("SWP Calculator");
    }

    [Fact]
    public async Task SwpCalculator_DefaultValues_DisplayCorrectResults()
    {
        await Page.GotoAsync($"{BaseUrl}/swp-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Verify default input values
        var totalInvestmentInput = Page.GetByLabel("Invested amount in Indian Rupees");
        await Expect(totalInvestmentInput).ToHaveValueAsync("5,00,000");

        var monthlyWithdrawalInput = Page.GetByLabel("Monthly withdrawal amount in Indian Rupees");
        await Expect(monthlyWithdrawalInput).ToHaveValueAsync("10,000");

        var annualReturnInput = Page.GetByLabel("Expected annual return rate as percentage");
        await Expect(annualReturnInput).ToHaveValueAsync("8");

        var timePeriodInput = Page.GetByLabel("Time period in years");
        await Expect(timePeriodInput).ToHaveValueAsync("5");

        // Verify results section is visible
        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToBeVisibleAsync();

        // Verify default result values are displayed
        // Default: P=500000, W=10000, R=8, N=5
        // Total Withdrawal = W * 12 * N = 10000 * 12 * 5 = 600000
        await Expect(resultsSection).ToContainTextAsync("Invested Amount");
        await Expect(resultsSection).ToContainTextAsync("Total Withdrawal");
        await Expect(resultsSection).ToContainTextAsync("Final Amount");

        var growthSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Growth over time" });
        await Expect(growthSection).ToBeVisibleAsync();
        var growthTable = Page.GetByLabel("Table showing yearly growth of total investment");
        await Expect(growthTable).ToBeVisibleAsync();
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(6);
        await Expect(Page.GetByLabel("Total investment at the end of year 1: 416170 rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Total investment at the end of year 5: 5256 rupees")).ToBeVisibleAsync();
    }

    [Theory]
    [InlineData("500000", "5000", "10", "5", "5,00,000", "3,00,000", "432243", "5")]
    [InlineData("1000000", "10000", "8", "3", "10,00,000", "3,60,000", "862179", "3")]
    [InlineData("100000", "5000", "8", "5", "1,00,000", "3,00,000", "-220849", "5")]
    public async Task SwpCalculator_CustomInputs_CalculatesCorrectly(
        string investedAmount,
        string monthlyWithdrawal,
        string annualReturn,
        string years,
        string expectedInvestedAmount,
        string expectedTotalWithdrawn,
        string expectedFinalYearInvestment,
        string expectedYearCount)
    {
        await Page.GotoAsync($"{BaseUrl}/swp-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Fill in custom values
        var totalInvestmentInput = Page.GetByLabel("Invested amount in Indian Rupees");
        await totalInvestmentInput.ClearAsync();
        await totalInvestmentInput.FillAsync(investedAmount);
        await totalInvestmentInput.BlurAsync();

        var monthlyWithdrawalInput = Page.GetByLabel("Monthly withdrawal amount in Indian Rupees");
        await monthlyWithdrawalInput.ClearAsync();
        await monthlyWithdrawalInput.FillAsync(monthlyWithdrawal);
        await monthlyWithdrawalInput.BlurAsync();

        var annualReturnInput = Page.GetByLabel("Expected annual return rate as percentage");
        await annualReturnInput.ClearAsync();
        await annualReturnInput.FillAsync(annualReturn);
        await annualReturnInput.BlurAsync();

        var timePeriodInput = Page.GetByLabel("Time period in years");
        await timePeriodInput.ClearAsync();
        await timePeriodInput.FillAsync(years);
        await timePeriodInput.BlurAsync();

        // Wait for calculation to complete
        await Page.WaitForTimeoutAsync(1000);

        // Verify results
        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync(expectedInvestedAmount);
        await Expect(resultsSection).ToContainTextAsync(expectedTotalWithdrawn);

        var growthTable = Page.GetByLabel("Table showing yearly growth of total investment");
        await Expect(growthTable).ToBeVisibleAsync();
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(int.Parse(expectedYearCount) + 1);
        await Expect(Page.GetByLabel($"Total investment at the end of year {years}: {expectedFinalYearInvestment} rupees"))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task SwpCalculator_Chart_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/swp-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var chart = Page.GetByRole(AriaRole.Img).First;
        await Expect(chart).ToBeVisibleAsync();
    }

    [Fact]
    public async Task SwpCalculator_Breadcrumb_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/swp-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Calculators")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("SWP")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task SwpCalculator_Navigation_UsingShortRoute()
    {
        await Page.GotoAsync($"{BaseUrl}/swp");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("SWP Calculator");
    }

    [Fact]
    public async Task SwpCalculator_InputFields_HaveCorrectAdornments()
    {
        await Page.GotoAsync($"{BaseUrl}/swp-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToBeVisibleAsync();

        await Expect(Page.GetByLabel("Invested amount in Indian Rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Monthly withdrawal amount in Indian Rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Expected annual return rate as percentage")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Time period in years")).ToBeVisibleAsync();
    }

    [Theory]
    [InlineData("5000", "10,000")]
    [InlineData("200000000", "10,00,00,000")]
    public async Task SwpCalculator_InvalidInvestedAmount_ShowsValidation(string invalidAmount, string expectedAmount)
    {
        await Page.GotoAsync($"{BaseUrl}/swp-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var totalInvestmentInput = Page.GetByLabel("Invested amount in Indian Rupees");
        await totalInvestmentInput.FillAsync(invalidAmount);
        await totalInvestmentInput.BlurAsync();

        await Page.WaitForTimeoutAsync(500);

        await Expect(totalInvestmentInput).ToHaveValueAsync(expectedAmount);

        var helperText = Page.Locator("text=Enter amount between");
        await Expect(helperText).ToBeVisibleAsync();
    }

    [Theory]
    [InlineData("100", "500")]
    [InlineData("20000000", "1,00,00,000")]
    public async Task SwpCalculator_InvalidMonthlyWithdrawal_ShowsValidation(string invalidAmount, string expectedAmount)
    {
        await Page.GotoAsync($"{BaseUrl}/swp-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var monthlyWithdrawalInput = Page.GetByLabel("Monthly withdrawal amount in Indian Rupees");
        await monthlyWithdrawalInput.FillAsync(invalidAmount);
        await monthlyWithdrawalInput.BlurAsync();

        await Page.WaitForTimeoutAsync(500);

        await Expect(monthlyWithdrawalInput).ToHaveValueAsync(expectedAmount);

        var helperText = Page.Locator("text=Enter withdrawal between");
        await Expect(helperText).ToBeVisibleAsync();
    }

    [Fact]
    public async Task SwpCalculator_ChangingInputs_RecalculatesResults()
    {
        await Page.GotoAsync($"{BaseUrl}/swp-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });

        // Get initial result value
        var initialResultText = await resultsSection.TextContentAsync();

        // Change an input value
        var totalInvestmentInput = Page.GetByLabel("Invested amount in Indian Rupees");
        await totalInvestmentInput.ClearAsync();
        await totalInvestmentInput.FillAsync("300000");
        await totalInvestmentInput.BlurAsync();

        // Wait for recalculation
        await Page.WaitForTimeoutAsync(1000);

        // Verify result has changed
        var updatedResultText = await resultsSection.TextContentAsync();
        Assert.NotEqual(initialResultText, updatedResultText);

        // Verify new invested amount is displayed
        await Expect(resultsSection).ToContainTextAsync("3,00,000");
    }

    [Fact]
    public async Task SwpCalculator_ResultsSection_DisplaysAllFields()
    {
        await Page.GotoAsync($"{BaseUrl}/swp-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });

        // Verify all result fields are present using more specific locators
        var resultsList = resultsSection.GetByRole(AriaRole.List, new LocatorGetByRoleOptions { Name = "Calculation results summary" });
        await Expect(resultsList.GetByText("Invested Amount")).ToBeVisibleAsync();
        await Expect(resultsList.GetByText("Total Withdrawal")).ToBeVisibleAsync();
        await Expect(resultsList.GetByText("Final Amount")).ToBeVisibleAsync();
    }
}
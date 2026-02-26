namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class MisCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task MisCalculator_PageLoads_DefaultValuesAndLabelsAreDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/mis-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Post Office MIS Calculator");

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToContainTextAsync("Invested Amount");
        await Expect(inputSection).ToContainTextAsync("Annual Interest Rate");
        await Expect(inputSection).ToContainTextAsync("Time Period (Years)");

        await Expect(Page.GetByLabel("Invested amount in Indian Rupees")).ToHaveValueAsync("1,00,000");
        await Expect(Page.GetByLabel("Annual interest rate as percentage")).ToHaveValueAsync("6.6");

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("Invested Amount");
        await Expect(resultsSection).ToContainTextAsync("Total Gain");
        await Expect(resultsSection).ToContainTextAsync("Monthly Income");
        await Expect(resultsSection).ToContainTextAsync("Final Amount");
    }

    [Fact]
    public async Task MisCalculator_TimePeriod_IsReadOnlyDisabledAndDefaultsToFiveYears()
    {
        await Page.GotoAsync($"{BaseUrl}/mis-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var timePeriodInput = Page.GetByLabel("Post Office MIS time period in years (read only)");
        await Expect(timePeriodInput).ToHaveValueAsync("5");
        await Expect(timePeriodInput).ToBeDisabledAsync();
    }

    [Fact]
    public async Task MisCalculator_DefaultValues_DisplayCorrectCalculatedOutputs()
    {
        await Page.GotoAsync($"{BaseUrl}/mis-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("1,00,000");
        await Expect(resultsSection).ToContainTextAsync("38,722");
        await Expect(resultsSection).ToContainTextAsync("645");
        await Expect(resultsSection).ToContainTextAsync("1,38,722");
    }

    [Fact]
    public async Task MisCalculator_CustomInputs_RecalculatesAndDisplaysUpdatedValues()
    {
        await Page.GotoAsync($"{BaseUrl}/mis-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var investedAmountInput = Page.GetByLabel("Invested amount in Indian Rupees");
        await investedAmountInput.ClearAsync();
        await investedAmountInput.FillAsync("200000");
        await investedAmountInput.BlurAsync();

        var annualInterestRateInput = Page.GetByLabel("Annual interest rate as percentage");
        await annualInterestRateInput.ClearAsync();
        await annualInterestRateInput.FillAsync("8");
        await annualInterestRateInput.BlurAsync();

        await Page.WaitForTimeoutAsync(1000);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("2,00,000");
        await Expect(resultsSection).ToContainTextAsync("97,189");
        await Expect(resultsSection).ToContainTextAsync("1,619");
        await Expect(resultsSection).ToContainTextAsync("2,97,189");
    }
}

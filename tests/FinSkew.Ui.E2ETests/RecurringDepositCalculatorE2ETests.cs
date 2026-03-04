namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class RecurringDepositCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task RecurringDepositCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/recurring-deposit-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Recurring Deposit Calculator");
    }

    [Fact]
    public async Task RecurringDepositCalculator_Labels_MatchUpdatedSpec()
    {
        await Page.GotoAsync($"{BaseUrl}/recurring-deposit-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToContainTextAsync("Monthly Deposit Amount");
        await Expect(inputSection).ToContainTextAsync("Expected Annual Interest Rate");
        await Expect(inputSection).ToContainTextAsync("Time Period (Years)");

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("Invested Amount");
        await Expect(resultsSection).ToContainTextAsync("Total Gain");
        await Expect(resultsSection).ToContainTextAsync("Final Amount");
    }

    [Fact]
    public async Task RecurringDepositCalculator_CustomInputs_DisplaysCorrectResults()
    {
        await Page.GotoAsync($"{BaseUrl}/recurring-deposit-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var monthlyDepositInput = Page.GetByLabel("Monthly deposit amount in Indian Rupees");
        await monthlyDepositInput.ClearAsync();
        await monthlyDepositInput.FillAsync("1000");
        await monthlyDepositInput.BlurAsync();

        var expectedAnnualInterestRateInput = Page.GetByLabel("Expected annual interest rate as percentage");
        await expectedAnnualInterestRateInput.ClearAsync();
        await expectedAnnualInterestRateInput.FillAsync("12");
        await expectedAnnualInterestRateInput.BlurAsync();

        var timePeriodInput = Page.GetByLabel("Investment time period in years");
        await timePeriodInput.ClearAsync();
        await timePeriodInput.FillAsync("1");
        await timePeriodInput.BlurAsync();

        await Page.WaitForTimeoutAsync(1000);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("12,000");
        await Expect(resultsSection).ToContainTextAsync("682");
        await Expect(resultsSection).ToContainTextAsync("12,682");
    }
}
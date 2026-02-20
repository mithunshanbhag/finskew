namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class StepUpSipCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task StepUpSipCalculator_PageLoads_DefaultValuesAreDisplayed()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Navigate to Step-Up SIP Calculator" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Step-Up SIP Calculator");

        await Expect(Page.GetByLabel("Monthly investment amount in Indian Rupees")).ToHaveValueAsync("1,000");
        await Expect(Page.GetByLabel("Annual step-up percentage for investment increase")).ToHaveValueAsync("5");
        await Expect(Page.GetByLabel("Expected annual return rate as percentage")).ToHaveValueAsync("12");
        await Expect(Page.GetByLabel("Investment time period in years")).ToHaveValueAsync("5");

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("66,307");
        await Expect(resultsSection).ToContainTextAsync("23,798");
        await Expect(resultsSection).ToContainTextAsync("90,105");

        var growthSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Growth over time" });
        await Expect(growthSection).ToBeVisibleAsync();

        var growthTable = Page.GetByLabel("Table showing yearly growth of investment");
        await Expect(growthTable).ToBeVisibleAsync();
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(6);
        await Expect(Page.GetByLabel("Final amount at the end of year 1: 12809 rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Final amount at the end of year 5: 90105 rupees")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task StepUpSipCalculator_Labels_MatchUpdatedSpec()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Navigate to Step-Up SIP Calculator" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToContainTextAsync("Monthly Investment Amount");
        await Expect(inputSection).ToContainTextAsync("Annual Step-Up Percentage");
        await Expect(inputSection).ToContainTextAsync("Expected Annual Return Rate");
        await Expect(inputSection).ToContainTextAsync("Time Period (Years)");

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("Invested Amount");
        await Expect(resultsSection).ToContainTextAsync("Total Gain");
        await Expect(resultsSection).ToContainTextAsync("Final Amount");
        await Expect(resultsSection).Not.ToContainTextAsync("Total Invested");
        await Expect(resultsSection).Not.ToContainTextAsync("Maturity Amount");

        var growthSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Growth over time" });
        await Expect(growthSection).ToBeVisibleAsync();
    }

    [Fact]
    public async Task StepUpSipCalculator_CustomInputs_DisplaysCorrectYearlyGrowthTable()
    {
        await Page.GotoAsync($"{BaseUrl}/step-up-sip-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var monthlyInvestmentInput = Page.GetByLabel("Monthly investment amount in Indian Rupees");
        await monthlyInvestmentInput.ClearAsync();
        await monthlyInvestmentInput.FillAsync("2000");
        await monthlyInvestmentInput.BlurAsync();

        var stepUpInput = Page.GetByLabel("Annual step-up percentage for investment increase");
        await stepUpInput.ClearAsync();
        await stepUpInput.FillAsync("10");
        await stepUpInput.BlurAsync();

        var expectedReturnInput = Page.GetByLabel("Expected annual return rate as percentage");
        await expectedReturnInput.ClearAsync();
        await expectedReturnInput.FillAsync("10");
        await expectedReturnInput.BlurAsync();

        var yearsInput = Page.GetByLabel("Investment time period in years");
        await yearsInput.ClearAsync();
        await yearsInput.FillAsync("10");
        await yearsInput.BlurAsync();

        await Page.WaitForTimeoutAsync(1000);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("3,82,498");
        await Expect(resultsSection).ToContainTextAsync("2,26,672");
        await Expect(resultsSection).ToContainTextAsync("6,09,170");

        var growthTable = Page.GetByLabel("Table showing yearly growth of investment");
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(11);
        await Expect(Page.GetByLabel("Final amount at the end of year 1: 25340 rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Final amount at the end of year 10: 609170 rupees")).ToBeVisibleAsync();
    }
}
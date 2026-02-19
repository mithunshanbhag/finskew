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
    }
}

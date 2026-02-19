namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class CompoundInterestCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task CompoundInterestCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/compound-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Compound Interest Calculator", new() { Timeout = 15000 });
    }

    [Fact]
    public async Task CompoundInterestCalculator_Breadcrumb_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/compound-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Calculators"))
            .ToBeVisibleAsync(new() { Timeout = 15000 });
        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Compound Interest"))
            .ToBeVisibleAsync(new() { Timeout = 15000 });
    }

    [Fact]
    public async Task CompoundInterestCalculator_Labels_MatchUpdatedSpec()
    {
        await Page.GotoAsync($"{BaseUrl}/compound-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToContainTextAsync("Invested Amount", new() { Timeout = 15000 });
        await Expect(inputSection).ToContainTextAsync("Annual Interest Rate", new() { Timeout = 15000 });
        await Expect(inputSection).ToContainTextAsync("Time Period (Years)", new() { Timeout = 15000 });
        await Expect(inputSection).ToContainTextAsync("Compounding Frequency", new() { Timeout = 15000 });

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("Invested Amount", new() { Timeout = 15000 });
        await Expect(resultsSection).ToContainTextAsync("Total Gain", new() { Timeout = 15000 });
        await Expect(resultsSection).ToContainTextAsync("Final Amount", new() { Timeout = 15000 });
        await Expect(resultsSection).Not.ToContainTextAsync("Maturity Amount", new() { Timeout = 15000 });
    }

    [Fact]
    public async Task CompoundInterestCalculator_Navigation_FromSimpleInterest()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Page.GotoAsync($"{BaseUrl}/cic");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Compound Interest Calculator", new() { Timeout = 15000 });
    }
}

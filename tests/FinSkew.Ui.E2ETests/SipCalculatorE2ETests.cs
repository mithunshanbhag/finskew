namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class SipCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task SipCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/sip-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("SIP Calculator", new() { Timeout = 15000 });
    }

    [Fact]
    public async Task SipCalculator_Labels_MatchUpdatedSpec()
    {
        await Page.GotoAsync($"{BaseUrl}/sip-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToContainTextAsync("Monthly Investment Amount", new() { Timeout = 15000 });
        await Expect(inputSection).ToContainTextAsync("Expected Annual Return Rate", new() { Timeout = 15000 });
        await Expect(inputSection).ToContainTextAsync("Time Period (Years)", new() { Timeout = 15000 });

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("Invested Amount", new() { Timeout = 15000 });
        await Expect(resultsSection).ToContainTextAsync("Total Gain", new() { Timeout = 15000 });
        await Expect(resultsSection).ToContainTextAsync("Final Amount", new() { Timeout = 15000 });
        await Expect(resultsSection).Not.ToContainTextAsync("Maturity Amount", new() { Timeout = 15000 });
        await Expect(resultsSection).Not.ToContainTextAsync("Total Invested", new() { Timeout = 15000 });
    }
}

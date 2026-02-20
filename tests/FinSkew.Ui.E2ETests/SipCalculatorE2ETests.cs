namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class SipCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task SipCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/sip-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("SIP Calculator", new PageAssertionsToHaveTitleOptions { Timeout = 15000 });
    }

    [Fact]
    public async Task SipCalculator_Labels_MatchUpdatedSpec()
    {
        await Page.GotoAsync($"{BaseUrl}/sip-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToContainTextAsync("Monthly Investment Amount", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(inputSection).ToContainTextAsync("Expected Annual Return Rate", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(inputSection).ToContainTextAsync("Time Period (Years)", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("Invested Amount", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(resultsSection).ToContainTextAsync("Total Gain", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(resultsSection).ToContainTextAsync("Final Amount", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(resultsSection).Not.ToContainTextAsync("Maturity Amount", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(resultsSection).Not.ToContainTextAsync("Total Invested", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });

        var growthSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Growth over time" });
        await Expect(growthSection).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });

        var growthTable = Page.GetByLabel("Table showing yearly growth of investment");
        await Expect(growthTable).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(6, new LocatorAssertionsToHaveCountOptions { Timeout = 15000 });
        await Expect(Page.GetByLabel("Final amount at the end of year 1: 12809 rupees"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
        await Expect(Page.GetByLabel("Final amount at the end of year 5: 82486 rupees"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
    }

    [Fact]
    public async Task SipCalculator_CustomInputs_DisplaysCorrectYearlyGrowthTable()
    {
        await Page.GotoAsync($"{BaseUrl}/sip-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var monthlyInvestmentInput = Page.GetByLabel("Monthly investment amount in Indian Rupees");
        await monthlyInvestmentInput.ClearAsync();
        await monthlyInvestmentInput.FillAsync("500");
        await monthlyInvestmentInput.BlurAsync();

        var returnRateInput = Page.GetByLabel("Expected annual return rate as percentage");
        await returnRateInput.ClearAsync();
        await returnRateInput.FillAsync("10");
        await returnRateInput.BlurAsync();

        var yearsInput = Page.GetByLabel("Investment time period in years");
        await yearsInput.ClearAsync();
        await yearsInput.FillAsync("5");
        await yearsInput.BlurAsync();

        await Page.WaitForTimeoutAsync(1000);

        var growthTable = Page.GetByLabel("Table showing yearly growth of investment");
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(6, new LocatorAssertionsToHaveCountOptions { Timeout = 15000 });
        await Expect(Page.GetByLabel("Final amount at the end of year 1: 6335 rupees"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
        await Expect(Page.GetByLabel("Final amount at the end of year 5: 39041 rupees"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
    }
}
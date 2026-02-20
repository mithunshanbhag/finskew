namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class CompoundInterestCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task CompoundInterestCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/compound-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Compound Interest Calculator", new PageAssertionsToHaveTitleOptions { Timeout = 15000 });
    }

    [Fact]
    public async Task CompoundInterestCalculator_Breadcrumb_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/compound-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Calculators"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Compound Interest"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
    }

    [Fact]
    public async Task CompoundInterestCalculator_Labels_MatchUpdatedSpec()
    {
        await Page.GotoAsync($"{BaseUrl}/compound-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToContainTextAsync("Invested Amount", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(inputSection).ToContainTextAsync("Annual Interest Rate", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(inputSection).ToContainTextAsync("Time Period (Years)", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(inputSection).ToContainTextAsync("Compounding Frequency", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("Invested Amount", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(resultsSection).ToContainTextAsync("Total Gain", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(resultsSection).ToContainTextAsync("Final Amount", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });
        await Expect(resultsSection).Not.ToContainTextAsync("Maturity Amount", new LocatorAssertionsToContainTextOptions { Timeout = 15000 });

        var growthSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Growth over time" });
        await Expect(growthSection).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });

        var growthTable = Page.GetByLabel("Table showing yearly growth of investment");
        await Expect(growthTable).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(4, new LocatorAssertionsToHaveCountOptions { Timeout = 15000 });
        await Expect(Page.GetByLabel("Final amount at the end of year 1: 10509 rupees"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
        await Expect(Page.GetByLabel("Final amount at the end of year 3: 11607 rupees"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
    }

    [Fact]
    public async Task CompoundInterestCalculator_Navigation_FromSimpleInterest()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Page.GotoAsync($"{BaseUrl}/cic");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Compound Interest Calculator", new PageAssertionsToHaveTitleOptions { Timeout = 15000 });
    }

    [Fact]
    public async Task CompoundInterestCalculator_CustomInputs_DisplaysCorrectYearlyGrowthTable()
    {
        await Page.GotoAsync($"{BaseUrl}/compound-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var principalInput = Page.GetByLabel("Invested amount in Indian Rupees");
        await principalInput.ClearAsync();
        await principalInput.FillAsync("50000");
        await principalInput.BlurAsync();

        var rateInput = Page.GetByLabel("Annual interest rate as percentage");
        await rateInput.ClearAsync();
        await rateInput.FillAsync("8");
        await rateInput.BlurAsync();

        var yearsInput = Page.GetByLabel("Time period in years");
        await yearsInput.ClearAsync();
        await yearsInput.FillAsync("5");
        await yearsInput.BlurAsync();

        await Page.WaitForTimeoutAsync(1000);

        var growthTable = Page.GetByLabel("Table showing yearly growth of investment");
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(6, new LocatorAssertionsToHaveCountOptions { Timeout = 15000 });
        await Expect(Page.GetByLabel("Final amount at the end of year 1: 54121 rupees"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
        await Expect(Page.GetByLabel("Final amount at the end of year 5: 74297 rupees"))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
    }
}
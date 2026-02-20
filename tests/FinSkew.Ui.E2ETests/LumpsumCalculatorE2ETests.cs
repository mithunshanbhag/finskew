namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class LumpsumCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task LumpsumCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/lumpsum-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Lumpsum Investment Calculator");
    }

    [Fact]
    public async Task LumpsumCalculator_DefaultValues_DisplayCorrectYearlyGrowthTable()
    {
        await Page.GotoAsync($"{BaseUrl}/lumpsum-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var principalInput = Page.GetByLabel("Lumpsum investment amount in Indian Rupees");
        await Expect(principalInput).ToHaveValueAsync("10,000");

        var rateInput = Page.GetByLabel("Expected annual return rate as percentage");
        await Expect(rateInput).ToHaveValueAsync("5");

        var yearsInput = Page.GetByLabel("Investment time period in years");
        await Expect(yearsInput).ToHaveValueAsync("3");

        var growthSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Growth over time" });
        await Expect(growthSection).ToBeVisibleAsync();

        var growthTable = Page.GetByLabel("Table showing yearly growth of investment");
        await Expect(growthTable).ToBeVisibleAsync();
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(4);
        await Expect(Page.GetByLabel("Final amount at the end of year 1: 10500 rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Final amount at the end of year 3: 11576 rupees")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task LumpsumCalculator_CustomInputs_DisplaysCorrectYearlyGrowthTable()
    {
        await Page.GotoAsync($"{BaseUrl}/lumpsum-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var principalInput = Page.GetByLabel("Lumpsum investment amount in Indian Rupees");
        await principalInput.ClearAsync();
        await principalInput.FillAsync("50000");
        await principalInput.BlurAsync();

        var rateInput = Page.GetByLabel("Expected annual return rate as percentage");
        await rateInput.ClearAsync();
        await rateInput.FillAsync("8");
        await rateInput.BlurAsync();

        var yearsInput = Page.GetByLabel("Investment time period in years");
        await yearsInput.ClearAsync();
        await yearsInput.FillAsync("5");
        await yearsInput.BlurAsync();

        await Page.WaitForTimeoutAsync(1000);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("50,000");
        await Expect(resultsSection).ToContainTextAsync("23,466");
        await Expect(resultsSection).ToContainTextAsync("73,466");

        var growthTable = Page.GetByLabel("Table showing yearly growth of investment");
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(6);
        await Expect(Page.GetByLabel("Final amount at the end of year 1: 54000 rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Final amount at the end of year 5: 73466 rupees")).ToBeVisibleAsync();
    }
}
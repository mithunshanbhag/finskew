namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class GratuityCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task GratuityCalculator_NavigationMenu_HasGratuityLink()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Gratuity", Exact = true })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task GratuityCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/gratuity-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Gratuity Calculator");
    }

    [Fact]
    public async Task GratuityCalculator_Navigation_UsingShortRoute()
    {
        await Page.GotoAsync($"{BaseUrl}/gratuity");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Gratuity Calculator");
    }

    [Fact]
    public async Task GratuityCalculator_CustomInputs_CalculatesCorrectly()
    {
        await Page.GotoAsync($"{BaseUrl}/gratuity-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var salaryInput = Page.GetByLabel("Monthly Salary (Basic + DA)");
        await salaryInput.ClearAsync();
        await salaryInput.FillAsync("50000");
        await salaryInput.BlurAsync();

        var yearsInput = Page.GetByLabel("Completed years of service");
        await yearsInput.ClearAsync();
        await yearsInput.FillAsync("5");
        await yearsInput.BlurAsync();

        await Page.WaitForTimeoutAsync(1000);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("30,00,000");
        await Expect(resultsSection).ToContainTextAsync("1,44,230");
    }

    [Fact]
    public async Task GratuityCalculator_LabelsAndChart_MatchUpdatedSpec()
    {
        await Page.GotoAsync($"{BaseUrl}/gratuity-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToContainTextAsync("Monthly Salary (Basic + DA)");

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        var resultsList = resultsSection.GetByRole(AriaRole.List, new LocatorGetByRoleOptions { Name = "Calculation results summary" });
        await Expect(resultsList.GetByText("Monthly Salary")).ToBeVisibleAsync();
        await Expect(resultsList.GetByText("Total Salary Drawn")).ToBeVisibleAsync();
        await Expect(resultsList.GetByText("Gratuity Amount")).ToBeVisibleAsync();

        var chart = resultsSection.GetByRole(AriaRole.Img).First;
        await Expect(chart).ToBeVisibleAsync();
        var chartDescription = await chart.GetAttributeAsync("aria-label");
        Assert.Contains("total salary drawn", chartDescription ?? string.Empty);
        Assert.Contains("gratuity amount", chartDescription ?? string.Empty);
    }
}
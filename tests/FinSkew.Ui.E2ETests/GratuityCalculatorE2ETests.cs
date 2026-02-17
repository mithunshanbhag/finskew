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

        var salaryInput = Page.GetByLabel("Salary basic and dearness allowance in Indian Rupees");
        await salaryInput.ClearAsync();
        await salaryInput.FillAsync("50000");
        await salaryInput.BlurAsync();

        var yearsInput = Page.GetByLabel("Completed years of service");
        await yearsInput.ClearAsync();
        await yearsInput.FillAsync("5");
        await yearsInput.BlurAsync();

        await Page.WaitForTimeoutAsync(1000);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("1,44,230");
    }
}

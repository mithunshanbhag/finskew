namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class CompoundInterestCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task CompoundInterestCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/compound-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Compound Interest Calculator");
    }

    [Fact]
    public async Task CompoundInterestCalculator_Breadcrumb_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/compound-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Calculators")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Compound Interest")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task CompoundInterestCalculator_Navigation_FromSimpleInterest()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Page.GotoAsync($"{BaseUrl}/cic");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Compound Interest Calculator");
    }
}
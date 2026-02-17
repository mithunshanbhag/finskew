namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class EmiCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task EmiCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/emi-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("EMI Calculator");
    }

    [Fact]
    public async Task EmiCalculator_Breadcrumb_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/emi-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Calculators")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("EMI")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task EmiCalculator_Navigation_UsingShortRoute()
    {
        await Page.GotoAsync($"{BaseUrl}/emi");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("EMI Calculator");
    }

    [Fact]
    public async Task EmiCalculator_InputFields_AreVisibleAndAccessible()
    {
        await Page.GotoAsync($"{BaseUrl}/emi-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToBeVisibleAsync();

        await Expect(Page.GetByLabel("Principal loan amount in Indian Rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Annual interest rate as percentage")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Loan tenure in years")).ToBeVisibleAsync();
    }

    [Theory]
    [InlineData("500000", "9", "15", "5,071", "9,12,839", "4,12,839")]
    public async Task EmiCalculator_CustomInputs_CalculatesCorrectly(
        string principal,
        string annualRate,
        string tenureYears,
        string expectedEmi,
        string expectedTotalPayment,
        string expectedTotalInterest)
    {
        await Page.GotoAsync($"{BaseUrl}/emi-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var principalInput = Page.GetByLabel("Principal loan amount in Indian Rupees");
        await principalInput.ClearAsync();
        await principalInput.FillAsync(principal);
        await principalInput.BlurAsync();

        var annualRateInput = Page.GetByLabel("Annual interest rate as percentage");
        await annualRateInput.ClearAsync();
        await annualRateInput.FillAsync(annualRate);
        await annualRateInput.BlurAsync();

        var tenureInput = Page.GetByLabel("Loan tenure in years");
        await tenureInput.ClearAsync();
        await tenureInput.FillAsync(tenureYears);
        await tenureInput.BlurAsync();

        await Page.WaitForTimeoutAsync(1000);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync(expectedEmi);
        await Expect(resultsSection).ToContainTextAsync(expectedTotalPayment);
        await Expect(resultsSection).ToContainTextAsync(expectedTotalInterest);
    }

    [Fact]
    public async Task EmiCalculator_ResultsSection_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/emi-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToBeVisibleAsync();
    }
}
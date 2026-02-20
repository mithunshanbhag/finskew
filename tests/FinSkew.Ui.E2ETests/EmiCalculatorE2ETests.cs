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

        await Expect(Page.GetByLabel("Loan amount in Indian Rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Annual interest rate as percentage")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Loan tenure in years")).ToBeVisibleAsync();
        await Expect(inputSection).ToContainTextAsync("Loan Amount");
        await Expect(inputSection).ToContainTextAsync("Annual Interest Rate");
        await Expect(inputSection).ToContainTextAsync("Loan Tenure (Years)");
    }

    [Theory]
    [InlineData("500000", "9", "15", "5,00,000", "5,071", "9,12,839", "4,12,839")]
    public async Task EmiCalculator_CustomInputs_CalculatesCorrectly(
        string loanAmount,
        string annualRate,
        string tenureYears,
        string expectedLoanAmount,
        string expectedEmi,
        string expectedTotalPayment,
        string expectedTotalInterest)
    {
        await Page.GotoAsync($"{BaseUrl}/emi-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var loanAmountInput = Page.GetByLabel("Loan amount in Indian Rupees");
        await loanAmountInput.ClearAsync();
        await loanAmountInput.FillAsync(loanAmount);
        await loanAmountInput.BlurAsync();

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
        await Expect(resultsSection).ToContainTextAsync(expectedLoanAmount);
        await Expect(resultsSection).ToContainTextAsync(expectedEmi);
        await Expect(resultsSection).ToContainTextAsync(expectedTotalPayment);
        await Expect(resultsSection).ToContainTextAsync(expectedTotalInterest);

        var growthTable = Page.GetByLabel("Table showing yearly amount paid towards loan");
        await Expect(growthTable).ToBeVisibleAsync();
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(int.Parse(tenureYears) + 1);
        await Expect(Page.GetByLabel($"Amount paid towards loan at the end of year {tenureYears}: {expectedTotalPayment.Replace(",", string.Empty)} rupees"))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task EmiCalculator_ResultsSection_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/emi-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToBeVisibleAsync();
        await Expect(resultsSection).ToContainTextAsync("Loan Amount");
        await Expect(resultsSection).ToContainTextAsync("Monthly EMI");
        await Expect(resultsSection).ToContainTextAsync("Total Amount");
        await Expect(resultsSection).ToContainTextAsync("Total Interest");

        var growthSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Growth over time" });
        await Expect(growthSection).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Table showing yearly amount paid towards loan")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Amount paid towards loan at the end of year 20: 208277 rupees")).ToBeVisibleAsync();
    }
}

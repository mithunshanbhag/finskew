namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class SimpleInterestCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task SimpleInterestCalculator_PageLoads_Successfully()
    {
        await Page.GotoAsync($"{BaseUrl}/simple-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Simple Interest Calculator");
    }

    [Fact]
    public async Task SimpleInterestCalculator_DefaultValues_DisplayCorrectResults()
    {
        await Page.GotoAsync($"{BaseUrl}/simple-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var principalInput = Page.GetByLabel("Invested amount in Indian Rupees");
        await Expect(principalInput).ToHaveValueAsync("10,000");

        var rateInput = Page.GetByLabel("Annual interest rate as percentage");
        await Expect(rateInput).ToHaveValueAsync("5");

        var yearsInput = Page.GetByLabel("Time period in years");
        await Expect(yearsInput).ToHaveValueAsync("3");

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToBeVisibleAsync();
        await Expect(resultsSection).ToContainTextAsync("Final Amount");

        var growthSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Growth over time" });
        await Expect(growthSection).ToBeVisibleAsync();

        var growthTable = Page.GetByLabel("Table showing yearly growth of investment");
        await Expect(growthTable).ToBeVisibleAsync();
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(4);
        await Expect(Page.GetByLabel("Final amount at the end of year 1: 10500 rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Final amount at the end of year 3: 11500 rupees")).ToBeVisibleAsync();
    }

    [Theory]
    [InlineData("50000", "8", "5", "50,000", "20,000", "70,000", "54000", "70000", 6)]
    [InlineData("100000", "10", "10", "1,00,000", "1,00,000", "2,00,000", "110000", "200000", 11)]
    public async Task SimpleInterestCalculator_CustomInputs_CalculatesCorrectly(
        string principal,
        string rate,
        string years,
        string expectedPrincipal,
        string expectedInterest,
        string expectedTotal,
        string expectedYearOneAmount,
        string expectedFinalYearAmount,
        int expectedRowCount)
    {
        await Page.GotoAsync($"{BaseUrl}/simple-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var principalInput = Page.GetByLabel("Invested amount in Indian Rupees");
        await principalInput.ClearAsync();
        await principalInput.FillAsync(principal);
        await principalInput.BlurAsync();

        var rateInput = Page.GetByLabel("Annual interest rate as percentage");
        await rateInput.ClearAsync();
        await rateInput.FillAsync(rate);
        await rateInput.BlurAsync();

        var yearsInput = Page.GetByLabel("Time period in years");
        await yearsInput.ClearAsync();
        await yearsInput.FillAsync(years);
        await yearsInput.BlurAsync();

        await Page.WaitForTimeoutAsync(1000);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync(expectedPrincipal);
        await Expect(resultsSection).ToContainTextAsync(expectedInterest);
        await Expect(resultsSection).ToContainTextAsync(expectedTotal);
        await Expect(resultsSection).ToContainTextAsync("Final Amount");

        var growthTable = Page.GetByLabel("Table showing yearly growth of investment");
        await Expect(growthTable).ToBeVisibleAsync();
        await Expect(growthTable.GetByRole(AriaRole.Row)).ToHaveCountAsync(expectedRowCount);
        await Expect(Page.GetByLabel($"Final amount at the end of year 1: {expectedYearOneAmount} rupees"))
            .ToBeVisibleAsync();
        await Expect(Page.GetByLabel($"Final amount at the end of year {years}: {expectedFinalYearAmount} rupees"))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task SimpleInterestCalculator_Chart_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/simple-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var chart = Page.GetByRole(AriaRole.Img).First;
        await Expect(chart).ToBeVisibleAsync();
    }

    [Fact]
    public async Task SimpleInterestCalculator_Breadcrumb_IsDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/simple-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Calculators")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Breadcrumb navigation").GetByText("Simple Interest")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task SimpleInterestCalculator_InputFields_HaveCorrectAdornments()
    {
        await Page.GotoAsync($"{BaseUrl}/simple-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToBeVisibleAsync();

        await Expect(Page.GetByLabel("Invested amount in Indian Rupees")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Annual interest rate as percentage")).ToBeVisibleAsync();
        await Expect(Page.GetByLabel("Time period in years")).ToBeVisibleAsync();
        await Expect(inputSection).ToContainTextAsync("Invested Amount");
        await Expect(inputSection).ToContainTextAsync("Annual Interest Rate");
        await Expect(inputSection).ToContainTextAsync("Time Period (Years)");
    }

    [Fact]
    public async Task SimpleInterestCalculator_Navigation_ToCompoundInterest()
    {
        await Page.GotoAsync($"{BaseUrl}/simple-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Page.GotoAsync($"{BaseUrl}/compound-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Compound Interest Calculator");
    }

    [Theory]
    [InlineData("5000")]
    [InlineData("200000000")]
    public async Task SimpleInterestCalculator_InvalidPrincipalAmount_ShowsValidation(string invalidAmount)
    {
        await Page.GotoAsync($"{BaseUrl}/simple-interest-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var principalInput = Page.GetByLabel("Invested amount in Indian Rupees");
        await principalInput.FillAsync(invalidAmount);
        await principalInput.BlurAsync();

        await Page.WaitForTimeoutAsync(500);

        var helperText = Page.Locator("text=Enter amount between");
        await Expect(helperText).ToBeVisibleAsync();
    }
}

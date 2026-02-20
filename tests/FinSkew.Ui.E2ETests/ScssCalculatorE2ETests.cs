using System.Text.RegularExpressions;

namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class ScssCalculatorE2ETests : PlaywrightTest
{
    [Fact]
    public async Task ScssCalculator_PageLoads_DefaultValuesAreDisplayed()
    {
        await Page.GotoAsync($"{BaseUrl}/scss-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("SCSS Calculator");

        var principalInput = Page.GetByLabel("Invested amount in Indian Rupees");
        await Expect(principalInput).ToHaveValueAsync("10,000");

        var annualInterestRateInput = Page.GetByLabel("SCSS annual interest rate (read only)");
        await Expect(annualInterestRateInput).ToHaveValueAsync("7.4");
        await Expect(annualInterestRateInput).ToBeDisabledAsync();
        await Expect(Page.GetByText("SCSS interest rate is fixed at 7.4%")).ToBeVisibleAsync();
        var annualInterestRateControl = annualInterestRateInput.Locator("xpath=ancestor::div[contains(@class,'mud-input-number-control')][1]");
        await Expect(annualInterestRateControl).ToHaveClassAsync(new Regex("mud-input-nospin"));

        var timePeriodInput = Page.GetByLabel("SCSS time period in years (read only)");
        await Expect(timePeriodInput).ToHaveValueAsync("5");
        await Expect(timePeriodInput).ToBeDisabledAsync();
        await Expect(Page.GetByText("SCSS has a fixed tenure of 5 years")).ToBeVisibleAsync();
        var timePeriodControl = timePeriodInput.Locator("xpath=ancestor::div[contains(@class,'mud-input-number-control')][1]");
        await Expect(timePeriodControl).ToHaveClassAsync(new Regex("mud-input-nospin"));

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync("Invested Amount");
        await Expect(resultsSection).ToContainTextAsync("Total Gain");
        await Expect(resultsSection).ToContainTextAsync("Final Amount");
        await Expect(resultsSection).ToContainTextAsync("10,000");
        await Expect(resultsSection).ToContainTextAsync("4,289");
        await Expect(resultsSection).ToContainTextAsync("14,289");
    }

    [Theory]
    [InlineData("500000", "5,00,000", "2,14,482", "7,14,482")]
    public async Task ScssCalculator_CustomInputs_CalculatesCorrectly(
        string principal,
        string expectedPrincipal,
        string expectedInterest,
        string expectedMaturityAmount)
    {
        await Page.GotoAsync($"{BaseUrl}/scss-calculator");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var principalInput = Page.GetByLabel("Invested amount in Indian Rupees");
        await principalInput.ClearAsync();
        await principalInput.FillAsync(principal);
        await principalInput.BlurAsync();
        await Expect(principalInput).ToHaveValueAsync(expectedPrincipal);

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToContainTextAsync(expectedPrincipal);
        await Expect(resultsSection).ToContainTextAsync(expectedInterest);
        await Expect(resultsSection).ToContainTextAsync(expectedMaturityAmount);
    }
}
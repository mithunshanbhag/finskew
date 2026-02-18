using System.Text.RegularExpressions;

namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class AppNavigationE2ETests : PlaywrightTest
{
    [Fact]
    public async Task App_LoadsHomePage_AtRootUrl()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("FinSkew: Financial Calculators");
        await Expect(Page.Locator(".landing-header-title")).ToBeVisibleAsync();
        await Expect(Page.GetByText("FinSkew: Your Financial Calculators")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Plan • Calculate • Grow")).ToHaveCountAsync(0);
        await Expect(Page.GetByText("Explore clear, practical calculators for loans, investments, and compounding growth.")).ToHaveCountAsync(0);
        await Expect(Page.Locator(".mud-container").First).ToHaveClassAsync(new Regex("mud-container-maxwidth-lg"));
        await Expect(Page.Locator(".mud-grid-item").First).ToHaveClassAsync(new Regex("mud-grid-item-md-4"));

        var firstCard = Page.Locator(".calculator-card").First;
        await Expect(firstCard).ToHaveAttributeAsync("class", new Regex(@"calculator-card.*rounded-lg|rounded-lg.*calculator-card"));
        await Expect(firstCard).ToHaveAttributeAsync("style", new Regex(@"background-color:\s*var\(--mud-palette-card-background\)", RegexOptions.IgnoreCase));

        var defaultBorder = await firstCard.EvaluateAsync<string>("el => window.getComputedStyle(el).borderColor");
        await firstCard.HoverAsync();
        await Page.WaitForTimeoutAsync(150);
        var hoverBorder = await firstCard.EvaluateAsync<string>("el => window.getComputedStyle(el).borderColor");
        Assert.NotEqual(defaultBorder, hoverBorder);
        Assert.DoesNotContain("224, 224, 224", hoverBorder);
    }

    [Theory]
    [InlineData("/simpleinterestcalculator", "Simple Interest Calculator")]
    [InlineData("/simple-interest-calculator", "Simple Interest Calculator")]
    [InlineData("/sic", "Simple Interest Calculator")]
    [InlineData("/compoundinterestcalculator", "Compound Interest Calculator")]
    [InlineData("/compound-interest-calculator", "Compound Interest Calculator")]
    [InlineData("/cic", "Compound Interest Calculator")]
    [InlineData("/cagrcalculator", "CAGR Calculator")]
    [InlineData("/cagr-calculator", "CAGR Calculator")]
    [InlineData("/cagrcalc", "CAGR Calculator")]
    [InlineData("/cagr-calc", "CAGR Calculator")]
    [InlineData("/cagr", "CAGR Calculator")]
    public async Task App_RouteAliases_WorkCorrectly(string route, string expectedTitle)
    {
        await Page.GotoAsync($"{BaseUrl}{route}");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync(expectedTitle);
    }

    [Fact]
    public async Task App_AppBar_IsVisible()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var appBar = Page.Locator("header, [role='banner']").First;
        await Expect(appBar).ToBeVisibleAsync();
    }

    [Fact]
    public async Task App_ResponsiveDesign_WorksOnMobile()
    {
        await Page.SetViewportSizeAsync(375, 667); // iPhone SE size

        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("FinSkew: Financial Calculators");
        await Expect(Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Navigate to Simple Interest Calculator" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task App_ResponsiveDesign_WorksOnTablet()
    {
        await Page.SetViewportSizeAsync(768, 1024); // iPad size

        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("FinSkew: Financial Calculators");
        await Expect(Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Navigate to Simple Interest Calculator" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task App_ResponsiveDesign_WorksOnDesktop()
    {
        await Page.SetViewportSizeAsync(1920, 1080); // Full HD

        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("FinSkew: Financial Calculators");
        await Expect(Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Navigate to Simple Interest Calculator" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Navigate to Compound Interest Calculator" })).ToBeVisibleAsync();
    }

    [Theory]
    [InlineData("Navigate to Simple Interest Calculator", "Simple Interest Calculator")]
    [InlineData("Navigate to Compound Interest Calculator", "Compound Interest Calculator")]
    public async Task App_LandingPageCalculatorCardLinks_NavigateToCalculatorPage(string linkName, string expectedTitle)
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = linkName }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync(expectedTitle);
    }

    [Fact(Skip = "@TODO: Need to investigate")]
    public async Task App_KeyboardNavigation_WorksCorrectly()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");

        var principalInput = Page.GetByRole(AriaRole.Spinbutton,
            new PageGetByRoleOptions { Name = "Principal amount in Indian Rupees" });
        await Expect(principalInput).ToBeVisibleAsync();
    }
}
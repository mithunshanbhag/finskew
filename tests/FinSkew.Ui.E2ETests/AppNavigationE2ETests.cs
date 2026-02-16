namespace FinSkew.Ui.E2ETests;

[Collection("E2E Tests")]
public class AppNavigationE2ETests : PlaywrightTest
{
    [Fact]
    public async Task App_LoadsHomePage_AtRootUrl()
    {
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Simple Interest Calculator");
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

        await Expect(Page).ToHaveTitleAsync("Simple Interest Calculator");

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToBeVisibleAsync();
    }

    [Fact]
    public async Task App_ResponsiveDesign_WorksOnTablet()
    {
        await Page.SetViewportSizeAsync(768, 1024); // iPad size

        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Simple Interest Calculator");

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToBeVisibleAsync();
    }

    [Fact]
    public async Task App_ResponsiveDesign_WorksOnDesktop()
    {
        await Page.SetViewportSizeAsync(1920, 1080); // Full HD

        await Page.GotoAsync(BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveTitleAsync("Simple Interest Calculator");

        var inputSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Input parameters" });
        await Expect(inputSection).ToBeVisibleAsync();

        var resultsSection = Page.GetByRole(AriaRole.Region, new PageGetByRoleOptions { Name = "Results" });
        await Expect(resultsSection).ToBeVisibleAsync();
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
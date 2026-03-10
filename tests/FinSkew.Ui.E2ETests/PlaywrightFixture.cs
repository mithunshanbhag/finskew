namespace FinSkew.Ui.E2ETests;

public class PlaywrightTest : IAsyncLifetime
{
    private IBrowser? _browser;
    private IPlaywright? _playwright;
    protected string BaseUrl => WebServerFixture.BaseUrl;
    protected IPage Page { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await WebServerFixture.EnsureServerIsRunningAsync();

        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        Page = await _browser.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await Page.CloseAsync();

        if (_browser != null) await _browser.DisposeAsync();

        _playwright?.Dispose();
    }
}
namespace FinSkew.Ui.E2ETests;

/// <summary>
///     Manages the lifecycle of the Blazor WASM app for E2E tests.
///     Starts the app before tests run and stops it after tests complete.
/// </summary>
public class WebServerFixture : IAsyncLifetime
{
    private const string BaseUrl = "http://localhost:5000";
    private const int MaxStartupWaitSeconds = 60;
    private readonly string _projectPath;
    private Process? _process;

    public WebServerFixture()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var solutionRoot = FindSolutionRoot(currentDir);
        _projectPath = Path.Combine(solutionRoot, "src", "FinSkew.Ui", "FinSkew.Ui.csproj");
    }

    public async Task InitializeAsync()
    {
        if (!File.Exists(_projectPath)) throw new InvalidOperationException($"Project file not found at: {_projectPath}");

        // First, build the project
        var buildProcess = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"build \"{_projectPath}\" --configuration Debug",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        });

        if (buildProcess == null) throw new InvalidOperationException("Failed to start build process");

        await buildProcess.WaitForExitAsync();
        if (buildProcess.ExitCode != 0) throw new InvalidOperationException($"Build failed with exit code {buildProcess.ExitCode}");

        // Then, run the project
        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project \"{_projectPath}\" --no-build --configuration Debug",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(_projectPath)!,
                Environment =
                {
                    ["ASPNETCORE_ENVIRONMENT"] = "Development"
                }
            }
        };

        _process.Start();

        await WaitForServerToBeReady();
    }

    public async Task DisposeAsync()
    {
        if (_process is { HasExited: false })
        {
            _process.Kill(true);
            await _process.WaitForExitAsync();
            _process.Dispose();
        }
    }

    private static async Task WaitForServerToBeReady()
    {
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(5);
        var startTime = DateTime.UtcNow;

        while (DateTime.UtcNow - startTime < TimeSpan.FromSeconds(MaxStartupWaitSeconds))
        {
            try
            {
                var response = await httpClient.GetAsync(BaseUrl);
                if (response.IsSuccessStatusCode) return;
            }
            catch
            {
                // Server not ready yet
            }

            await Task.Delay(500);
        }

        throw new TimeoutException($"Server did not start within {MaxStartupWaitSeconds} seconds at {BaseUrl}");
    }

    private static string FindSolutionRoot(string startPath)
    {
        var currentDir = new DirectoryInfo(startPath);
        while (currentDir != null)
        {
            if (currentDir.GetFiles("*.slnx").Length > 0 || currentDir.GetFiles("*.sln").Length > 0) return currentDir.FullName;
            currentDir = currentDir.Parent;
        }

        throw new InvalidOperationException("Could not find solution root directory");
    }
}

[CollectionDefinition("E2E Tests")]
public class E2ETestCollection : ICollectionFixture<WebServerFixture>
{
    // This class is never instantiated. It exists only to define the collection.
}
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FinSkew.Ui.E2ETests;

/// <summary>
///     Manages the lifecycle of the Blazor WASM app for E2E tests.
///     Starts a dedicated server instance on an isolated port and keeps it healthy for the suite.
/// </summary>
public class WebServerFixture : IAsyncLifetime
{
    private const int MaxStartupWaitSeconds = 60;
    private static readonly SemaphoreSlim SyncLock = new(1, 1);
    private static readonly StringBuilder ServerLogs = new();
    private static readonly string ProjectPath = GetProjectPath();
    private static Process? _process;
    private static string? _baseUrl;
    private static bool _isBuilt;

    public static string BaseUrl => _baseUrl ?? throw new InvalidOperationException("The E2E web server has not been started.");

    public Task InitializeAsync()
    {
        return EnsureServerIsRunningAsync();
    }

    public async Task DisposeAsync()
    {
        await SyncLock.WaitAsync();

        try
        {
            await StopServerProcessAsync();
            _baseUrl = null;
        }
        finally
        {
            SyncLock.Release();
        }
    }

    public static async Task EnsureServerIsRunningAsync()
    {
        await SyncLock.WaitAsync();

        try
        {
            if (await IsServerHealthyAsync()) return;

            await StopServerProcessAsync();

            if (!File.Exists(ProjectPath)) throw new InvalidOperationException($"Project file not found at: {ProjectPath}");

            if (!_isBuilt)
            {
                await BuildProjectAsync();
                _isBuilt = true;
            }

            _baseUrl = CreateBaseUrl();
            _process = StartServerProcess(_baseUrl);

            await WaitForServerToBeReadyAsync(_baseUrl);
        }
        finally
        {
            SyncLock.Release();
        }
    }

    private static async Task BuildProjectAsync()
    {
        using var buildProcess = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"build \"{ProjectPath}\" --configuration Debug",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        });

        if (buildProcess == null) throw new InvalidOperationException("Failed to start build process");

        var standardOutput = buildProcess.StandardOutput.ReadToEndAsync();
        var standardError = buildProcess.StandardError.ReadToEndAsync();

        await buildProcess.WaitForExitAsync();

        var output = await standardOutput;
        var error = await standardError;

        if (buildProcess.ExitCode == 0) return;

        throw new InvalidOperationException(
            $"Build failed with exit code {buildProcess.ExitCode}.{Environment.NewLine}{output}{Environment.NewLine}{error}");
    }

    private static string CreateBaseUrl()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();

        return $"http://127.0.0.1:{port}";
    }

    private static Process StartServerProcess(string baseUrl)
    {
        lock (ServerLogs)
        {
            ServerLogs.Clear();
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project \"{ProjectPath}\" --no-build --configuration Debug --no-launch-profile -- --urls {baseUrl}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(ProjectPath)!,
                Environment =
                {
                    ["ASPNETCORE_ENVIRONMENT"] = "Development",
                    ["ASPNETCORE_URLS"] = baseUrl
                }
            },
            EnableRaisingEvents = true
        };

        process.OutputDataReceived += (_, args) => AppendServerLog(args.Data);
        process.ErrorDataReceived += (_, args) => AppendServerLog(args.Data);

        if (!process.Start()) throw new InvalidOperationException("Failed to start web server process");

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return process;
    }

    private static async Task WaitForServerToBeReadyAsync(string baseUrl)
    {
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(5);

        var startTime = DateTime.UtcNow;

        while (DateTime.UtcNow - startTime < TimeSpan.FromSeconds(MaxStartupWaitSeconds))
        {
            if (_process is { HasExited: true })
                throw new InvalidOperationException(
                    $"The E2E web server exited before it became ready.{Environment.NewLine}{GetServerLogs()}");

            try
            {
                var response = await httpClient.GetAsync(baseUrl);
                if (response.IsSuccessStatusCode) return;
            }
            catch
            {
                // Server not ready yet.
            }

            await Task.Delay(500);
        }

        throw new TimeoutException(
            $"Server did not start within {MaxStartupWaitSeconds} seconds at {baseUrl}.{Environment.NewLine}{GetServerLogs()}");
    }

    private static async Task<bool> IsServerHealthyAsync()
    {
        if (string.IsNullOrWhiteSpace(_baseUrl) || _process is null || _process.HasExited) return false;

        try
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(2);

            var response = await httpClient.GetAsync(_baseUrl);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static async Task StopServerProcessAsync()
    {
        if (_process is null) return;

        try
        {
            if (!_process.HasExited)
            {
                _process.Kill(true);
                await _process.WaitForExitAsync();
            }
        }
        finally
        {
            _process.Dispose();
            _process = null;
        }
    }

    private static void AppendServerLog(string? message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        lock (ServerLogs)
        {
            ServerLogs.AppendLine(message);
        }
    }

    private static string GetServerLogs()
    {
        lock (ServerLogs)
        {
            return ServerLogs.ToString();
        }
    }

    private static string GetProjectPath()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var solutionRoot = FindSolutionRoot(currentDir);
        return Path.Combine(solutionRoot, "src", "FinSkew.Ui", "FinSkew.Ui.csproj");
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
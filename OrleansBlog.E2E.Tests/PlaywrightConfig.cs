using Microsoft.Playwright.NUnit;

namespace OrleansBlog.E2E.Tests;

public static class PlaywrightConfig
{
    public const string BaseUrl = "http://localhost:5261";
    public const int TimeoutMs = 30000;
    
    public static void ConfigureDefault()
    {
        // Use system-installed browsers from the earlier npx playwright install
        Environment.SetEnvironmentVariable("PLAYWRIGHT_BROWSERS_PATH", "/Users/jimschneider/Library/Caches/ms-playwright");
    }
}
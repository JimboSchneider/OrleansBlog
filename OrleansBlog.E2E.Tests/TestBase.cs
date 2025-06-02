using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;

namespace OrleansBlog.E2E.Tests;

[TestFixture]
public abstract class TestBase : PageTest
{
    [SetUp]
    public virtual async Task SetUp()
    {
        PlaywrightConfig.ConfigureDefault();
        
        // Set default timeout for all tests
        Page.SetDefaultTimeout(PlaywrightConfig.TimeoutMs);
        
        // Navigate to the application home page
        await Page.GotoAsync(PlaywrightConfig.BaseUrl);
        
        // Wait for page to load
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
    
    [TearDown]
    public virtual async Task TearDown()
    {
        // Optional: Clear any application state if needed
        // This could include logging out users, cleaning up test data, etc.
    }
    
    protected async Task WaitForApplicationToLoad()
    {
        // Wait for the Orleans Blog title to appear
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Orleans Blog" })).ToBeVisibleAsync();
    }
    
    protected async Task AssertPageTitle(string expectedTitle)
    {
        await Expect(Page).ToHaveTitleAsync(new Regex($".*{expectedTitle}.*"));
    }
    
    protected async Task TakeScreenshotOnFailure(string testName)
    {
        try
        {
            await Page.ScreenshotAsync(new()
            {
                Path = $"screenshots/{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png",
                FullPage = true
            });
        }
        catch
        {
            // Ignore screenshot errors
        }
    }
}
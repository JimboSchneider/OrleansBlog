using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using System.Text.RegularExpressions;

namespace OrleansBlog.E2E.Tests.TestHelpers;

public static class BlogTestHelpers
{
    public static async Task CreateBlogPost(IPage page, string title, string content, string? tags = null)
    {
        // Navigate to new post page (click the nav menu link, not the home page button)
        await page.Locator(".nav-link").Filter(new() { HasText = "New Post" }).ClickAsync();
        
        // Wait for page to load
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Fill in the form
        await page.GetByLabel("Title").FillAsync(title);
        await page.GetByLabel("Content").FillAsync(content);
        
        if (!string.IsNullOrEmpty(tags))
        {
            await page.GetByLabel("Tags (comma-separated)").FillAsync(tags);
        }
        
        // Submit the form
        await page.GetByRole(AriaRole.Button, new() { Name = "Create Post" }).ClickAsync();
        
        // Wait for redirect to post view page (indicates successful creation)
        await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex(".*/post/\\d+$"), new() { Timeout = 5000 });
    }
    
    public static async Task VerifyPostExistsOnHomePage(IPage page, string title)
    {
        // Navigate to home page
        await page.GotoAsync(PlaywrightConfig.BaseUrl);
        
        // Wait for page to load
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Verify post title appears on home page
        await Expect(page.GetByRole(AriaRole.Link, new() { Name = title })).ToBeVisibleAsync();
    }
    
    public static async Task NavigateToPost(IPage page, string title)
    {
        // Click on the post title link
        await page.GetByRole(AriaRole.Link, new() { Name = title }).ClickAsync();
        
        // Wait for page to load
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
    
    public static async Task VerifyPostContent(IPage page, string title, string content)
    {
        // Verify post title
        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = title })).ToBeVisibleAsync();
        
        // Verify post content - look for content within the main content area to avoid duplicates
        // Use a more specific selector to target the post content area
        var contentLocator = page.Locator("main").GetByText(content).First;
        await Expect(contentLocator).ToBeVisibleAsync();
    }
    
    public static async Task VerifyPostTags(IPage page, params string[] expectedTags)
    {
        foreach (var tag in expectedTags)
        {
            // Use a more specific selector to find tags within the main content area
            await Expect(page.Locator("main .badge").Filter(new() { HasText = tag }).First).ToBeVisibleAsync();
        }
    }
}
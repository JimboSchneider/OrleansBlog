using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

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
        
        // Wait for success message or redirect
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
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
        
        // Verify post content
        await Expect(page.GetByText(content)).ToBeVisibleAsync();
    }
    
    public static async Task VerifyPostTags(IPage page, params string[] expectedTags)
    {
        foreach (var tag in expectedTags)
        {
            await Expect(page.Locator(".badge").Filter(new() { HasText = tag })).ToBeVisibleAsync();
        }
    }
}
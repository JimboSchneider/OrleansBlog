using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using OrleansBlog.E2E.Tests.TestHelpers;
using System.Text.RegularExpressions;

namespace OrleansBlog.E2E.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class BlogPostTests : TestBase
{
    [Test]
    public async Task CreateBlogPost_WithValidData_ShouldCreateAndDisplayPost()
    {
        // Arrange - Create and login a test user
        var (testEmail, testPassword) = await UserTestHelpers.CreateAndRegisterTestUser(Page);
        await UserTestHelpers.LoginUser(Page, testEmail, testPassword);
        
        var postTitle = $"Test Post {Guid.NewGuid():N}";
        var postContent = "This is a test blog post content with multiple lines.\n\nIt should display correctly on the post view page.";
        var postTags = "test, automation, playwright";
        
        // Act - Create a blog post
        await BlogTestHelpers.CreateBlogPost(Page, postTitle, postContent, postTags);
        
        // Assert - Verify post was created successfully (success message appears briefly before redirect)
        await Expect(Page.GetByText("Post created successfully!")).ToBeVisibleAsync(new() { Timeout = 2000 });
        
        // Wait for redirect to post view page
        await Page.WaitForURLAsync(new System.Text.RegularExpressions.Regex(".*/post/\\d+$"), new() { Timeout = 3000 });
        
        // Verify we're on the post view page with the correct title
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = postTitle })).ToBeVisibleAsync();
        
        // Verify post appears on home page
        await BlogTestHelpers.VerifyPostExistsOnHomePage(Page, postTitle);
        
        // Navigate to the post and verify content
        await BlogTestHelpers.NavigateToPost(Page, postTitle);
        await BlogTestHelpers.VerifyPostContent(Page, postTitle, "This is a test blog post content");
        await BlogTestHelpers.VerifyPostTags(Page, "test", "automation", "playwright");
    }
    
    [Test]
    public async Task CreateBlogPost_WithoutAuthentication_ShouldNotShowNewPostLink()
    {
        // Arrange - Ensure user is not logged in by going to home page first
        await Page.GotoAsync(PlaywrightConfig.BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await UserTestHelpers.VerifyUserIsLoggedOut(Page);
        
        // Assert - New Post link should not be visible when not authenticated
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "New Post" })).Not.ToBeVisibleAsync();
        
        // And Login/Register links should be visible
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Login" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task CreateBlogPost_WithEmptyTitle_ShouldShowValidationError()
    {
        // Arrange - Create and login a test user
        var (testEmail, testPassword) = await UserTestHelpers.CreateAndRegisterTestUser(Page);
        await UserTestHelpers.LoginUser(Page, testEmail, testPassword);
        
        // Navigate to new post page (click the nav menu link)
        await Page.Locator(".nav-link").Filter(new() { HasText = "New Post" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Act - Try to create post with empty title
        await Page.GetByLabel("Content").FillAsync("Some content without title");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Create Post" }).ClickAsync();
        
        // Assert - Verify validation error appears
        await Expect(Page.Locator(".validation-message").GetByText("Title is required")).ToBeVisibleAsync();
        await AssertPageTitle("Create New Post");
    }
    
    [Test]
    public async Task CreateBlogPost_WithEmptyContent_ShouldShowValidationError()
    {
        // Arrange - Create and login a test user
        var (testEmail, testPassword) = await UserTestHelpers.CreateAndRegisterTestUser(Page);
        await UserTestHelpers.LoginUser(Page, testEmail, testPassword);
        
        // Navigate to new post page (click the nav menu link)
        await Page.Locator(".nav-link").Filter(new() { HasText = "New Post" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Act - Try to create post with empty content
        await Page.GetByLabel("Title").FillAsync("Test Title");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Create Post" }).ClickAsync();
        
        // Assert - Verify validation error appears
        await Expect(Page.Locator(".validation-message").GetByText("Content is required")).ToBeVisibleAsync();
        await AssertPageTitle("Create New Post");
    }
    
    [Test]
    public async Task ViewBlogPost_FromHomePage_ShouldDisplayCorrectContent()
    {
        // Arrange - Create a user, login, and create a post
        var (testEmail, testPassword) = await UserTestHelpers.CreateAndRegisterTestUser(Page);
        await UserTestHelpers.LoginUser(Page, testEmail, testPassword);
        
        var postTitle = $"View Test Post {Guid.NewGuid():N}";
        var postContent = "This is content for testing the view functionality.";
        
        await BlogTestHelpers.CreateBlogPost(Page, postTitle, postContent);
        
        // Wait for redirect to post view after creation
        await Page.WaitForURLAsync(new System.Text.RegularExpressions.Regex(".*/post/\\d+$"), new() { Timeout = 3000 });
        
        // Act - Navigate to home and click on the post
        await Page.GotoAsync(PlaywrightConfig.BaseUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await BlogTestHelpers.NavigateToPost(Page, postTitle);
        
        // Assert - Verify all post details are displayed
        await BlogTestHelpers.VerifyPostContent(Page, postTitle, postContent);
        
        // Verify creation date is displayed
        await Expect(Page.GetByText(new Regex("Created on .* at .*"))).ToBeVisibleAsync();
        
        // Verify back to home link works
        await Page.GetByRole(AriaRole.Link, new() { Name = "Back to Home" }).ClickAsync();
        await Page.WaitForURLAsync(PlaywrightConfig.BaseUrl);
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Orleans Blog" })).ToBeVisibleAsync();
    }
}
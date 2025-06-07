using Microsoft.Playwright;
using System.Text.RegularExpressions;
using static Microsoft.Playwright.Assertions;

namespace OrleansBlog.E2E.Tests.TestHelpers;

public static class UserTestHelpers
{
    public static async Task<(string email, string password)> CreateAndRegisterTestUser(IPage page)
    {
        var testEmail = $"testuser_{Guid.NewGuid():N}@example.com";
        var testPassword = "TestPassword123!";
        
        await RegisterUser(page, testEmail, testPassword);
        
        return (testEmail, testPassword);
    }
    
    public static async Task RegisterUser(IPage page, string email, string password)
    {
        // Navigate to register page
        await page.GotoAsync($"{PlaywrightConfig.BaseUrl}/Account/Register");
        
        // Wait for page to load
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Fill registration form
        await page.GetByLabel("Email").FillAsync(email);
        await page.GetByLabel("Password", new() { Exact = true }).FillAsync(password);
        await page.GetByLabel("Confirm Password").FillAsync(password);
        
        // Submit registration
        await page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        
        // Wait for registration to complete
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Handle email confirmation
        // The app should redirect to RegisterConfirmation page with a confirmation link
        await page.WaitForURLAsync($"**/Account/RegisterConfirmation**");
        
        // Look for the email confirmation link that's displayed on the page
        // (since we're using IdentityNoOpEmailSender, it shows the link directly)
        var confirmationLink = page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your account" });
        await Expect(confirmationLink).ToBeVisibleAsync(new() { Timeout = 5000 });
        
        // Click the confirmation link
        await confirmationLink.ClickAsync();
        
        // Wait for email confirmation to complete
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
    
    public static async Task LoginUser(IPage page, string email, string password)
    {
        // Navigate to login page
        await page.GotoAsync($"{PlaywrightConfig.BaseUrl}/Account/Login");
        
        // Wait for page to load
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Fill in login form
        await page.GetByLabel("Email").FillAsync(email);
        await page.GetByLabel("Password", new() { Exact = true }).FillAsync(password);
        
        // Click login button
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        
        // Wait for navigation after successful login
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
    
    public static async Task LogoutUser(IPage page)
    {
        // Click logout button
        await page.GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync();
        
        // Wait for redirect after logout
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
    
    public static async Task VerifyUserIsLoggedIn(IPage page, string email)
    {
        // Wait for authentication state to update
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Wait additional time for Blazor interactive components to initialize
        // This is necessary because AuthorizeView components require interactive mode
        // and may take extra time to render after authentication state changes
        await Task.Delay(3000);
        
        // Primary indicators of successful login
        // Verify "New Post" link is visible (only for authenticated users)
        await Expect(page.Locator("a.nav-link", new() { HasTextString = "New Post" })).ToBeVisibleAsync();
        // Verify Logout button is visible
        await Expect(page.Locator("button.nav-link", new() { HasTextString = "Logout" })).ToBeVisibleAsync();
        
        // Verify Login/Register links are no longer visible
        await Expect(page.Locator("a.nav-link", new() { HasTextString = "Login" })).Not.ToBeVisibleAsync();
        await Expect(page.Locator("a.nav-link", new() { HasTextString = "Register" })).Not.ToBeVisibleAsync();
    }
    
    public static async Task VerifyUserIsLoggedOut(IPage page)
    {
        // Verify Login link is visible
        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Login" })).ToBeVisibleAsync();
        
        // Verify Register link is visible (use exact match to avoid conflicts)
        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true })).ToBeVisibleAsync();
        
        // Verify "New Post" link is not visible
        await Expect(page.GetByRole(AriaRole.Link, new() { Name = "New Post" })).Not.ToBeVisibleAsync();
        
        // Verify Logout button is not visible
        await Expect(page.GetByRole(AriaRole.Button, new() { Name = "Logout" })).Not.ToBeVisibleAsync();
    }
}
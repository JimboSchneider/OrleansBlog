using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using OrleansBlog.E2E.Tests.TestHelpers;

namespace OrleansBlog.E2E.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class LoginTests : TestBase
{
    [Test]
    public async Task UserLogin_WithValidCredentials_ShouldLoginSuccessfully()
    {
        // Arrange - Create and register a test user
        var (testEmail, testPassword) = await UserTestHelpers.CreateAndRegisterTestUser(Page);
        
        // Act - Login with the registered user
        await UserTestHelpers.LoginUser(Page, testEmail, testPassword);
        
        // Assert - Verify successful login
        await UserTestHelpers.VerifyUserIsLoggedIn(Page, testEmail);
    }

    [Test]
    public async Task UserLogin_WithInvalidCredentials_ShouldShowError()
    {
        // Arrange
        var invalidEmail = "invalid@example.com";
        var invalidPassword = "wrongpassword";
        
        // Navigate to login page
        await Page.GotoAsync($"{PlaywrightConfig.BaseUrl}/Account/Login");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Act - Try to login with invalid credentials
        await Page.GetByLabel("Email").FillAsync(invalidEmail);
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync(invalidPassword);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        
        // Assert - Verify error message and staying on login page
        await Expect(Page.GetByText("Error: Invalid login attempt.")).ToBeVisibleAsync(new() { Timeout = 10000 });
        
        await AssertPageTitle("Log in");
        await UserTestHelpers.VerifyUserIsLoggedOut(Page);
    }

    [Test]
    public async Task UserLogin_WithEmptyFields_ShouldShowValidationErrors()
    {
        // Navigate to login page
        await Page.GotoAsync($"{PlaywrightConfig.BaseUrl}/Account/Login");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Act - Try to submit empty form
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        
        // Assert - Verify validation errors appear (use more specific selectors)
        await Expect(Page.Locator(".validation-message").GetByText("The Email field is required")).ToBeVisibleAsync();
        await Expect(Page.Locator(".validation-message").GetByText("The Password field is required")).ToBeVisibleAsync();
        await AssertPageTitle("Log in");
    }
}
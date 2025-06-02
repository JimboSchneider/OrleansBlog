# OrleansBlog End-to-End Tests

This project contains Playwright-based end-to-end tests for the OrleansBlog application.

## Setup

### Prerequisites
- .NET 9.0 SDK
- Node.js (for Playwright browser installation)
- OrleansBlog application running locally

### Installation
1. Install Playwright browsers:
   ```bash
   cd OrleansBlog.E2E.Tests
   npx playwright install
   ```

2. Build the test project:
   ```bash
   dotnet build
   ```

## Running Tests

### Manual Testing
1. Start the Orleans Silo:
   ```bash
   cd OrleansBlog.Silo
   dotnet run
   ```

2. Start the Blazor web application:
   ```bash
   cd OrleansBlog
   dotnet run
   ```

3. Run the E2E tests:
   ```bash
   cd OrleansBlog.E2E.Tests
   dotnet test
   ```

### Automated Testing
For CI/CD scenarios, you can run tests with specific configurations:

```bash
# Run tests with specific timeout
dotnet test --logger "console;verbosity=detailed" -- NUnit.DefaultTimeout=60000

# Run specific test category
dotnet test --filter Category=Login

# Run tests in parallel
dotnet test --parallel
```

## Test Structure

### Test Categories
- **LoginTests**: User authentication scenarios
- **BlogPostTests**: Blog post creation and viewing scenarios

### Test Helpers
- **UserTestHelpers**: User management utilities (registration, login, logout)
- **BlogTestHelpers**: Blog post utilities (creation, verification)
- **TestBase**: Base class with common setup and utilities

### Configuration
- **PlaywrightConfig**: Central configuration for URLs and timeouts
- **playwright.config.js**: Playwright-specific configuration

## Test Scenarios

### LoginTests
- ✅ Successful login with valid credentials
- ✅ Login failure with invalid credentials  
- ✅ Validation errors with empty fields

### BlogPostTests
- ✅ Create blog post with valid data
- ✅ Authentication required for post creation
- ✅ Validation errors for empty title/content
- ✅ View blog post from home page

## Configuration

The tests are configured to run against `https://localhost:7233` by default. This can be changed in `PlaywrightConfig.cs`.

### Browser Configuration
Tests run on:
- Chromium (default)
- Firefox
- WebKit (Safari)

### Timeouts
- Default timeout: 30 seconds
- Custom timeouts can be set per test

## Best Practices

1. **Test Isolation**: Each test creates its own test user to avoid conflicts
2. **Wait Strategies**: Use `WaitForLoadState` and `WaitForURL` for reliable tests
3. **Assertions**: Use Playwright's built-in assertions for better error messages
4. **Screenshots**: Automatic screenshots on test failures
5. **Parallel Execution**: Tests are marked as parallelizable where safe

## Troubleshooting

### Common Issues
1. **Application not running**: Ensure both Silo and Web app are running
2. **Browser not found**: Run `npx playwright install` to install browsers
3. **Timeout errors**: Check if application is responsive and increase timeouts if needed
4. **Element not found**: Verify selectors match the current UI

### Debug Tips
- Use `--headed` flag to see browser during test execution
- Add `await page.PauseAsync()` to pause execution for debugging
- Use browser developer tools to inspect elements and validate selectors
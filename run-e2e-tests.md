# Running E2E Tests - Step by Step Guide

## Prerequisites Setup

### 1. Install Playwright Browsers
Since the .NET Playwright tool requires PowerShell on macOS, you can install browsers manually:

```bash
# Navigate to E2E test directory
cd OrleansBlog.E2E.Tests

# Install browsers via npm (if not already done)
npm init -y
npm install @playwright/test
npx playwright install
```

### 2. Set Environment Variable (Optional)
```bash
export PLAYWRIGHT_BROWSERS_PATH=/Users/jimschneider/Library/Caches/ms-playwright
```

## Running the Complete Test Suite

### Step 1: Start Orleans Silo
In Terminal 1:
```bash
cd OrleansBlog.Silo
dotnet run
```
Wait for: "Press Enter to terminate..."

### Step 2: Start Blazor Web Application  
In Terminal 2:
```bash
cd OrleansBlog
dotnet run
```
Wait for: "Now listening on: https://localhost:7233"

### Step 3: Run E2E Tests
In Terminal 3:
```bash
cd OrleansBlog.E2E.Tests
dotnet test --logger "console;verbosity=detailed"
```

## Expected Test Results

The E2E tests will validate:

### LoginTests (3 tests)
- ✅ **UserLogin_WithValidCredentials_ShouldLoginSuccessfully**
  - Creates test user → Logs in → Verifies authentication state
- ✅ **UserLogin_WithInvalidCredentials_ShouldShowError**  
  - Attempts invalid login → Verifies error message
- ✅ **UserLogin_WithEmptyFields_ShouldShowValidationErrors**
  - Submits empty form → Verifies validation messages

### BlogPostTests (5 tests)
- ✅ **CreateBlogPost_WithValidData_ShouldCreateAndDisplayPost**
  - Creates user → Logs in → Creates post → Verifies on home page
- ✅ **CreateBlogPost_WithoutAuthentication_ShouldRedirectToLogin**
  - Attempts post creation → Verifies redirect to login
- ✅ **CreateBlogPost_WithEmptyTitle_ShouldShowValidationError**
  - Submits post without title → Verifies validation
- ✅ **CreateBlogPost_WithEmptyContent_ShouldShowValidationError**
  - Submits post without content → Verifies validation  
- ✅ **ViewBlogPost_FromHomePage_ShouldDisplayCorrectContent**
  - Creates post → Views from home → Verifies content display

## Troubleshooting

### Browser Issues
If tests fail with browser errors:
```bash
# Try manually installing for .NET Playwright
export PLAYWRIGHT_BROWSERS_PATH=""
cd OrleansBlog.E2E.Tests
dotnet build
# Then use existing browsers from system cache
```

### Application Not Running
Tests will fail if either Orleans Silo or Web app aren't running:
- Check both terminals are running
- Verify URLs respond: `curl https://localhost:7233`
- Check for port conflicts

### Test Isolation
Each test creates unique users with GUIDs to avoid conflicts. Tests can run in parallel safely.

## Test Features Demonstrated

- **User Registration & Authentication Flow**
- **Blog Post Creation with Validation**
- **Navigation and UI Interactions**  
- **Form Validation and Error Handling**
- **Orleans Grain Integration through UI**
- **Multi-step User Journeys**

The E2E tests provide confidence that the complete application stack (Orleans Silo → Grains → Blazor UI) works correctly for end users.
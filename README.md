# OrleansBlog

A blog application built with .NET Blazor Server and Microsoft Orleans Framework. This project demonstrates the use of Orleans' grain-based architecture for scalable, distributed state management in a real-world blog application.

## Current Status
**Functional Blog Platform** - Core blog functionality is implemented with Orleans grains, including post creation, viewing, and comprehensive testing.

## Technology Stack
- **.NET 9.0** - Latest .NET version
- **Blazor Server** - Interactive web UI with server-side rendering
- **Microsoft Orleans 9.1.2** - Actor-based distributed computing framework
- **Entity Framework Core 9.0** - ORM for data access
- **ASP.NET Core Identity** - Authentication and authorization
- **SQLite** - Database for development (SQL Server supported for production)

## Architecture
The solution follows a clean architecture pattern with the following projects:

- **OrleansBlog** - Blazor Server web application (UI layer)
- **OrleansBlog.Abstractions** - Grain interfaces and domain models
- **OrleansBlog.Grains** - Orleans grain implementations (business logic)
- **OrleansBlog.Silo** - Orleans host/runtime
- **OrleansBlog.Tests** - Comprehensive unit tests with Orleans TestingHost
- **OrleansBlog.E2E.Tests** - End-to-end tests with Playwright for UI testing

## Prerequisites
- .NET 9.0 SDK
- SQLite (for development) or SQL Server (for production)

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/yourusername/OrleansBlog.git
cd OrleansBlog
```

### 2. Build the solution
```bash
dotnet build
```

### 3. Run tests (optional)
```bash
# Unit tests
dotnet test OrleansBlog.Tests

# End-to-end tests (requires application to be running)
dotnet test OrleansBlog.E2E.Tests
```

### 4. Run the Orleans Silo (required first)
In one terminal:
```bash
cd OrleansBlog.Silo
dotnet run
```

### 5. Run the Blazor Web Application
In another terminal:
```bash
cd OrleansBlog
dotnet run
```

The application will be available at:
- HTTP: http://localhost:5261
- HTTPS: https://localhost:7233

## Using the Blog

### Creating Posts
1. Register a new account or log in with an existing account
2. Navigate to the "New Post" link in the navigation menu (only visible when authenticated)
3. Fill out the post form with title, content, and optional tags (comma-separated)
4. Submit to create the post using Orleans grains for distributed state management

### Viewing Posts
- Recent posts are displayed on the home page
- Click any post title to view the full post content
- Posts show creation date, tags, and formatted content

### Authentication
- User registration and login powered by ASP.NET Core Identity
- Only authenticated users can create posts
- User data stored in SQLite database

## Development Notes

### Database
- Development uses SQLite for simplicity (`orleans-blog.db` file)
- Database is automatically created on first run
- Connection string can be configured in `appsettings.Development.json`

### Running in Background
To run the silo without console interaction:
```bash
cd OrleansBlog.Silo
ORLEANS_NO_WAIT=true dotnet run
```

### Viewing SQLite Database
```bash
# List tables
sqlite3 orleans-blog.db ".tables"

# View data
sqlite3 orleans-blog.db "SELECT * FROM AspNetUsers;"
```

## Current Features
- ✅ Orleans silo and client configuration
- ✅ ASP.NET Core Identity integration with authentication
- ✅ SQLite database with Entity Framework migrations
- ✅ Blog post creation with form validation and error handling
- ✅ Post viewing with formatted content display
- ✅ PostGrain implementation with Create, Update, and Get operations
- ✅ PostService for business logic and grain interaction
- ✅ Recent posts display on home page
- ✅ Tag support with comma-separated input
- ✅ Comprehensive unit testing with Orleans TestingHost
- ✅ End-to-end testing with Playwright for UI scenarios
- ✅ Navigation integration for authenticated users

## Planned Features
- [ ] Comments system
- [ ] Post editing functionality
- [ ] Advanced search and filtering
- [ ] Admin dashboard
- [ ] Orleans grain persistence with SQLite storage
- [ ] Production deployment configuration
- [ ] Post categories and advanced tagging

## Known Issues
- Orleans grains currently use in-memory storage (persistence planned)
- Localhost clustering only (not production-ready)
- Limited post formatting options

## Contributing
This is a learning project for exploring Orleans Framework. Contributions and suggestions are welcome!

## License
This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

### MIT License Summary
This software is provided under the MIT License, which means:
- ✅ Commercial use allowed
- ✅ Modification allowed
- ✅ Distribution allowed
- ✅ Private use allowed
- ⚠️ No liability or warranty
- 📋 License and copyright notice must be included

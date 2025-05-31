# OrleansBlog

A blog application built with .NET Blazor Server and Microsoft Orleans Framework. This project demonstrates the use of Orleans' grain-based architecture for scalable, distributed state management in a real-world blog application.

## Current Status
**Work in Progress** - Basic infrastructure is in place and functional. Blog features are not yet implemented.

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

### 3. Run the Orleans Silo (required first)
In one terminal:
```bash
cd OrleansBlog.Silo
dotnet run
```

### 4. Run the Blazor Web Application
In another terminal:
```bash
cd OrleansBlog
dotnet run
```

The application will be available at:
- HTTP: http://localhost:5261
- HTTPS: https://localhost:7233

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
- ✅ ASP.NET Core Identity integration
- ✅ SQLite database with Entity Framework migrations
- ✅ Basic Blazor Server setup

## Planned Features
- [ ] Blog post creation and editing (using Orleans grains)
- [ ] Comments system
- [ ] Tags and categories
- [ ] Search functionality
- [ ] Admin dashboard
- [ ] Orleans grain persistence (MongoDB planned)
- [ ] Production deployment configuration

## Known Issues
- Orleans grains currently use in-memory storage only
- No actual blog functionality implemented yet
- Localhost clustering only (not production-ready)

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

# Contributing to OrleansBlog

Thank you for your interest in contributing to OrleansBlog! This document provides guidelines and information for contributors.

## Getting Started

1. Fork the repository
2. Clone your fork locally
3. Create a new branch for your feature or bug fix
4. Make your changes
5. Submit a pull request

## Development Setup

Please refer to the [README.md](README.md) for detailed setup instructions.

## Pull Request Process

### Build Validation

All pull requests targeting the `main` branch trigger automated build validation on both Windows and Linux platforms. The CI/CD pipeline will:

- Build all projects in Release configuration
- Cache NuGet packages for faster builds
- Start Orleans Silo and Blazor Server
- Verify services are running correctly

### Documentation-Only Changes

To save CI/CD resources, the following changes **do not** trigger builds:

- Markdown files (`*.md`)
- License files (`LICENSE*`)
- `.gitignore` file
- Documentation in `docs/` directory
- GitHub markdown files in `.github/*.md`
- GitHub issue templates

This means you can update documentation without waiting for lengthy build processes!

## Code Style

- Follow existing code conventions in the codebase
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Ensure all tests pass before submitting

## Testing

### Unit Tests
```bash
dotnet test OrleansBlog.Tests
```

### E2E Tests
E2E tests require the application to be running. See the [E2E Tests README](OrleansBlog.E2E.Tests/README.md) for details.

## Commit Messages

- Use clear, descriptive commit messages
- Start with a verb in present tense (e.g., "Add", "Fix", "Update")
- Keep the first line under 50 characters
- Add detailed description if needed after a blank line

## Reporting Issues

- Use GitHub Issues to report bugs or suggest features
- Check existing issues before creating a new one
- Provide clear reproduction steps for bugs
- Include relevant system information

## Questions?

Feel free to open an issue for any questions about contributing!
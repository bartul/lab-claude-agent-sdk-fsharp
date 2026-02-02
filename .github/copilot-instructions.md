# Copilot Instructions

F# implementation exploring Claude's Agent SDK capabilities.

## Build & Test

```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Run tests
dotnet test

# Run a single test
dotnet test --filter "FullyQualifiedName~TestName"

# Run with watch mode
dotnet watch test
```

## Architecture

<!-- Update this section as the project takes shape -->

This project wraps or interacts with Claude's Agent SDK. Key areas to document:
- How F# types map to SDK concepts
- Async patterns used (Task vs Async)
- Error handling approach

## Conventions

- Use F# idiomatic patterns (discriminated unions, pattern matching, pipelines)
- Prefer immutable data structures
- Use `Async` for F# async workflows, `Task` when interfacing with .NET libraries

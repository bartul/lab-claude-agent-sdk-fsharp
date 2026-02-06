# F# Fable Claude Agent SDK Proof-of-Concept

## Introduction

This project is a proof-of-concept that explores using the Claude Agent SDK (originally written for TypeScript) from F# code via the Fable transpiler. The goal was to enable F# developers to leverage the Claude Agent SDK's capabilities while working in a functional programming paradigm.

Fable is an F#-to-JavaScript compiler that transpiles F# code into idiomatic JavaScript. This POC uses Fable to create type-safe F# bindings for the Claude Agent SDK, allowing F# code to directly utilize the SDK after transpilation to JavaScript.

## TL;DR

**Verdict: This approach is not satisfactory for production use.**

While technically feasible, binding the TypeScript Claude Agent SDK to F# via Fable introduces significant friction:
- **980 lines of manual bindings** that require constant maintenance
- **Type system mismatch** between TypeScript's structural and F#'s nominal typing
- **Brittle runtime behavior** with no compile-time safety guarantees
- **Loss of F# idioms** - forced to work with JavaScript patterns
- **High maintenance burden** for every SDK update

For F# developers, using the Anthropic API directly is a better approach. See the [Conclusion](#conclusion) section for full details and recommendations.

## Design

### Project Structure

```
lab-claude-agent-sdk-fsharp/
├── src/                         # F# source code
│   ├── Bindings.fs             # Core TypeScript SDK bindings (980 lines)
│   ├── ClaudeAgent.fs          # Helper functions for running queries
│   └── Program.fs              # Example CLI application
├── output/                      # Fable-compiled JavaScript (gitignored)
├── package.json                # NPM dependencies and build scripts
└── README.md                   # This file
```

**Dependencies**:
- `@anthropic-ai/claude-agent-sdk` (v0.2.31) - The official TypeScript SDK
- Fable tooling for F#-to-JavaScript compilation
- .NET 10.0 SDK for F# compilation

### Binding Architecture

The F# bindings use Fable's Foreign Function Interface (FFI) to create type-safe interop with the TypeScript SDK:

**Type-Safe FFI Attributes**:
- `[<Import>]` - Imports JavaScript modules and functions
- `[<Emit>]` - Generates inline JavaScript code
- `[<StringEnum>]` - Maps F# discriminated unions to TypeScript string literals
- `[<Erase>]` - Creates zero-cost abstractions that disappear at runtime

**Key Binding Patterns**:

1. **String Enums for TypeScript Literals**:
   ```fsharp
   [<StringEnum>]
   type PermissionBehavior =
       | [<CompiledName("allow")>] Allow
       | [<CompiledName("deny")>] Deny
       | [<CompiledName("ask")>] Ask
   ```

2. **Erased Discriminated Unions for Pattern Matching**:
   ```fsharp
   [<Erase>]
   type SDKMessage =
       | SystemMessage of SystemMessage
       | UserMessage of UserMessage
       | AssistantMessage of AssistantMessage
       | ResultMessage of ResultMessage
   ```

3. **F# Records for Configuration Structures**:
   Strongly-typed configuration objects matching TypeScript interfaces

4. **Promise Integration**:
   Seamless async/await via `Fable.Promise` for JavaScript Promise interop

5. **Observable Pattern**:
   Streaming support via `AsyncRx` for handling asynchronous event streams

### Code Organization

The codebase is organized into three main components:

**`Bindings.fs` (980 lines)**:
- Complete type bindings matching the TypeScript SDK interfaces
- Environment variable access (`CLAUDE_PATH`, authentication tokens)
- Core SDK functions: `query`, `runAgent`, message streaming
- Permission systems, tool configurations, and agent options

**`ClaudeAgent.fs`**:
- High-level helper functions (`runQuery`, `runQueryObservable`)
- Message classification system for pattern matching on SDK messages
- Simplified API for common use cases

**`Program.fs`**:
- Example CLI application demonstrating SDK usage
- Command-line argument parsing
- Pattern matching on agent responses

## Development

### Prerequisites

- **.NET 10.0 SDK** - For F# compilation
- **Node.js** - JavaScript runtime (v18+ recommended)
- **pnpm** - Fast, efficient package manager (recommended over npm)
- **Fable tooling** - Installed automatically via `dotnet tool restore`

### Setup Instructions

Install dependencies and prepare the development environment:

```bash
pnpm install
pnpm run setup
```

The `setup` script performs the following operations:
1. `dotnet tool restore` - Installs Fable, Femto, and Fantomas
2. `dotnet restore src` - Restores F# NuGet dependencies
3. `dotnet femto src --resolve` - Resolves NPM dependencies for Fable packages

### Build and Run Commands

**Note**: Use `pnpm` for all commands below.

#### Build Only

Compile F# source files to JavaScript:

```bash
pnpm run build
```

This runs Fable to transpile all F# files in `src/` to JavaScript in `output/`.

#### Run with Claude Code Settings

Execute the application with environment variables loaded from Claude Code:

```bash
pnpm start
```

This command:
1. Extracts environment variables from `~/.claude/settings.json` using `jq`
2. Exports them to the current shell session
3. Runs `output/Program.js` with those variables available

#### Run Without Environment Injection

Execute the compiled JavaScript directly:

```bash
pnpm run start:plain
```

Runs `output/Program.js` without loading Claude settings. Useful for testing with manually-set environment variables.

#### Development Mode (Watch + Auto-Run)

Continuous compilation and execution during development:

```bash
pnpm run dev
```

This watches F# source files for changes, automatically recompiles them, and runs the application on each change.

### Claude Code Settings Integration

The `start` script integrates with Claude Code's configuration system to provide authentication and runtime settings:

**Settings File**: `~/.claude/settings.json`

**Environment Variable Extraction**:
```bash
eval $(jq -r '.env | to_entries | .[] | "export \(.key)=\"\(.value)\""' ~/.claude/settings.json)
```

**Variables Loaded**:
- `ANTHROPIC_AUTH_TOKEN` - API authentication token
- `ANTHROPIC_BASE_URL` - Base URL for Anthropic API
- Model configurations (default model, temperature, etc.)
- Telemetry and logging settings
- `CLAUDE_PATH` - Path to Claude CLI executable (defaults to `/opt/homebrew/bin/claude`)

These settings provide seamless authentication and configuration for the SDK at runtime without hardcoding credentials.

## Conclusion

After implementing and testing this proof-of-concept, **this approach is not satisfactory** for production use or serious development work.

### Friction Points

1. **Binding Complexity**:
   The 980-line `Bindings.fs` file demonstrates the significant effort required to manually maintain bindings. Every type, interface, and function from the TypeScript SDK must be carefully translated to F# with appropriate Fable attributes. This is error-prone and time-consuming.

2. **Type System Impedance Mismatch**:
   TypeScript's structural typing fundamentally differs from F#'s nominal typing. Many TypeScript patterns (union types, intersection types, conditional types) require awkward workarounds in F#. This creates friction when trying to express TypeScript idioms in F#.

3. **Brittle Bindings**:
   Changes to the JavaScript SDK can silently break F# bindings. TypeScript would catch these at compile-time, but F# bindings may only fail at runtime when JavaScript attempts to access missing properties or call removed functions.

4. **Loss of F# Idioms**:
   The bindings force F# code to work with JavaScript patterns rather than idiomatic F# constructs. This negates many benefits of using F# in the first place—pattern matching becomes cumbersome, discriminated unions feel unnatural, and optional values lose their elegance.

5. **Maintenance Burden**:
   Every SDK update requires:
   - Reviewing TypeScript changelog for breaking changes
   - Manually updating F# bindings to match
   - Testing to ensure runtime behavior matches expectations
   - Fixing any newly-discovered type mismatches

   For a rapidly-evolving SDK, this maintenance overhead becomes untenable.

### Recommendations

For F# developers interested in using Claude's capabilities:
- Use the **Anthropic API directly** with F# HTTP clients (like `FSharp.Data.Http` or `Fable.SimpleHttp`)
- Create a lightweight F# wrapper around the REST API rather than binding to the TypeScript SDK
- Consider using TypeScript directly for projects requiring the full Claude Agent SDK feature set

This POC successfully demonstrates that Fable *can* bind to complex TypeScript SDKs, but the practical costs outweigh the benefits for this particular use case.

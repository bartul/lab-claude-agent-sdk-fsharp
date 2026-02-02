module Bindings

open Fable.Core
open Fable.Core.JsInterop

/// Default path to Claude CLI executable.
/// Reads from CLAUDE_PATH env var, falls back to Homebrew location.
[<Emit("process.env.CLAUDE_PATH || '/opt/homebrew/bin/claude'")>]
let defaultClaudePath: string = jsNative

/// Message from the agent (simplified)
type SDKMessage =
    abstract ``type``: string with get

/// Result message with content
type SDKResultMessage =
    abstract ``type``: string with get
    abstract result: string with get

/// SDK Options
type QueryOptions = {| pathToClaudeCodeExecutable: string option |}

/// Query parameters
type QueryParams =
    {| prompt: string
       options: QueryOptions option |}

/// AsyncGenerator returned by query
type Query =
    inherit System.Collections.Generic.IAsyncEnumerable<SDKMessage>

/// Import the query function from SDK
[<Import("query", "@anthropic-ai/claude-agent-sdk")>]
let query: QueryParams -> Query = jsNative

/// Convert any AsyncIterable/AsyncGenerator to an array.
/// Uses native Array.fromAsync (Node 22+).
[<Emit("Array.fromAsync($0)")>]
let toArray (asyncIterable: 'T) : JS.Promise<'U array> = jsNative

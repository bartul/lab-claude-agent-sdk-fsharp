module ClaudeAgent

open Fable.Core
open Fable.Core.JS
open Bindings

/// Run a simple query against Claude Agent using system CLI
let runQuery (claudePath: string) (prompt: string) : Promise<SDKMessage array> =
    let options =
        { Options.defaults () with
            pathToClaudeCodeExecutable = Some claudePath 
            permissionMode = Some PermissionMode.Plan 
            allowedTools = Some [| "Read"; "WebSearch" |] }

    let parameters: QueryParams =
        {| prompt = U2.Case1 prompt
           options = Some options |}

    query parameters |> toArray

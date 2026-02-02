module ClaudeAgent

open Fable.Core.JS
open Bindings

/// Run a simple query against Claude Agent using system CLI
let runQuery (claudePath: string) (prompt: string) : Promise<SDKMessage array> =
    let paramz: QueryParams =
        {| prompt = prompt
           options = Some {| pathToClaudeCodeExecutable = Some claudePath |} |}

    query paramz |> toArray

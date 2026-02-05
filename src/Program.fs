module Program

open Fable.Core
open Fable.Core.JsInterop
open ClaudeAgent
open Bindings

/// Get command line args (skip first two: node and script path)
[<Emit("process.argv.slice(2)")>]
let argv: string array = jsNative

let main () =
    // Parse --claude-path argument, fallback to env/default
    let claudePath =
        argv
        |> Array.tryFindIndex (fun arg -> arg = "--claude-path")
        |> Option.bind (fun i -> Array.tryItem (i + 1) argv)
        |> Option.defaultValue defaultClaudePath

    promise {
        printfn "Starting Claude Agent query..."
        printfn "Using Claude path: %s" claudePath

        let! messages = runQuery claudePath "Check the Bindings.fs file and validate if encapsulates the latest version of Claude Agent TypeScript SDK"

        for msg in messages do
            match classifyMessage msg with
            | SystemKind sysMsg ->
                printfn "System message with model %s" sysMsg.model
            | UserKind userMsg ->
                printfn "User message received (session: %s)" userMsg.session_id
            | AssistantKind assistantMsg ->
                printfn "Assistant message received (uuid: %s)" assistantMsg.uuid
            | ResultKind resultMsg ->
                printfn "Agent done: %s" resultMsg.result
            | _ ->
                printfn "Other message: %A" msg
    }
    |> ignore

main ()

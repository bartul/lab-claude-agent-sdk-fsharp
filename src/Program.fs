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

        let! messages = runQuery claudePath "What is 2 + 2? Reply briefly."

        for msg in messages do
            printfn "Message type: %s" msg.``type``
            // Check if it's a result message
            if msg.``type`` = "result" then
                let resultMsg: SDKResultMessage = !!msg
                printfn "Agent: %s" resultMsg.result

        printfn "Done."
    }
    |> ignore

main ()

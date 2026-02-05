module ClaudeAgent

open System
open Fable.Core
module JS = Fable.Core.JS
open FSharp.Control
open Bindings

/// Run a simple query against Claude Agent using system CLI
let runQuery (claudePath: string) (prompt: string) : JS.Promise<SDKMessage array> =
    let options =
        { Options.defaults () with
            pathToClaudeCodeExecutable = Some claudePath 
            permissionMode = Some PermissionMode.Plan 
            allowedTools = Some [| "Read"; "WebSearch" |] }

    let parameters: QueryParams =
        {| prompt = U2.Case1 prompt
           options = Some options |}

    query parameters |> toArray

/// Stream query results as AsyncRx observable
let runQueryObservable (claudePath: string) (prompt: string) : IAsyncObservable<SDKMessage> =
    let options =
        { Options.defaults () with
            pathToClaudeCodeExecutable = Some claudePath
            permissionMode = Some PermissionMode.Plan
            allowedTools = Some [| "Read"; "WebSearch" |] }

    let parameters: QueryParams =
        {| prompt = U2.Case1 prompt
           options = Some options |}

    AsyncRx.create (fun observer ->
        async {
            let mutable cancelled = false
            let mutable tokenOpt: AsyncIterable.CancellationToken option = None

            let cancel () =
                async {
                    cancelled <- true
                    tokenOpt |> Option.iter (fun token -> token.Cancel())
                }

            let iterable = query parameters |> unbox<JS.AsyncIterable<SDKMessage>>

            let iteration =
                AsyncIterable.iter
                    (fun token msg ->
                        if tokenOpt.IsNone then
                            tokenOpt <- Some token

                        if cancelled then
                            token.Cancel()
                        else
                            observer.OnNextAsync msg |> Async.StartImmediate)
                    iterable

            iteration
            |> Promise.map (fun () ->
                if not cancelled then
                    observer.OnCompletedAsync() |> Async.StartImmediate)
            |> Promise.catch (fun err ->
                if not cancelled then
                    observer.OnErrorAsync(Exception(string err)) |> Async.StartImmediate
                ())
            |> Promise.iter ignore

            return AsyncDisposable.Create cancel
        })

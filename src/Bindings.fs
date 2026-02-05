module Bindings

open System
open System.Collections.Generic
open Fable.Core
open Fable.Core.JsInterop
open Fable.Core.JS

/// Default path to Claude CLI executable.
/// Reads from CLAUDE_PATH env var, falls back to Homebrew location.
[<Emit("process.env.CLAUDE_PATH || '/opt/homebrew/bin/claude'")>]
let defaultClaudePath: string = jsNative

// Runtime and stream placeholders
// These are kept as obj to avoid pulling extra runtime bindings.
type AbortController = obj
type AbortSignal = obj
type Readable = obj
type Writable = obj
type McpServer = obj
type UUID = string
type BetaMessageContainer = obj
type BetaContentBlock = obj
type BetaContextManagementResponse = obj
type BetaStopReason = string
type BetaUsage = obj
type BetaMessage =
    abstract id: string with get
    abstract container: BetaMessageContainer option with get
    abstract content: BetaContentBlock array with get
    abstract context_management: BetaContextManagementResponse option with get
    abstract model: string with get
    abstract role: string with get
    abstract stop_reason: BetaStopReason option with get
    abstract stop_sequence: string option with get
    abstract ``type``: string with get
    abstract usage: BetaUsage with get
type BetaRawMessageStreamEvent = obj
type MessageParam = obj

[<StringEnum>]
type PermissionBehavior =
    | [<CompiledName("allow")>] Allow
    | [<CompiledName("deny")>] Deny
    | [<CompiledName("ask")>] Ask

[<StringEnum>]
type PermissionMode =
    | [<CompiledName("default")>] Default
    | [<CompiledName("acceptEdits")>] AcceptEdits
    | [<CompiledName("bypassPermissions")>] BypassPermissions
    | [<CompiledName("plan")>] Plan
    | [<CompiledName("delegate")>] Delegate
    | [<CompiledName("dontAsk")>] DontAsk

[<StringEnum>]
type PermissionUpdateDestination =
    | [<CompiledName("userSettings")>] UserSettings
    | [<CompiledName("projectSettings")>] ProjectSettings
    | [<CompiledName("localSettings")>] LocalSettings
    | [<CompiledName("session")>] Session
    | [<CompiledName("cliArg")>] CliArg

[<StringEnum>]
type PermissionUpdateType =
    | [<CompiledName("addRules")>] AddRules
    | [<CompiledName("replaceRules")>] ReplaceRules
    | [<CompiledName("removeRules")>] RemoveRules
    | [<CompiledName("setMode")>] SetMode
    | [<CompiledName("addDirectories")>] AddDirectories
    | [<CompiledName("removeDirectories")>] RemoveDirectories

[<StringEnum>]
type HookEvent =
    | PreToolUse
    | PostToolUse
    | PostToolUseFailure
    | Notification
    | UserPromptSubmit
    | SessionStart
    | SessionEnd
    | Stop
    | SubagentStart
    | SubagentStop
    | PreCompact
    | PermissionRequest
    | Setup

[<StringEnum>]
type ExitReason =
    | [<CompiledName("clear")>] Clear
    | [<CompiledName("logout")>] Logout
    | [<CompiledName("prompt_input_exit")>] PromptInputExit
    | [<CompiledName("other")>] Other
    | [<CompiledName("bypass_permissions_disabled")>] BypassPermissionsDisabled

[<StringEnum>]
type SessionStartSource =
    | [<CompiledName("startup")>] Startup
    | [<CompiledName("resume")>] Resume
    | [<CompiledName("clear")>] Clear
    | [<CompiledName("compact")>] Compact

[<StringEnum>]
type CompactTrigger =
    | [<CompiledName("manual")>] Manual
    | [<CompiledName("auto")>] Auto

[<StringEnum>]
type SetupTrigger =
    | [<CompiledName("init")>] Init
    | [<CompiledName("maintenance")>] Maintenance

[<StringEnum>]
type OutputFormatType = | [<CompiledName("json_schema")>] JsonSchema

[<StringEnum>]
type PresetType = | [<CompiledName("preset")>] Preset

[<StringEnum>]
type PresetName = | [<CompiledName("claude_code")>] ClaudeCode

[<StringEnum>]
type Executable =
    | [<CompiledName("bun")>] Bun
    | [<CompiledName("deno")>] Deno
    | [<CompiledName("node")>] Node

[<StringEnum>]
type AgentModel =
    | [<CompiledName("sonnet")>] Sonnet
    | [<CompiledName("opus")>] Opus
    | [<CompiledName("haiku")>] Haiku
    | [<CompiledName("inherit")>] Inherit

[<StringEnum>]
type SdkBeta = | [<CompiledName("context-1m-2025-08-07")>] Context1M20250807

[<StringEnum>]
type SettingSource =
    | [<CompiledName("user")>] User
    | [<CompiledName("project")>] Project
    | [<CompiledName("local")>] Local

[<StringEnum>]
type SdkPluginType = | [<CompiledName("local")>] Local

[<StringEnum>]
type McpServerType =
    | [<CompiledName("stdio")>] Stdio
    | [<CompiledName("sse")>] Sse
    | [<CompiledName("http")>] Http
    | [<CompiledName("sdk")>] Sdk

[<StringEnum>]
type HookDecision =
    | [<CompiledName("approve")>] Approve
    | [<CompiledName("block")>] Block

[<StringEnum>]
type ApiKeySource =
    | [<CompiledName("user")>] ApiKeyUser
    | [<CompiledName("project")>] ApiKeyProject
    | [<CompiledName("org")>] ApiKeyOrg
    | [<CompiledName("temporary")>] ApiKeyTemporary

// Permissions

type PermissionRuleValue =
    { toolName: string
      ruleContent: string option }

type PermissionUpdateAddRules =
    { ``type``: PermissionUpdateType
      rules: PermissionRuleValue array
      behavior: PermissionBehavior
      destination: PermissionUpdateDestination }

type PermissionUpdateReplaceRules =
    { ``type``: PermissionUpdateType
      rules: PermissionRuleValue array
      behavior: PermissionBehavior
      destination: PermissionUpdateDestination }

type PermissionUpdateRemoveRules =
    { ``type``: PermissionUpdateType
      rules: PermissionRuleValue array
      behavior: PermissionBehavior
      destination: PermissionUpdateDestination }

type PermissionUpdateSetMode =
    { ``type``: PermissionUpdateType
      mode: PermissionMode
      destination: PermissionUpdateDestination }

type PermissionUpdateAddDirectories =
    { ``type``: PermissionUpdateType
      directories: string array
      destination: PermissionUpdateDestination }

type PermissionUpdateRemoveDirectories =
    { ``type``: PermissionUpdateType
      directories: string array
      destination: PermissionUpdateDestination }

[<Erase>]
type PermissionUpdate =
    | PermissionUpdateAddRules of PermissionUpdateAddRules
    | PermissionUpdateReplaceRules of PermissionUpdateReplaceRules
    | PermissionUpdateRemoveRules of PermissionUpdateRemoveRules
    | PermissionUpdateSetMode of PermissionUpdateSetMode
    | PermissionUpdateAddDirectories of PermissionUpdateAddDirectories
    | PermissionUpdateRemoveDirectories of PermissionUpdateRemoveDirectories

type PermissionDecisionAllow =
    { behavior: PermissionBehavior
      updatedInput: IDictionary<string, obj> option
      updatedPermissions: PermissionUpdate array option }

type PermissionDecisionDeny =
    { behavior: PermissionBehavior
      message: string option
      interrupt: bool option }

[<Erase>]
type PermissionDecision =
    | PermissionDecisionAllow of PermissionDecisionAllow
    | PermissionDecisionDeny of PermissionDecisionDeny

type PermissionResultAllow =
    { behavior: PermissionBehavior
      updatedInput: IDictionary<string, obj> option
      updatedPermissions: PermissionUpdate array option
      toolUseID: string option }

type PermissionResultDeny =
    { behavior: PermissionBehavior
      message: string
      interrupt: bool option
      toolUseID: string option }

[<Erase>]
type PermissionResult =
    | PermissionResultAllow of PermissionResultAllow
    | PermissionResultDeny of PermissionResultDeny

type CanUseToolOptions =
    { signal: AbortSignal
      suggestions: PermissionUpdate array option
      blockedPath: string option
      decisionReason: string option
      toolUseID: string
      agentID: string option }

type CanUseTool = string -> obj -> CanUseToolOptions -> Promise<PermissionResult>

// Hook inputs

type BaseHookInput =
    abstract session_id: string with get
    abstract transcript_path: string with get
    abstract cwd: string with get
    abstract permission_mode: string option with get

type PreToolUseHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract tool_name: string with get
    abstract tool_input: obj with get
    abstract tool_use_id: string with get

type PostToolUseHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract tool_name: string with get
    abstract tool_input: obj with get
    abstract tool_response: obj with get
    abstract tool_use_id: string with get

type PostToolUseFailureHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract tool_name: string with get
    abstract tool_input: obj with get
    abstract tool_use_id: string with get
    abstract error: string with get
    abstract is_interrupt: bool option with get

type NotificationHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract message: string with get
    abstract title: string option with get
    abstract notification_type: string with get

type UserPromptSubmitHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract prompt: string with get

type SessionStartHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract source: SessionStartSource with get
    abstract agent_type: string option with get
    abstract model: string option with get

type SessionEndHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract reason: ExitReason with get

type StopHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract stop_hook_active: bool with get

type SubagentStartHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract agent_id: string with get
    abstract agent_type: string with get

type SubagentStopHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract stop_hook_active: bool with get
    abstract agent_id: string with get
    abstract agent_transcript_path: string with get
    abstract agent_type: string with get

type PreCompactHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract trigger: CompactTrigger with get
    abstract custom_instructions: string option with get

type PermissionRequestHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract tool_name: string with get
    abstract tool_input: obj with get
    abstract permission_suggestions: PermissionUpdate array option with get

type UserMessageBase =
    abstract parent_tool_use_id: string option with get
    abstract isSynthetic: bool option with get
    abstract tool_use_result: obj option with get
    abstract session_id: string with get

[<StringEnum>]
type SDKAssistantMessageError =
    | [<CompiledName("authentication_failed")>] AuthenticationFailed
    | [<CompiledName("billing_error")>] BillingError
    | [<CompiledName("rate_limit")>] RateLimit
    | [<CompiledName("invalid_request")>] InvalidRequest
    | [<CompiledName("server_error")>] ServerError
    | [<CompiledName("unknown")>] Unknown

type SDKAssistantMessage =
    abstract ``type``: string with get
    abstract message: BetaMessage with get
    abstract parent_tool_use_id: string option with get
    abstract error: SDKAssistantMessageError option with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKAuthStatusMessage =
    abstract ``type``: string with get
    abstract isAuthenticating: bool with get
    abstract output: string array with get
    abstract error: string option with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKCompactBoundaryMessage =
    abstract ``type``: string with get
    abstract subtype: string with get
    abstract compact_metadata: obj with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKFilesPersistedEvent =
    abstract ``type``: string with get
    abstract subtype: string with get
    abstract files: obj array with get
    abstract failed: obj array with get
    abstract processed_at: string with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKHookProgressMessage =
    abstract ``type``: string with get
    abstract subtype: string with get
    abstract hook_id: string with get
    abstract hook_name: string with get
    abstract hook_event: string with get
    abstract stdout: string with get
    abstract stderr: string with get
    abstract output: string with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKHookResponseMessage =
    abstract ``type``: string with get
    abstract subtype: string with get
    abstract hook_id: string with get
    abstract hook_name: string with get
    abstract hook_event: string with get
    abstract output: string with get
    abstract stdout: string with get
    abstract stderr: string with get
    abstract exit_code: int option with get
    abstract outcome: string with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKHookStartedMessage =
    abstract ``type``: string with get
    abstract subtype: string with get
    abstract hook_id: string with get
    abstract hook_name: string with get
    abstract hook_event: string with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKPartialAssistantMessage =
    abstract ``type``: string with get
    abstract event: BetaRawMessageStreamEvent with get
    abstract parent_tool_use_id: string option with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKPermissionDenial =
    abstract tool_name: string with get
    abstract tool_use_id: string with get
    abstract tool_input: IDictionary<string, obj> with get

type SDKResultError =
    abstract ``type``: string with get
    abstract subtype: string with get
    abstract duration_ms: float with get
    abstract duration_api_ms: float with get
    abstract is_error: bool with get
    abstract num_turns: int with get
    abstract stop_reason: string option with get
    abstract total_cost_usd: float with get
    abstract usage: obj with get
    abstract modelUsage: IDictionary<string, obj> with get
    abstract permission_denials: SDKPermissionDenial array with get
    abstract errors: string array with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKResultSuccess =
    abstract ``type``: string with get
    abstract subtype: string with get
    abstract duration_ms: float with get
    abstract duration_api_ms: float with get
    abstract is_error: bool with get
    abstract num_turns: int with get
    abstract result: string with get
    abstract stop_reason: string option with get
    abstract total_cost_usd: float with get
    abstract usage: obj with get
    abstract modelUsage: IDictionary<string, obj> with get
    abstract permission_denials: SDKPermissionDenial array with get
    abstract structured_output: obj option with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKResultMessage =
    abstract ``type``: string with get
    abstract subtype: string with get
    abstract duration_ms: float with get
    abstract duration_api_ms: float with get
    abstract is_error: bool with get
    abstract num_turns: int with get
    abstract result: string with get
    abstract stop_reason: string option with get
    abstract total_cost_usd: float with get
    abstract usage: obj with get
    abstract modelUsage: IDictionary<string, obj> with get
    abstract permission_denials: SDKPermissionDenial array with get
    abstract errors: string array option with get
    abstract structured_output: obj option with get
    abstract uuid: UUID with get
    abstract session_id: string with get

[<StringEnum>]
type SDKStatus =
    | [<CompiledName("compacting")>] Compacting

type SDKStatusMessage =
    abstract ``type``: string with get
    abstract subtype: string with get
    abstract status: SDKStatus option with get
    abstract permissionMode: PermissionMode option with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKSystemMessage =
    abstract ``type``: string with get
    abstract subtype: string with get
    abstract agents: string array option with get
    abstract apiKeySource: ApiKeySource with get
    abstract betas: string array option with get
    abstract claude_code_version: string with get
    abstract cwd: string with get
    abstract tools: string array with get
    abstract mcp_servers: obj array with get
    abstract model: string with get
    abstract permissionMode: PermissionMode with get
    abstract slash_commands: string array with get
    abstract output_style: string with get
    abstract skills: string array with get
    abstract plugins: obj array with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKTaskNotificationMessage =
    abstract ``type``: string with get
    abstract subtype: string with get
    abstract task_id: string with get
    abstract status: string with get
    abstract output_file: string with get
    abstract summary: string with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKToolProgressMessage =
    abstract ``type``: string with get
    abstract tool_use_id: string with get
    abstract tool_name: string with get
    abstract parent_tool_use_id: string option with get
    abstract elapsed_time_seconds: float with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKToolUseSummaryMessage =
    abstract ``type``: string with get
    abstract summary: string with get
    abstract preceding_tool_use_ids: string array with get
    abstract uuid: UUID with get
    abstract session_id: string with get

type SDKUserMessage =
    inherit UserMessageBase
    abstract ``type``: string with get
    abstract message: MessageParam with get
    abstract uuid: UUID option with get

type SDKUserMessageReplay =
    inherit UserMessageBase
    abstract ``type``: string with get
    abstract message: MessageParam with get
    abstract uuid: UUID with get
    abstract isReplay: bool with get

type SDKMessage =
    abstract ``type``: string with get
    abstract subtype: string option with get
    abstract uuid: UUID option with get
    abstract session_id: string option with get

[<StringEnum>]
type MessageType =
    | [<CompiledName("assistant")>] Assistant
    | [<CompiledName("user")>] User
    | [<CompiledName("result")>] Result
    | [<CompiledName("system")>] System
    | [<CompiledName("auth_status")>] AuthStatus
    | [<CompiledName("stream_event")>] StreamEvent
    | [<CompiledName("tool_progress")>] ToolProgress
    | [<CompiledName("tool_use_summary")>] ToolUseSummary

module MessageType =
    let parse (value: string) =
        match value with
        | "assistant" -> Some MessageType.Assistant
        | "user" -> Some MessageType.User
        | "result" -> Some MessageType.Result
        | "system" -> Some MessageType.System
        | "auth_status" -> Some MessageType.AuthStatus
        | "stream_event" -> Some MessageType.StreamEvent
        | "tool_progress" -> Some MessageType.ToolProgress
        | "tool_use_summary" -> Some MessageType.ToolUseSummary
        | _ -> None

type SDKMessageKind =
    | AssistantKind of SDKAssistantMessage
    | UserKind of SDKUserMessage
    | UserReplayKind of SDKUserMessageReplay
    | ResultKind of SDKResultMessage
    | SystemKind of SDKSystemMessage
    | StatusKind of SDKStatusMessage
    | CompactBoundaryKind of SDKCompactBoundaryMessage
    | HookStartedKind of SDKHookStartedMessage
    | HookProgressKind of SDKHookProgressMessage
    | HookResponseKind of SDKHookResponseMessage
    | ToolProgressKind of SDKToolProgressMessage
    | AuthStatusKind of SDKAuthStatusMessage
    | TaskNotificationKind of SDKTaskNotificationMessage
    | FilesPersistedKind of SDKFilesPersistedEvent
    | ToolUseSummaryKind of SDKToolUseSummaryMessage
    | PartialAssistantKind of SDKPartialAssistantMessage
    | UnknownKind of SDKMessage

let classifyMessage (message: SDKMessage) : SDKMessageKind =
    match MessageType.parse message.``type`` with
    | Some MessageType.Assistant -> AssistantKind (!!message)
    | Some MessageType.User ->
        let replay: SDKUserMessageReplay = !!message
        if replay.isReplay then
            UserReplayKind replay
        else
            UserKind (!!message)
    | Some MessageType.Result -> ResultKind (!!message)
    | Some MessageType.AuthStatus -> AuthStatusKind (!!message)
    | Some MessageType.StreamEvent -> PartialAssistantKind (!!message)
    | Some MessageType.ToolProgress -> ToolProgressKind (!!message)
    | Some MessageType.ToolUseSummary -> ToolUseSummaryKind (!!message)
    | Some MessageType.System ->
        match message.subtype with
        | Some "init" -> SystemKind (!!message)
        | Some "status" -> StatusKind (!!message)
        | Some "compact_boundary" -> CompactBoundaryKind (!!message)
        | Some "hook_started" -> HookStartedKind (!!message)
        | Some "hook_progress" -> HookProgressKind (!!message)
        | Some "hook_response" -> HookResponseKind (!!message)
        | Some "task_notification" -> TaskNotificationKind (!!message)
        | Some "files_persisted" -> FilesPersistedKind (!!message)
        | Some _ -> UnknownKind message
        | None -> UnknownKind message
    | None -> UnknownKind message

type SetupHookInput =
    inherit BaseHookInput
    abstract hook_event_name: HookEvent with get
    abstract trigger: SetupTrigger with get

// Hook outputs

type NotificationHookSpecificOutput =
    { hookEventName: HookEvent
      additionalContext: string option }

type PermissionRequestHookSpecificOutput =
    { hookEventName: HookEvent
      decision: PermissionDecision }

type PostToolUseFailureHookSpecificOutput =
    { hookEventName: HookEvent
      additionalContext: string option }

type PostToolUseHookSpecificOutput =
    { hookEventName: HookEvent
      additionalContext: string option
      updatedMCPToolOutput: obj option }

type PreToolUseHookSpecificOutput =
    { hookEventName: HookEvent
      permissionDecision: PermissionBehavior option
      permissionDecisionReason: string option
      updatedInput: IDictionary<string, obj> option
      additionalContext: string option }

type SessionStartHookSpecificOutput =
    { hookEventName: HookEvent
      additionalContext: string option }

type SetupHookSpecificOutput =
    { hookEventName: HookEvent
      additionalContext: string option }

type SubagentStartHookSpecificOutput =
    { hookEventName: HookEvent
      additionalContext: string option }

type UserPromptSubmitHookSpecificOutput =
    { hookEventName: HookEvent
      additionalContext: string option }

[<Erase>]
type HookSpecificOutput =
    | PreToolUseHookSpecificOutput of PreToolUseHookSpecificOutput
    | UserPromptSubmitHookSpecificOutput of UserPromptSubmitHookSpecificOutput
    | SessionStartHookSpecificOutput of SessionStartHookSpecificOutput
    | SetupHookSpecificOutput of SetupHookSpecificOutput
    | SubagentStartHookSpecificOutput of SubagentStartHookSpecificOutput
    | PostToolUseHookSpecificOutput of PostToolUseHookSpecificOutput
    | PostToolUseFailureHookSpecificOutput of PostToolUseFailureHookSpecificOutput
    | NotificationHookSpecificOutput of NotificationHookSpecificOutput
    | PermissionRequestHookSpecificOutput of PermissionRequestHookSpecificOutput

type AsyncHookJSONOutput =
    { ``async``: bool
      asyncTimeout: float option }

type SyncHookJSONOutput =
    { ``continue``: bool option
      suppressOutput: bool option
      stopReason: string option
      decision: HookDecision option
      systemMessage: string option
      reason: string option
      hookSpecificOutput: HookSpecificOutput option }

[<Erase>]
type HookJSONOutput =
    | AsyncHookJSONOutput of AsyncHookJSONOutput
    | SyncHookJSONOutput of SyncHookJSONOutput

type HookCallbackOptions = { signal: AbortSignal }

type HookCallback = HookInput -> string option -> HookCallbackOptions -> Promise<HookJSONOutput>

and HookInput = obj

/// Hook callback matcher containing hook callbacks and optional pattern matching.
type HookCallbackMatcher =
    { matcher: string option
      hooks: HookCallback array
      timeout: float option }

// MCP server config

type McpStdioServerConfig =
    { ``type``: McpServerType option
      command: string
      args: string array option
      env: IDictionary<string, string> option }

type McpSseServerConfig =
    { ``type``: McpServerType
      url: string
      headers: IDictionary<string, string> option }

type McpHttpServerConfig =
    { ``type``: McpServerType
      url: string
      headers: IDictionary<string, string> option }

type McpSdkServerConfig =
    { ``type``: McpServerType
      name: string }

type McpSdkServerConfigWithInstance =
    { ``type``: McpServerType
      name: string
      instance: McpServer }

[<Erase>]
type McpServerConfig =
    | McpServerStdio of McpStdioServerConfig
    | McpServerSse of McpSseServerConfig
    | McpServerHttp of McpHttpServerConfig
    | McpServerSdk of McpSdkServerConfigWithInstance

[<Erase>]
type McpServerConfigForProcessTransport =
    | McpServerStdioTransport of McpStdioServerConfig
    | McpServerSseTransport of McpSseServerConfig
    | McpServerHttpTransport of McpHttpServerConfig
    | McpServerSdkTransport of McpSdkServerConfig

// Agent and plugin definitions

type AgentMcpServerSpec =
    | AgentMcpServerName of string
    | AgentMcpServerConfig of IDictionary<string, McpServerConfigForProcessTransport>

type AgentDefinition =
    { description: string
      tools: string array option
      disallowedTools: string array option
      prompt: string
      model: AgentModel option
      mcpServers: AgentMcpServerSpec array option
      criticalSystemReminder_EXPERIMENTAL: string option
      skills: string array option
      maxTurns: int option }

type SdkPluginConfig =
    { ``type``: SdkPluginType
      path: string }

// Output format

type JsonSchemaOutputFormat =
    { ``type``: OutputFormatType
      schema: IDictionary<string, obj> }

type OutputFormat = JsonSchemaOutputFormat

// Sandbox settings

type SandboxNetworkConfig =
    { allowedDomains: string array option
      allowUnixSockets: string array option
      allowAllUnixSockets: bool option
      allowLocalBinding: bool option
      httpProxyPort: int option
      socksProxyPort: int option }

type SandboxRipgrepConfig =
    { command: string
      args: string array option }

type SandboxSettings =
    { enabled: bool option
      autoAllowBashIfSandboxed: bool option
      allowUnsandboxedCommands: bool option
      network: SandboxNetworkConfig option
      ignoreViolations: IDictionary<string, string array> option
      enableWeakerNestedSandbox: bool option
      excludedCommands: string array option
      ripgrep: SandboxRipgrepConfig option }

// Spawn

type SpawnedProcess =
    abstract stdin: Writable with get
    abstract stdout: Readable with get
    abstract killed: bool with get
    abstract exitCode: int option with get
    abstract kill: string -> bool
    abstract on: string * obj -> unit
    abstract once: string * obj -> unit
    abstract off: string * obj -> unit

type SpawnOptions =
    { command: string
      args: string array
      cwd: string option
      env: IDictionary<string, string option>
      signal: AbortSignal }

// Query options

type ToolsPreset =
    { ``type``: PresetType
      preset: PresetName }

[<Erase>]
type Tools =
    | ToolsList of string array
    | ToolsPresetValue of ToolsPreset

type SystemPromptPreset =
    { ``type``: PresetType
      preset: PresetName
      append: string option }

[<Erase>]
type SystemPrompt =
    | SystemPromptText of string
    | SystemPromptPresetValue of SystemPromptPreset

type HookMap = IDictionary<HookEvent, HookCallbackMatcher array>

type Options =
    { abortController: AbortController option
      additionalDirectories: string array option
      agent: string option
      agents: IDictionary<string, AgentDefinition> option
      allowedTools: string array option
      canUseTool: CanUseTool option
      ``continue``: bool option
      cwd: string option
      disallowedTools: string array option
      tools: Tools option
      env: IDictionary<string, string option> option
      executable: Executable option
      executableArgs: string array option
      extraArgs: IDictionary<string, string option> option
      fallbackModel: string option
      enableFileCheckpointing: bool option
      forkSession: bool option
      betas: SdkBeta array option
      hooks: HookMap option
      persistSession: bool option
      includePartialMessages: bool option
      maxThinkingTokens: int option
      maxTurns: int option
      maxBudgetUsd: float option
      mcpServers: IDictionary<string, McpServerConfig> option
      model: string option
      outputFormat: OutputFormat option
      pathToClaudeCodeExecutable: string option
      permissionMode: PermissionMode option
      allowDangerouslySkipPermissions: bool option
      permissionPromptToolName: string option
      plugins: SdkPluginConfig array option
      resume: string option
      resumeSessionAt: string option
      sandbox: SandboxSettings option
      settingSources: SettingSource array option
      debug: bool option
      debugFile: string option
      stderr: (string -> unit) option
      strictMcpConfig: bool option
      systemPrompt: SystemPrompt option
      spawnClaudeCodeProcess: (SpawnOptions -> SpawnedProcess) option }

module Options =
    let defaults () : Options =
        { abortController = None
          additionalDirectories = None
          agent = None
          agents = None
          allowedTools = None
          canUseTool = None
          ``continue`` = None
          cwd = None
          disallowedTools = None
          tools = None
          env = None
          executable = None
          executableArgs = None
          extraArgs = None
          fallbackModel = None
          enableFileCheckpointing = None
          forkSession = None
          betas = None
          hooks = None
          persistSession = None
          includePartialMessages = None
          maxThinkingTokens = None
          maxTurns = None
          maxBudgetUsd = None
          mcpServers = None
          model = None
          outputFormat = None
          pathToClaudeCodeExecutable = None
          permissionMode = None
          allowDangerouslySkipPermissions = None
          permissionPromptToolName = None
          plugins = None
          resume = None
          resumeSessionAt = None
          sandbox = None
          settingSources = None
          debug = None
          debugFile = None
          stderr = None
          strictMcpConfig = None
          systemPrompt = None
          spawnClaudeCodeProcess = None }

    let withClaudePath path (options: Options) =
        { options with
            pathToClaudeCodeExecutable = Some path }

    let withModel model (options: Options) = { options with model = Some model }

    let withTools tools (options: Options) = { options with tools = Some tools }

    let withPermissionMode mode (options: Options) =
        { options with
            permissionMode = Some mode }

/// Query parameters
type QueryParams =
    {| prompt: U2<string, IAsyncEnumerable<SDKUserMessage>>
       options: Options option |}

/// AsyncGenerator returned by query
type Query =
    inherit IAsyncEnumerable<SDKMessage>

/// Import the query function from SDK
[<Import("query", "@anthropic-ai/claude-agent-sdk")>]
let query: QueryParams -> Query = jsNative

/// Convert any AsyncIterable/AsyncGenerator to an array.
/// Uses native Array.fromAsync (Node 22+).
[<Emit("Array.fromAsync($0)")>]
let toArray (asyncIterable: 'T) : Promise<'U array> = jsNative

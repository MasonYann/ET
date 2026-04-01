# ET-Centric SDG Status Snapshot

Date: 2026-03-31
Workspace: `E:\ZyProject\Gitee\ETProject\ET8.1-Fork`

## 1. What this repository is right now

This repository is currently an ET 8.1 based client-server project fork with a working Demo gameplay chain:

- Unity client startup scene: `Unity/Assets/Scenes/Init.unity`
- ET client initialization event chain:
  - `FiberInit_Main`
  - `EntryEvent1`
  - `EntryEvent2`
  - `EntryEvent3`
  - `HotfixView/Client/Demo/EntryEvent3_InitClient.cs`
- Client login UI entry:
  - `HotfixView/Client/Demo/UI/DlgLogin/Event/AppStartInitFinish_CreateLoginUI.cs`
- Client login/business chain:
  - `Hotfix/Client/Demo/NetClient/LoginHelper.cs`
  - `Hotfix/Client/Demo/NetClient/Main2NetClient_LoginHandler.cs`
  - `Hotfix/Client/Demo/NetClient/Main2NetClient_LoginGameHandler.cs`
  - `Hotfix/Client/Demo/Main/Login/EnterMapHelper.cs`
  - `Hotfix/Client/Demo/Main/Scene/SceneChangeHelper.cs`
- Scene loading:
  - `HotfixView/Client/Demo/Scene/SceneChangeStart_AddComponent.cs`
- Unit creation and表现层实例化:
  - `Hotfix/Client/Demo/Main/Unit/UnitFactory.cs`
  - `HotfixView/Client/Demo/Unit/AfterUnitCreate_CreateUnitView.cs`

In short: the current repository already has a complete ET Demo runtime loop for startup, login, gate handshake, enter-map, scene change, and test unit visualization.

## 2. What is already aligned with the planned ET-Centric SDG direction

The following parts of the planned architecture already have a useful base in the current codebase:

- ET ECS architecture is already the project core.
- Client/server shared message and entity model already exists.
- Scene switching and unit lifecycle are already event-driven and structured.
- Runtime data flow is already layered by ET concepts:
  - Scene
  - Component
  - System/Event
  - Message handlers
- There is already a workable notion of "atomic gameplay building blocks" at the ET level:
  - Unit
  - Move
  - Numeric
  - Scene transfer
  - AOI
  - UI events
- The repository already contains ET-side references related to AI, behavior-machine thinking, skill/numeric design, and Luban extension materials:
  - `Book/6.1AI框架.md`
  - `Book/6.2AI框架-行为机.md`
  - `Book/5.6数值组件设计.md`
  - `Store/LubanExtension.md`

These are not yet the SDG pipeline itself, but they are valid foundations for it.

## 3. What is not yet found in this repository

As of this snapshot, I did not find repository-level implementation for the following planned modules:

- LangChain orchestration layer
- ET-CLI command line tool as the only business entry
- Unity-MCP bridge/server
- schema validation layer for AI-issued ET instructions
- natural-language-to-gameplay-atom compiler flow
- closed-loop auto-healing chain:
  - LangChain
  - ET-CLI
  - Unity-MCP
  - ET-CLI structured error parsing
  - LangChain regeneration
- dedicated structured intermediate artifacts for SDG such as:
  - DSL
  - JSON gameplay graph
  - Excel-to-runtime generation pipeline specifically for AI production

Conclusion: the "ET-Centric SDG Pipeline" is still in the planning / architecture-definition stage in this repository. The current repo is best understood as the ET runtime base that the future pipeline will build on.

## 4. Current login and scene pipeline status

Current client login flow in this repository is the extended Demo chain, not yet an AI-dev streamlined chain:

1. App init completes and login UI is shown.
2. User clicks login button in `DlgLogin`.
3. `LoginHelper.Login(...)` is called.
4. Client connects Realm through NetClient fiber.
5. Current implementation uses the extended account flow:
   - `C2R_LoginAccount`
   - `R2C_LoginAccount`
   - `C2R_GetServerInfos`
   - `C2R_GetRoles`
   - `C2R_CreateRole` when needed
   - `C2R_GetRealmKey`
   - `C2G_LoginGameGate`
   - `C2G_EnterGame`
6. Login completion publishes `LoginFinish`.
7. Map enter flow runs through `EnterMapHelper.EnterMapAsync()`.
8. Scene changes after `M2C_StartSceneChange`.
9. Client unit is created after `M2C_CreateMyUnit`.

There is also an older simpler message route still present in the repository:

- `C2R_Login`
- `R2C_Login`
- `C2G_LoginGate`
- `G2C_LoginGate`
- `C2G_EnterMap`

So the codebase currently contains both a legacy/simple handshake path and a newer extended account/role/game-gate path.

## 5. Scene status relevant to AI dev mode

Current bundled runtime scenes found under `Unity/Assets/Bundles/Scenes/`:

- `Map1.unity`
- `Map1 1.unity`
- `Map2.unity`
- `Room1.unity`

Observed status:

- Server default enter-game target is hardcoded to `Map1` in multiple handlers.
- There is no scene explicitly named `SkillTest`, `SkillTestEmpty`, or similar.
- For AI-dev fast entry, `Map2` is the closest candidate to serve as a temporary "skill test empty scene" unless a dedicated scene is added later.

## 6. Immediate engineering direction chosen for next step

For the requested AI dev workflow, the practical implementation direction is:

- keep the existing ET runtime chain as the stable base
- add an `isAIDevMode` switch
- hardcode a test account when AI dev mode is enabled
- skip login UI in that mode
- auto-run login and enter-map
- auto-route the player into the temporary test scene target

## 7. Assumptions in this snapshot

- The SDG pipeline design you described is the target architecture, not yet fully implemented in the repository.
- The current repository should be treated as the ET runtime substrate for that future pipeline.
- `Map2` is treated as the temporary "skill test empty scene" until a dedicated scene name is provided or created.

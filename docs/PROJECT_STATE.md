# Project State

## AD-29 - Production gameplay HUD, background, and camera

- `Adventure-Island-2-Game.unity` now uses one production gameplay HUD Canvas with Power, Fruit Progress, and Lives visuals while preserving the existing Controller-to-View contracts.
- `PowerView` keeps the current/max text synchronized and drives the existing horizontal `PowerBar_Fill` Image through `fillAmount`; `LivesView` presents the production `x N` counter without owning gameplay state.
- The production scene includes its approved background, Player/platform presentation, scene Fruit prefabs, and a root-level `Main Camera` with `CameraFollow`; the background stays under the camera while camera rotation remains independent of Player rotation.
- `Ground` and `Background` layers support the production scene setup. Referenced scene art is committed, while unused imported sprite-pack content is intentionally excluded.
- Verification: the generated `Assembly-CSharp`, Power, Fruit Progress, and Lives projects compile successfully; scene component/GUID/reference checks pass. Build Settings still target the legacy scene and final end-to-end production bootstrap verification remains AD-30 scope.

## AD-28 / AD-26 / AD-27 - Shared gameplay composition and Lives flow

- The shared `GameLifetimeScope` now composes `PowerInstaller`, `FruitProgressInstaller`, `LivesInstaller`, and the domain-specific `LivesFlowCoordinator`; installers remain registration/configuration-only.
- `LivesFlowCoordinator` owns instance-event subscriptions and cleanup: `FruitThresholdReached` grants one life, while `PowerReachedMinimum` removes one life and selects the follow-up flow. No static events, forwarding bridge, or generic GameFlow layer is used.
- A surviving life restarts the current attempt by resetting Power and restarting the Power drain interval. Fruit progress is intentionally retained on that path because the assignment does not explicitly require its counter to reset on life loss. Reaching zero Lives performs the full feature-state reset for Lives, Fruit Progress, and Power and starts a fresh drain interval.
- Resettable feature state uses the shared `IResettable.Reset()` contract (bool result) across Power, Fruit Progress, and Lives models/controllers. Drain timing remains an explicit runner lifecycle operation.
- The production scene no longer contains a separate GameFlow composition object; Power, Fruit Progress, and Lives resolve from the single shared scope. Legacy death/teleport code remains outside this architecture, and no player/stage respawn implementation was added because the current production scene has no lifecycle owner for it.
- Verification: `Assembly-CSharp.csproj` built successfully with 0 errors (one unrelated existing `PlayerJump.isJumping` warning), source/scene static checks passed, and no Unity Play Mode run was claimed. Obsolete Power minimum and Lives model/controller test sources and their test assemblies were removed as requested; no automated test source remains for these flows.

## AD-25 - Lives MVC foundation

- Adds a focused Lives MVC slice: `LivesModel` owns initial/current Lives and zero-minimum rules; concrete `LivesController` orchestrates through `ILivesModel` and `ILivesView`; `LivesView` renders temporary TextMeshPro text in the form `Lives: 3`.
- `LivesInstaller` owns Lives VContainer configuration: `LivesModel` is registered as `ILivesModel`, the serialized `LivesView` as `ILivesView`, and `LivesController` as itself. It resolves the Controller to synchronize the initial display.
- The current `LivesInstaller` temporarily retains the prior `LifetimeScope` behavior and delegates through `Install(IContainerBuilder)`, preserving the existing scene without changing shared composition. A future, explicitly approved `GameLifetimeScope` will invoke `LivesInstaller.Install(...)`; that migration must ensure the standalone scope no longer registers Lives a second time.
- The scene adds `Scripts/Lives/LivesLifetimeScope` and root-level `LivesCanvas/Txt_Lives`; the script GUID is preserved through the installer refactor so existing serialized references remain valid. The hierarchy rename and shared-scope connection are deferred.
- No Power subscription, life-loss reaction, respawn, Game Over, stage flow, legacy `SC_Death`, or `ResetPosition` integration is included.
- Verification: Lives production and EditMode test sources compile successfully through the Unity C# compiler; scene IDs and Lives references passed static checks. Unity's batch test runner did not produce results because its local licensing client repeatedly disconnected.

## AD-21 - Fruit collection and Power gain

- Status: integrated into `master` as commit `735f555` and moved to Jira Done on 2026-09-18.
- `FruitPickup` specializes the existing `PickUp` template, uses one configured Power amount, and delegates collection effects to `PowerController.AddPower(...)`.
- Scene-owned Fruit components receive their controllers through the `Fruits` root in `GameLifetimeScope`'s Auto Inject Game Objects list; no `FruitLifetimeScope` or per-fruit registrations are used.
- The unchanged `PickUp` template deactivates a Fruit after every valid Player pickup, including when Power is already at maximum.
- Runtime Fruit creation, a Fruit factory, and Fruit pooling remain deferred.
- Verification: production code and saved-scene structure passed static checks. Final gameplay trigger behavior was not runtime-confirmed because the temporary test Player's `Rigidbody2D` had `Simulated` disabled; Eden explicitly approved the current implementation after that diagnosis.

## AD-23 - Reset and initialize Power for stage/life start

- Adds `void ResetPower()` to `IPowerModel`, implemented by `PowerModel` by restoring its validated `InitialPower`.
- Adds `PowerController.ResetPower()`, which commands the Model and synchronizes the existing `IPowerView`.
- No lifecycle, lives, respawn, stage reload, event, or scene behavior is part of AD-23.
- Verified by successful Power and game-assembly builds and an in-memory check of construction, Model reset, and Controller/View synchronization.
- Future dependency: when the Lives / GameFlow system exists, `PowerController` will publish `PowerReachedMinimum`; its coordinator will handle life loss and call `PowerController.ResetPower()` only when a new attempt begins. Power remains independent of lives and restart behavior.

## AD-22 - Power-minimum notification

- Status: completed and integrated into `master` on 2026-09-18.
- `IPowerModel` exposes its configured `MinimumPower`, allowing `PowerController` to compare it with `CurrentPower` after a successful reduction.
- `PowerController.PowerReachedMinimum` notifies external observers only after the View reflects a transition to minimum Power. It does not fire at construction or for repeated reductions already at the minimum, and can notify again after Power rises and is depleted.
- Focused EditMode tests cover ordinary, exact, clamped, repeated, initial-minimum, replenished, nonzero-minimum, and View-before-notification cases. The Power and test assemblies compile successfully.
- No Lives, respawn, reset trigger, scene, or subscriber is introduced. A future Lives / GameFlow coordinator owns subscribing and reacting to this notification.

## AD-24 - Fruit Progress MVC

- Status: approved complete; integration and Jira completion are handled through the task-transition workflow.
- `GameLifetimeScope` is the one scene-wide VContainer scope. It invokes `PowerInstaller` and `FruitProgressInstaller`, keeping registration/configuration owned by each feature while allowing `FruitPickup` to receive both concrete Controllers in one injection pass.
- `FruitProgressModel` owns count and threshold behavior. Each Fruit increments the count once, and the twentieth Fruit resets it to `0` for the next group.
- `FruitProgressController` updates `FruitProgressView`, publishes the non-static `FruitThresholdReached` notification when a group completes, then publishes the non-static `FruitCollected` notification. It does not remove lives or own any restart/GameFlow behavior.
- `FruitPickup` receives `PowerController` and the shared `FruitProgressController`, then commands `PowerController.AddPower(powerAmount)` and `FruitProgressController.CollectFruit()` in that order. `FruitCollected` is for future optional reactions; required Power and Fruit Progress behavior does not use event subscriptions.

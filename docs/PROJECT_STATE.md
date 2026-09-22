# Project State

## AD-29 - Production gameplay HUD and scene presentation

- The production scene uses one gameplay HUD canvas for the existing Power, Fruit Progress, and Lives Views; presentation continues through the existing Controller-to-View contracts without duplicating gameplay state.
- `PowerView` keeps the current/max text synchronized and drives the existing horizontal `PowerBar_Fill` Image through `fillAmount`; `LivesView` presents the production `x N` counter without owning gameplay state.
- The production scene includes its approved background, Player/platform presentation, scene Fruit prefabs, and a root-level `Main Camera` with `CameraFollow`; the background stays under the camera while camera rotation remains independent of Player rotation.
- `Ground` and `Background` layers support the production scene setup. Referenced scene art is committed, while unused imported sprite-pack content is intentionally excluded and preserved separately from the task commit.
- Verification: the generated `Assembly-CSharp`, Power, Fruit Progress, and Lives projects compile successfully; scene component/GUID/reference checks pass. Build Settings still target the legacy scene and final end-to-end production bootstrap verification remains AD-30 scope.

## AD-35 - Animals mounting and active-animal foundation

- `PlayerAnimalMount` owns exactly one active, mounted `IAnimal`. It parents a Factory-created animal `MonoBehaviour` to the Player, deactivates and destroys the prior instance on replacement, and performs the same cleanup on `ClearActiveAnimal()`.
- `IAnimal.Attack()` is the shared capability boundary. `PlayerAnimalMount.AttackActiveAnimal()` delegates directly to the active animal without Blue/Red/Green checks; concrete attack implementations remain deferred.
- `AnimalPickup` remains generic over `AnimalDefinition` and requests creation through `AnimalFactory`. The Builder/Director/Factory flow remains `AnimalDefinition -> AnimalBuilder -> AnimalDirector -> AnimalFactory` and VContainer instantiates the configured prefab.
- `AnimalsInstaller` registers `AnimalBuilder` as `IAnimalBuilder`, plus `AnimalDirector` and `AnimalFactory`, with scoped lifetime. `GameLifetimeScope` invokes it, and the production scene contains `Scripts/Animals` with the serialized installer reference.
- `AnimalDefinition` is an abstract configuration base with minimal Blue, Red, and Green configuration subtypes; no attack logic or unsupported stats live in definitions.
- Focused EditMode sources cover Builder/Factory creation plus mount, replacement, clear, and attack delegation. `Assembly-CSharp` and `AdventureIsland.Animals.Tests` built successfully. The Unity batch test runner could not produce test results while the local editor process was active, so no Play Mode or executed Unity-test claim is made.

## AD-31 - Production weapon foundation and attack flow

- The production Combat assembly defines the minimal `IWeapon.TryAttack()` capability and a concrete `WeaponController` that directly owns one active `IWeapon`, replaces it on equip, and delegates attacks without weapon-specific branching. The redundant `WeaponLoadout` was removed during AD-32.
- AD-32 replaces the weapon-only input with `PlayerAttackInput` -> `PlayerAttackController` -> the explicitly assigned `IAttackSource`. The controller stores only that source and delegates one attack; it has no weapon fallback or mounted-attack priority rule. The gameplay caller that handles equip/mount transitions must assign `WeaponController` or a mounted source and explicitly restore the weapon source after dismount. That switching flow is not yet wired into a production scene.
- AD-32 keeps accumulated Hammer throws in a plain `HammerWeapon`. `HammerPickup` passes the scoped Hammer to `PlayerWeaponCollector`, which calls the weapon's `ICollectibleWeapon.Collect()` behavior and then equips it through `WeaponController`; replacing the active weapon does not erase its count. `HammerWeapon` uses `ProjectileProvider<HammerProjectileDirector>` to obtain a configured projectile and consumes one throw only after that projectile launches successfully.
- `HammerWeapon` receives the player-specific Hammer spawn-point Transform through composition and supplies it on each `ProjectileProvider<HammerProjectileDirector>.GetReady(spawnPoint)` call. The Provider acquires an unconfigured instance from the non-generic `ProjectilePool`, invokes its Hammer Director for both new and reused projectiles, and releases the instance on configuration failure. The Pool owns one prefab, available and leased instances, self-release binding, reset, and disposal; it asks the non-generic `ProjectileFactory` to instantiate only when no instance is available. The Factory only instantiates and returns a projectile. The Director keeps only its concrete `HammerProjectileBuilder` and coordinates configuration of the actual `HammerProjectile`; there is no intermediate request, spawn, or launch product. `HammerProjectile` owns its serialized movement parameters and fixed lifetime, reads the spawn point when launching, and owns its Rigidbody2D movement. Boomerang return behavior, damage, HUD, and Animals remain deferred.
- `WeaponInstaller` holds serialized Hammer construction inputs and registers the plain Hammer weapon, `PlayerWeaponCollector`, `HammerProjectileDirector`, its concrete `HammerProjectileBuilder`, non-generic `ProjectileFactory`, non-generic `ProjectilePool`, and `ProjectileProvider<HammerProjectileDirector>`. `GameLifetimeScope` remains composition-only. `Adventure-Island-2-Game.unity` already hosts `Scripts/Combat/WeaponInstaller` and references it from `GameLifetimeScope`, but the Hammer projectile prefab and dedicated player Hammer spawn-point references are intentionally unwired pending Player changes. Player/pickup input injection and runtime Hammer scene integration are not yet verified; no scene changes are part of this reconciliation.
- Verification: focused compilation and in-memory behavior checks passed for equip, replacement, empty attack, delegation result, scoped registrations, and the injection contract; Combat assembly JSON, Unity metadata GUID uniqueness, and diff whitespace also passed.

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

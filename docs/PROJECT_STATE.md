# Project State

## AD-48 - Stage 1 production-content completion

- Preserved the approved Stage 1 layout. `Stage1Root` now explicitly owns the existing Fruit, WeaponPickups, AnimalPickups, world/hazard, Spawn, and Goal branches, so a Stage 1 restart cannot leak these collectible objects into another stage.
- Existing `PickUp` reset behavior restores Fruit, weapon, and animal pickups through the generic stage reset pass. Rock and Bonfire now also implement `IStageResettable`, restoring their own active state after a Stage 1 restart; Abyss has no mutable state to restore.
- No new level geometry, managers, enemies, prefabs, gameplay systems, or StageFlow responsibilities were introduced.

## AD-47 - Production stage-flow foundation

- `Adventure-Island-2-Game.unity` uses one production scene with `Stage1Root` and `Stage2Root`; shared Player, camera, HUD, installers, and controllers remain outside stage roots. Each root owns its serialized Spawn, Goal trigger, and only stage-local resettable descendants.
- `StageFlowController` initializes Stage 1, restarts only the current stage, transitions once from Stage 1 to Stage 2, restores both roots on a full reset, and records final Stage 2 completion. It coordinates existing focused owners for player position, Power/drain, Fruit Progress, Weapon, and active Animal without owning their internal rules.
- `LivesFlowCoordinator` remains limited to Power/Fruit event and Lives decisions. A surviving life calls `RestartCurrentStage()`; zero lives resets Lives and requests `ResetToFirstStage()`.
- `StageRoot.ResetStageState()` performs one generic pass over stage-local `IStageResettable` components. `PickUp` implements that contract to reactivate itself; no generic reset manager, scene reload, or legacy `SC_Death` / `ResetPosition` path is used.
- Verification: `Assembly-CSharp.csproj` built with 0 warnings and 0 errors; source and scene static checks confirm both roots, Spawn/Goal references, auto-injection roots, and the approved stage-flow call paths. No AD-48/AD-49 level content was saved or committed in this workstream.

## AD-43 - Enemy foundation and respawn lifecycle (complete)

- `AdventureIsland.Enemies` provides the reusable typed construction path: `EnemyFactory<TEnemy> -> EnemyDirector<TEnemy> -> IEnemyBuilder<TEnemy> -> concrete builder`. Position and rotation flow through every step, so `Enemy.Awake()` captures the instantiated object's real spawn transform.
- `Enemy` owns the fixed, non-overridable `TryDie()` lifecycle: it rejects duplicate requests, invokes `OnDeathStarted()`, waits the serialized per-prefab delay, restores the captured transform, restores `IsAlive`, and invokes `OnRespawned()`. The base class intentionally has no arbitrary non-zero delay default; future concrete enemy prefabs configure their own value.
- `EnemiesInstaller` registers scoped open-generic Factory and Director services through the shared `GameLifetimeScope`; concrete enemy builders remain future concrete-enemy composition work.
- Focused construction and lifecycle test sources compile. Automated Unity Test Runner runtime execution remains unconfirmed: batch mode returned successfully but produced no test-results XML or execution summary. Do not treat the runtime assertions as passed until they are run in the Unity Editor.

## AD-30 - Production bootstrap and scene wiring (complete)

- `ProjectSettings/EditorBuildSettings.asset` now uses `Adventure-Island-2-Game.unity` as the enabled build entry; legacy `Scene_Physics.unity` remains reference content and is not deleted.
- The production `GameLifetimeScope` composes the existing Power, Fruit Progress, Lives, Combat, Animals, and Reset installers. The Player is now included in VContainer auto-injection and hosts the existing `WeaponAttackInput` and `PlayerAnimalMount` components.
- Existing HUD, Main Camera/CameraFollow, background, Fruit injection root, and installer references remain serialized in the production scene. No new gameplay rules, weapons, animal pickups, or lifecycle abstractions were introduced.
- Verification: project sources compiled with 0 errors, scene component/GUID/reference checks passed, and the legacy `Reset()` API has no remaining usages. No fresh Unity Play Mode run was performed during closeout.
- `LivesFlowCoordinator` now separates the two reset flows after Power reaches its minimum. With lives remaining, it uses `IPlayerResetter` to reset only the existing Player's spawn/velocity, Power, and the Power drain interval. At zero lives, it also uses `IPickupResetter` to reactivate the existing `PickUp` instances under the production `Fruits` root and resets Lives and Fruit Progress. It does not reload the scene or instantiate replacements.

## AD-29 - Production gameplay HUD and scene presentation

- The production scene uses one gameplay HUD canvas for the existing Power, Fruit Progress, and Lives Views; presentation continues through the existing Controller-to-View contracts without duplicating gameplay state.
- `PowerView` keeps the current/max text synchronized and drives the existing horizontal `PowerBar_Fill` Image through `fillAmount`; `LivesView` presents the production `x N` counter without owning gameplay state.
- The production scene includes its approved background, Player/platform presentation, scene Fruit prefabs, and a root-level `Main Camera` with `CameraFollow`; the background stays under the camera while camera rotation remains independent of Player rotation.
- `Ground` and `Background` layers support the production scene setup. Referenced scene art is committed, while unused imported sprite-pack content is intentionally excluded and preserved separately from the task commit.
- Verification: the generated `Assembly-CSharp`, Power, Fruit Progress, and Lives projects compile successfully; scene component/GUID/reference checks pass. Build Settings still targeted the legacy scene at AD-29 closeout; AD-30 subsequently switches the enabled entry to production.

## AD-36 - Blue animal and tail attack (complete)

- `HeartPickup` specializes generic `AnimalPickup<BlueAnimal>`. The scoped VContainer construction path is `HeartPickup -> PlayerAnimalCollector<BlueAnimal> -> AnimalFactory<BlueAnimal> -> AnimalDirector<BlueAnimal> -> IAnimalBuilder<BlueAnimal> -> BlueAnimalBuilder -> BlueAnimal`.
- `PlayerActiveAnimal` remains the non-generic mount owner: it parents the created animal to Player, replaces and destroys the old mounted instance deterministically, and performs the same cleanup when cleared.
- `Animal` provides the shared `TryAttack()` Template Method. `BlueAnimal` owns its tail behavior: an explicit serialized tail `BoxCollider2D`, a 0.1-second active window, and an independent 0.5-second cooldown. No Rock, Enemy, Bonfire, Red, or Green behavior was introduced.
- `AnimalAttackSource` implements the existing Combat `IAttackSource` boundary. It registers itself as PlayerAttackController's override only while an animal is active, so weapon behavior remains the default source and shared Combat code contains no animal-type branching.
- The production scene serializes the Blue and Heart prefabs, `AnimalsInstaller`, `PlayerActiveAnimal`, and `AnimalAttackSource`; `GameLifetimeScope` continues to own the shared composition.
- Verification: `Assembly-CSharp.csproj` and `AdventureIsland.Animals.csproj` build with 0 errors and 0 warnings. The prior focused Animals test source was removed by explicit approval, and Unity Test Runner / Play Mode was not claimed.
- Deferred: AD-56 will verify the live Weapons-and-Animals attack-source integration; it must not add target-specific hit effects unless separately assigned.

## AD-37 / AD-38 / AD-39 - Red and Green animals with shared replacement flow

- `LeafPickup` and `StarPickup` reuse the generic Animal collection/construction path to create Red and Green mounts without animal-type branching in the player collection flow.
- `RedAnimal` owns its fire attack presentation and delegates projectile preparation to the existing production `ProjectileProvider<RedFireProjectileDirector>` path. The pooled `RedFireProjectile` owns its movement, lifetime, launch, and reset behavior; it contains no target-specific damage rules.
- `GreenAnimal` owns its spin timing and visual sequence. Its serialized `Collider2D` reference points to the disabled `CircleCollider2D` on the prefab's `SpinHitbox` child, which is enabled only during the attack window and disabled afterward or on disable.
- Blue, Red, and Green all replace the active mount through `PlayerActiveAnimal`; `AnimalAttackSource` remains the single override of the default `WeaponController` attack source while a mount is active.
- Manual Unity verification covered Heart/Leaf/Star collection, all three attacks and visuals, Green's circular attack hitbox, replacement between animals, and fallback to weapon attacks after dismount. Production assemblies and Unity serialization checks also passed. Rock/Enemy/Bonfire interaction rules remain outside this slice.

## AD-35 - Animals mounting and active-animal foundation

- The mounting and DI foundation is now represented by the concrete AD-36 flow above: the shared scope owns the installer, `PlayerActiveAnimal` owns one mounted runtime `IAnimal`, and construction remains Builder/Director/Factory based without animal-type branching in clients.

## AD-31 - Production weapon foundation and attack flow

- The production Combat assembly defines the minimal `IWeapon.TryAttack()` capability and a concrete `WeaponController` that directly owns one active `IWeapon`, replaces it on equip, and delegates attacks without weapon-specific branching. The redundant `WeaponLoadout` was removed during AD-32.
- AD-32 replaces the weapon-only input with `PlayerAttackInput` -> `PlayerAttackController` -> `IAttackSource`. `WeaponController` is configured as the default source; `AnimalAttackSource` supplies a temporary override while a mount is active, and clearing that override automatically falls back to the equipped weapon. The controller contains no weapon/animal type branching and does not fall back to a weapon when an active animal attack returns false.
- AD-32 keeps accumulated Hammer throws in a plain `HammerWeapon`. `HammerPickup` passes the scoped Hammer to `PlayerWeaponCollector`, which calls the weapon's `ICollectibleWeapon.Collect()` behavior and then equips it through `WeaponController`; replacing the active weapon does not erase its count. `HammerWeapon` uses `ProjectileProvider<HammerProjectileDirector>` to obtain a configured projectile and consumes one throw only after that projectile launches successfully.
- `HammerWeapon` receives the player-specific Hammer spawn-point Transform through composition and supplies it on each `ProjectileProvider<HammerProjectileDirector>.GetReady(spawnPoint)` call. The Provider acquires an unconfigured instance from the non-generic `ProjectilePool`, invokes its Director for both new and reused projectiles, and releases the instance on configuration failure. The Pool owns one prefab, available and leased instances, self-release binding, reset, and disposal; it asks the non-generic `ProjectileFactory` to instantiate only when no instance is available. The Factory only instantiates and returns a projectile. Each projectile Director depends on the shared `IProjectileBuilder` build-step contract, while composition supplies the matching concrete Hammer or Boomerang Builder. Builders configure the actual projectile; there is no intermediate request, spawn, or launch product.
- AD-33 adds a reusable collectible `BoomerangWeapon`. Its projectile owns serialized outward distance/speed, return speed, catch radius, safety timeout, live-player return targeting, and clean pooled-state reset. The weapon prevents overlapping throws but consumes no ammo. `WeaponInstaller` registers separate Hammer and keyed Boomerang pool/provider construction paths while retaining one non-generic Factory. `GameLifetimeScope` remains unchanged and composition-only.
- `Adventure-Island-2-Game.unity` hosts `Scripts/Combat/WeaponInstaller`, wired Hammer/Boomerang projectile prefabs and dedicated player spawn points, pickup objects, and the shared Weapons/Animals attack-source integration. Manual Unity verification confirmed both weapon attacks and default/override switching with mounted animals. AD-34 still requires its dedicated repeated pool-reuse/stress verification before projectile lifecycle work is considered complete.
- Verification: focused compilation and in-memory behavior checks passed for equip, replacement, empty attack, delegation result, scoped registrations, and the injection contract; Combat assembly JSON, Unity metadata GUID uniqueness, and diff whitespace also passed.

## AD-28 / AD-26 / AD-27 - Shared gameplay composition and Lives flow

- The shared `GameLifetimeScope` now composes `PowerInstaller`, `FruitProgressInstaller`, `LivesInstaller`, and the domain-specific `LivesFlowCoordinator`; installers remain registration/configuration-only.
- `LivesFlowCoordinator` owns instance-event subscriptions and cleanup: `FruitThresholdReached` grants one life, while `PowerReachedMinimum` removes one life and selects the follow-up flow. No static events, forwarding bridge, or generic GameFlow layer is used.
- A surviving life restarts the current attempt by resetting Power and restarting the Power drain interval. Fruit progress is intentionally retained on that path because the assignment does not explicitly require its counter to reset on life loss. Reaching zero Lives performs the full feature-state reset for Lives, Fruit Progress, and Power and starts a fresh drain interval.
- Resettable feature state uses the shared `IResettable.ResetState()` contract (bool result) across Power, Fruit Progress, and Lives models/controllers. `PowerDrainRunner` also implements that contract to restart its existing drain interval.
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

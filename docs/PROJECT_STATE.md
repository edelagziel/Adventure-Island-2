# Project State

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

## AD-24 - Fruit Progress MVC

- Status: approved complete; integration and Jira completion are handled through the task-transition workflow.
- `GameLifetimeScope` is the one scene-wide VContainer scope. It invokes `PowerInstaller` and `FruitProgressInstaller`, keeping registration/configuration owned by each feature while allowing `FruitPickup` to receive both concrete Controllers in one injection pass.
- `FruitProgressModel` owns count and threshold behavior. Each Fruit increments the count once, and the twentieth Fruit resets it to `0` for the next group.
- `FruitProgressController` updates `FruitProgressView`, publishes the non-static `FruitThresholdReached` notification when a group completes, then publishes the non-static `FruitCollected` notification. It does not remove lives or own any restart/GameFlow behavior.
- `FruitPickup` receives `PowerController` and the shared `FruitProgressController`, then commands `PowerController.AddPower(powerAmount)` and `FruitProgressController.CollectFruit()` in that order. `FruitCollected` is for future optional reactions; required Power and Fruit Progress behavior does not use event subscriptions.

# Project State

## AD-21 - Fruit collection and Power gain

- Status: integrated into `master` as commit `735f555` and moved to Jira Done on 2026-09-18.
- `FruitPickup` specializes the existing `PickUp` template, uses one configured Power amount, and delegates collection effects to `PowerController.AddPower(...)`.
- Scene-owned Fruit components receive `PowerController` through the `Fruits` root in `PowerLifetimeScope`'s Auto Inject Game Objects list; no `FruitLifetimeScope` or per-fruit registrations are used.
- The unchanged `PickUp` template deactivates a Fruit after every valid Player pickup, including when Power is already at maximum.
- Runtime Fruit creation, a Fruit factory, and Fruit pooling remain deferred.
- Verification: production code and saved-scene structure passed static checks. Final gameplay trigger behavior was not runtime-confirmed because the temporary test Player's `Rigidbody2D` had `Simulated` disabled; Eden explicitly approved the current implementation after that diagnosis.

## AD-23 - Reset and initialize Power for stage/life start

- Status: claimed and moved to Jira In Progress on 2026-09-18; planning only, with no implementation approved yet.
- Branch: `feature/AD-23-power-reset`, reusing the existing persistent worktree.
- Current constraint: `Adventure-Island-2-Game.unity` has no production life/restart coordinator. `SC_Death` and `ResetPosition` are legacy `Scene_Physics.unity` behavior and only teleport the player.
- The reset trigger owner and its dependency on the parallel AD-22 life-loss integration remain unresolved pending Eden's approval.

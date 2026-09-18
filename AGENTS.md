# Adventure Island 2 - Agent Context

## Current project status

- Jira tasks `AD-19` (Power MVC foundation), `AD-20` (automatic Power drain), and `AD-21` (Fruit collection and Power gain) are complete in Jira and integrated into `master`.
- Latest completed feature commit: `735f555 feat(AD-21): add fruit power pickup`.
- VContainer `1.19.0` is installed and used for dependency injection.
- The current Power configuration is:
  - initial Power: `20`
  - minimum Power: `0`
  - maximum Power: `30`
- AD-23 adds the Power-side `ResetPower()` capability. The future lifecycle owner and reset timing remain outside the Power feature.
- The current Power-drain configuration is:
  - drain amount: `1`
  - drain interval: `5` seconds of scaled gameplay time

## Approved Power MVC architecture

Use the controller-centric flow below. Do not silently replace it with another MVC variation.

```text
Gameplay systems
        |
        v
PowerController
       / \
      v   v
IPowerModel  IPowerView
      ^           ^
      |           |
PowerModel    PowerView
```

Runtime flow:

```text
Gameplay request
-> PowerController
-> IPowerModel operation
-> operation result / current state
-> PowerController
-> IPowerView.UpdatePowerDisplay(...)
```

Responsibilities:

- `PowerModel` implements `IPowerModel` and owns Power state, validation, minimum/maximum rules, clamping, and mutation behavior.
- `PowerController` is a concrete plain C# orchestration object. It depends on `IPowerModel` and `IPowerView`, calls the Model, updates the View after `ResetPower()`, and updates the View after `AddPower(...)` or `ReducePower(...)` only when the Model reports a change.
- `PowerView` is a Unity `MonoBehaviour` that implements `IPowerView` and only updates presentation.
- Gameplay code must send Power changes through the concrete `PowerController`; it must not mutate `PowerModel` directly.
- Do not introduce `IPowerController`, `IPowerModifier`, an event chain between Controller and View, or another Power service without explicit architectural approval from Eden.

## Power composition and files

The Power feature is self-contained under:

```text
Assets/Scripts/Power/
  AdventureIsland.Power.asmdef
  Composition/PowerLifetimeScope.cs
  Controllers/PowerController.cs
  Interfaces/IPowerModel.cs
  Interfaces/IPowerView.cs
  Models/PowerModel.cs
  Timing/PowerDrainRunner.cs
  Views/PowerView.cs
```

`PowerLifetimeScope` is the topic-specific Composition Root. It registers:

- `PowerModel` as `IPowerModel` with scoped lifetime.
- the serialized `PowerView` component as `IPowerView`.
- `PowerController` as itself with scoped lifetime.
- the serialized `PowerDrainRunner` component with its configured drain amount and interval.
- a build callback that resolves the Controller so the initial UI is synchronized.

Keep future feature registrations in their own topic-specific LifetimeScopes. Do not grow one generic `GameLifetimeScope` containing unrelated systems.

## AD-20 Power drain architecture

`PowerDrainRunner` is a sealed Unity `MonoBehaviour` and a gameplay consumer of the existing Power MVC system. It is not another MVC component or a Power service.

Runtime flow:

```text
PowerDrainRunner coroutine
-> wait for the configured scaled-time interval
-> PowerController.ReducePower(drainAmount)
-> IPowerModel.ReducePower(...)
-> PowerController updates IPowerView only when Power changed
```

Responsibilities and lifecycle:

- VContainer injects the concrete `PowerController`, drain amount, and interval through `PowerDrainRunner.Construct(...)`.
- The runner waits before the first drain and then requests one reduction after every configured interval using `WaitForSeconds`.
- It owns one coroutine, prevents duplicate starts, stops that coroutine when disabled, and starts a fresh interval when re-enabled.
- It keeps scheduling at minimum Power so a future Power increase can begin draining without another event or dependency.
- `PowerModel` remains the only owner of validation, mutation, and minimum clamping.
- Pause behavior, lives, death, respawn, game over, fruit behavior, and stage transitions are outside AD-20.
- Do not replace the coroutine with `Task`, `ITickable`, an event chain, or a new interface without explicit architectural approval from Eden.

## Scene integration

- `Assets/Scenes/Adventure-Island-2-Game.unity` contains:
  - a root-level `Scripts` organization GameObject
  - a `Power` GameObject under `Scripts`
  - separate `PowerLifetimeScope` and `PowerDrainRunner` GameObjects under `Scripts/Power`
  - `PowerCanvas`
  - `Txt_Power`
  - `PowerView`
  - serialized references connecting the View, the separate drain runner component, and LifetimeScope
- The initial display is `Power: 20/30`.
- `Assets/Scenes/Scene_Physics.unity` is the older Mario-based scene and is considered legacy/reference content.
- Build Settings currently still list `Scene_Physics.unity`; changing the main build scene is a separate explicit task.

## AD-21 Scene Fruit architecture

### Current / implemented

- AD-21 reuses `PickUp` as the shared pickup Template Method.
- `FruitPickup` is one configurable component: Fruit Type 1 grants `+1` Power and Fruit Type 2 grants `+2` Power.
- A valid Player pickup always consumes/deactivates the Fruit.
- `FruitPickup` delegates Power changes to `PowerController`.
- Scene Fruit components are Unity-owned and receive `PowerController` through VContainer `Auto Inject Game Objects` on the existing `PowerLifetimeScope`.
- `FruitLifetimeScope`, per-fruit DI registrations, and Fruit pooling are intentionally not used.

### Next planned step

- Runtime-created Fruit will use a Factory that instantiates Fruit prefabs through VContainer so `FruitPickup` is injected automatically.
- That creation path may later support level/Tiled Fruit and enemy-drop Fruit; it is intentionally not implemented yet.

## Future Lives / GameFlow integration

This integration is intentionally outside AD-23 and must be revisited when the Lives / GameFlow system is implemented.

- `PowerController` will publish `PowerReachedMinimum` for the future Lives / GameFlow integration.
- A future Lives / GameFlow coordinator will subscribe, remove one life, and determine whether lives remain.
- When lives remain, that coordinator will restart the current stage attempt; when none remain, it will reset the game according to the assignment rules.
- When a new attempt actually begins, that coordinator will call `PowerController.ResetPower()`.
- Power must not manage lives, respawn, stage restart, or game-over logic.
- `PowerReachedMinimum` is an external notification for the future coordinator, not an event chain between `PowerController` and `IPowerView`.

## Verification completed

- `AdventureIsland.Power.csproj` builds successfully with `0` errors and `0` warnings.
- Unity imported and loaded `Adventure-Island-2-Game.unity` successfully after the Power integration.
- Scene references were checked: `powerView` and `powerText` are assigned and the added scene objects have no duplicate file IDs.
- Automated Power tests were intentionally removed at Eden's request. Do not claim automated test coverage for AD-19.
- For AD-20, a clean Unity `6000.3.21f1` import compiled `AdventureIsland.Power.dll` with no compiler errors, invalid GUIDs, or missing scripts.
- The Unity-generated `AdventureIsland.Power.csproj` built with `0` errors; its two warnings came from the Unity Test Framework dependency rather than AD-20 code.
- The AD-20 scene changes have no duplicate file IDs, and `Assets/Scenes/Scene_Physics.unity` remains untouched in the feature branch.
- No automated tests were added for AD-20, following Eden's manual-playtest decision.
- Retain the recorded distinction between compile/static verification and manual Play Mode verification; do not retroactively claim automated coverage for AD-20.
- AD-23 was verified by successful `AdventureIsland.Power.csproj` and `Assembly-CSharp.csproj` builds, plus an in-memory Model/Controller/View reset check. No scene or runtime lifecycle verification was added.

## Known working-tree note

Unity automatically rewrote thousands of serialization lines in the legacy `Assets/Scenes/Scene_Physics.unity` after it was opened with the current Unity version. This change is unrelated to AD-19, was not committed, and must not be included in a future feature commit without deliberate review and approval.

## Next development point

The current production scene has no real player life/restart coordinator; the legacy `SC_Death` / `ResetPosition` flow only teleports the player and must not silently become the production lifecycle architecture.

Before implementing a future Lives / GameFlow slice, define and approve the owner of attempt restart and the exact `PowerReachedMinimum` publication contract.

For architecture work, follow the global `eden-software-architecture-coach` skill and the lecturer materials it references.

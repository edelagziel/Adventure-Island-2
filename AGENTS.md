# Adventure Island 2 - Agent Context

## Current project status

- Jira task `AD-19` (Power MVC foundation) is complete and Eden intends to move it to `Done`.
- The implementation is committed and pushed to the `master` branch.
- Latest completed feature commit: `9909a30 feat: integrate Power system into game scene`.
- VContainer `1.19.0` is installed and used for dependency injection.
- The current Power configuration is:
  - initial Power: `20`
  - minimum Power: `0`
  - maximum Power: `30`
- Jira task `AD-20` (automatic Power drain) is implemented on branch `feature/AD-20-power-drain`.
- AD-20 still requires its manual Unity Play Mode verification before it can be declared complete.
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
- `PowerController` is a concrete plain C# orchestration object. It depends on `IPowerModel` and `IPowerView`, calls the Model, and updates the View only when the Model reports a change.
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
  - a dedicated `PowerLifetimeScope` GameObject
  - `PowerDrainRunner` on the same GameObject
  - `PowerCanvas`
  - `Txt_Power`
  - `PowerView`
  - serialized references connecting the View, drain runner, and LifetimeScope
- The initial display is `Power: 20/30`.
- `Assets/Scenes/Scene_Physics.unity` is the older Mario-based scene and is considered legacy/reference content.
- Build Settings currently still list `Scene_Physics.unity`; changing the main build scene is a separate explicit task.

## Verification completed

- `AdventureIsland.Power.csproj` builds successfully with `0` errors and `0` warnings.
- Unity imported and loaded `Adventure-Island-2-Game.unity` successfully after the Power integration.
- Scene references were checked: `powerView` and `powerText` are assigned and the added scene objects have no duplicate file IDs.
- Automated Power tests were intentionally removed at Eden's request. Do not claim automated test coverage for AD-19.
- For AD-20, a clean Unity `6000.3.21f1` import compiled `AdventureIsland.Power.dll` with no compiler errors, invalid GUIDs, or missing scripts.
- The Unity-generated `AdventureIsland.Power.csproj` built with `0` errors; its two warnings came from the Unity Test Framework dependency rather than AD-20 code.
- The AD-20 scene changes have no duplicate file IDs, and `Assets/Scenes/Scene_Physics.unity` remains untouched in the feature branch.
- No automated tests were added for AD-20, following Eden's manual-playtest decision.
- Do not claim AD-20 runtime completion until Play Mode confirms the `20 -> 19` change after five seconds, repeated five-second drains, minimum clamping, and disable/re-enable behavior without duplicate coroutines.

## Known working-tree note

Unity automatically rewrote thousands of serialization lines in the legacy `Assets/Scenes/Scene_Physics.unity` after it was opened with the current Unity version. This change is unrelated to AD-19, was not committed, and must not be included in a future feature commit without deliberate review and approval.

## Next development point

Complete the AD-20 manual Play Mode verification before moving the Jira task to `Done` or merging the feature branch.

The next gameplay slice may connect a real gameplay consumer, such as fruit collection, to `PowerController.AddPower(...)`. Before implementing the next Jira task:

1. Read the task and inspect the existing project state.
2. Keep the approved Power responsibilities unchanged.
3. Define and approve any new system architecture before implementation.
4. Keep the change limited to one coherent vertical slice.
5. Verify Unity runtime behavior before declaring the next task complete.

For architecture work, follow the global `eden-software-architecture-coach` skill and the lecturer materials it references.

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
  Views/PowerView.cs
```

`PowerLifetimeScope` is the topic-specific Composition Root. It registers:

- `PowerModel` as `IPowerModel` with scoped lifetime.
- the serialized `PowerView` component as `IPowerView`.
- `PowerController` as itself with scoped lifetime.
- a build callback that resolves the Controller so the initial UI is synchronized.

Keep future feature registrations in their own topic-specific LifetimeScopes. Do not grow one generic `GameLifetimeScope` containing unrelated systems.

## Scene integration

- `Assets/Scenes/Adventure-Island-2-Game.unity` contains:
  - a dedicated `PowerLifetimeScope` GameObject
  - `PowerCanvas`
  - `Txt_Power`
  - `PowerView`
  - serialized references connecting the View and LifetimeScope
- The initial display is `Power: 20/30`.
- `Assets/Scenes/Scene_Physics.unity` is the older Mario-based scene and is considered legacy/reference content.
- Build Settings currently still list `Scene_Physics.unity`; changing the main build scene is a separate explicit task.

## Verification completed

- `AdventureIsland.Power.csproj` builds successfully with `0` errors and `0` warnings.
- Unity imported and loaded `Adventure-Island-2-Game.unity` successfully after the Power integration.
- Scene references were checked: `powerView` and `powerText` are assigned and the added scene objects have no duplicate file IDs.
- Automated Power tests were intentionally removed at Eden's request. Do not claim automated test coverage for AD-19.

## Known working-tree note

Unity automatically rewrote thousands of serialization lines in the legacy `Assets/Scenes/Scene_Physics.unity` after it was opened with the current Unity version. This change is unrelated to AD-19, was not committed, and must not be included in a future feature commit without deliberate review and approval.

## Next development point

The next gameplay slice may connect a real gameplay consumer, such as fruit collection, to `PowerController.AddPower(...)`. Before implementing the next Jira task:

1. Read the task and inspect the existing project state.
2. Keep the approved Power responsibilities unchanged.
3. Define and approve any new system architecture before implementation.
4. Keep the change limited to one coherent vertical slice.
5. Verify Unity runtime behavior before declaring the next task complete.

For architecture work, follow the global `eden-software-architecture-coach` skill and the lecturer materials it references.

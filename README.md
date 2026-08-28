# Adventure Island 2

A Unity 2D platformer inspired by Adventure Island 2 for the NES. The project is being developed as a final game-development assignment with small playable Vertical Slices, SOLID principles, and extensible gameplay systems.

## Game Goal

The player must reach the end of two different stages:

- **Stage 1:** side-scrolling platforming toward the right.
- **Stage 2:** a maze-like platforming stage focused on jumping.

The planned gameplay systems include player movement and jumping, power and lives, fruits, weapons, mountable animals, obstacles, enemies, eggs, Fairy power, HUD screens, and stage transitions.

## Current Project State

The repository currently contains the Unity project foundation and an existing 2D gameplay prototype, including:

- Player movement and jumping.
- Enemy and spawning foundations.
- Weapon and projectile foundations.
- Pickup and power-up foundations.
- Reusable prefabs, sprites, scenes, and level resources.
- Partial examples of Factory, Strategy, Builder, Pooling, and MVC.

The full Adventure Island 2 feature set is still being implemented. Existing prototype code is evaluated and improved system by system; it is not assumed to be the final architecture.

## Requirements

- Unity `6000.3.21f1`
- Git, if you want to work with the repository history

## Open the project

1. Open Unity Hub.
2. Choose **Add** and select this project folder.
3. Open the project with Unity `6000.3.21f1`.
4. Open `Assets/Scenes/Scene_Physics.unity`.
5. Press the **Play** button to run the scene.

## Controls

| Action | Input |
| --- | --- |
| Move left or right | `A` / `D` or the arrow keys |
| Jump | The project input mapped to `Jump` (usually `Space`) |

The player uses Unity's 2D physics system for the current movement and jumping prototype. Controls may change as the new Adventure Island systems are integrated.

## Project structure

```text
Assets/
  Scenes/          Unity scenes
  Scripts/         Player, gameplay, enemy, weapon, and support code
  Prefabs/         Reusable gameplay prefabs
  Resources/       Level data and runtime resources
  Sprites/         2D visual assets
Packages/          Unity package configuration
ProjectSettings/   Unity project and editor settings
docs/
  jira-plan.md     Jira and architecture planning source of truth
```

Important scripts include:

- `PlayerMovement.cs` - horizontal player movement
- `PlayerJump.cs` - player jumping
- `Enemy/EnemyCreator.cs` - existing enemy creation foundation
- `Enemy/GameManager.cs` - existing game management foundation
- `Controller/StarController.cs` - existing pickup behaviour
- `PowerUps/StarInvinciblePowerUp.cs` - existing invincibility foundation

## Planned Gameplay Systems

- Player movement, jumping, input abstraction, and Dependency Injection.
- Power that decreases over time.
- Three lives, respawn, stage restart, and Game Over.
- Two fruit types that restore power.
- Hammer and Boomerang weapon family.
- Blue, Red, and Green mountable animals.
- Rock, Fire, Abyss, Eggs, Rewards, and Fairy.
- Spider, Bird, Jumping Snake, Fire Snake, Frog, and Ghost enemies.
- Enemy drops and timed respawn.
- Two stages with automatic stage transition.
- MVC HUD and Game Screens.

## Architecture Direction

The project is planned around:

- SOLID principles and design for known domain extension.
- Manual Dependency Injection through a Composition Root.
- Factory for families of Weapons, Enemies, Animals, and Rewards.
- Builder for configurable Projectiles.
- Reusable Pooling for frequently created runtime objects.
- Strategy for interchangeable attack and movement behaviours.
- Template Method for shared Enemy lifecycle behaviour.
- MVC for HUD and game-state presentation.
- Async & Tasks for cancellable timers, respawn, Fairy duration, and scene loading where appropriate.

Gameplay features are implemented as Vertical Slices:

```text
Design -> Code -> Minimal Unity Integration -> Prefab / Collider / Scene Setup -> Test -> Playtest -> Documentation
```

## Planning and Jira

Jira project: **AD - Adventure Game-Island**

Jira hierarchy:

```text
Epic -> Story / Task -> Subtask
```

The project uses progressive planning. All major Epics are visible, but only the next small execution batch is expanded into Stories and detailed Subtasks.

The authoritative planning document is [`docs/jira-plan.md`](docs/jira-plan.md). It defines Epic ownership, Story/Subtask rules, SOLID and Pattern expectations, the development roadmap, the Definition of Done, and the process for planning the next Jira batch.

The high-level roadmap is available in [`DEVELOPMENT_ROADMAP.md`](DEVELOPMENT_ROADMAP.md).

## Git notes

The repository includes a Unity `.gitignore`. Generated folders such as `Library`, `Temp`, `Logs`, and `UserSettings` are intentionally excluded from Git. The source project folders and Unity metadata remain tracked.

## Current status

The current repository contains a playable prototype foundation. The full Adventure Island 2 assignment feature set is still under progressive development and is tracked in Jira project `AD`.

## Reference Project

The previous Mario project is a reference implementation only:

```text
Inspect -> Understand -> Evaluate -> Adapt or Rewrite
```

Existing behaviour may be reused when it fits, but its architecture is not copied automatically. Adventure Island 2 maintains its own system boundaries and extension points.

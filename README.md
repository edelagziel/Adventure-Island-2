# Adventure Island 2 — Unity 2D Platformer

<p align="center">
  <strong>A course-built 2D platformer focused on clean gameplay architecture, reusable systems, and classic arcade mechanics.</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Unity-6000.3.21f1-000000?logo=unity&logoColor=white" alt="Unity 6000.3.21f1" />
  <img src="https://img.shields.io/badge/C%23-Game%20Architecture-512BD4?logo=csharp&logoColor=white" alt="C#" />
  <img src="https://img.shields.io/badge/VContainer-1.19.0-5C2D91" alt="VContainer 1.19.0" />
  <img src="https://img.shields.io/badge/Status-Active%20Course%20Project-2ea44f" alt="Project status" />
</p>

> Educational, non-commercial project inspired by the gameplay style of *Adventure Island 2*.  
> This repository is not affiliated with or endorsed by the original game's rights holders.

## Overview

This project recreates the feel of a classic 2D platformer while using modern Unity architecture and software-engineering practices.

The gameplay work includes:

- Player movement, jumping, power, lives, and stage restart flows.
- Hammer and Boomerang weapon systems.
- Blue, Red, and Green mountable animals with distinct attacks.
- Projectile pooling and reusable projectile construction.
- Multiple enemy behaviours: spiders, bird, jumping/fire snakes, frog, and ghost.
- Fruits, eggs, rewards, hazards, Fairy protection, and stage-local reset behaviour.
- Stage progression with shared player/HUD systems and stage-owned content.
- VContainer-based Dependency Injection and focused assembly boundaries.

## Engineering Highlights

| Area | Implementation |
| --- | --- |
| Dependency Injection | VContainer composition through a shared game lifetime scope |
| UI architecture | MVC-style Power, Fruit Progress, and Lives features |
| Runtime creation | Factory + Director + Builder flows for enemies and animals |
| Projectiles | Factory + Director/Builder configuration + reusable Pool |
| Combat | Capability-based interfaces such as IDefeatable and IDestructible |
| Enemy lifecycle | Shared Template Method-style death and respawn flow |
| Attack behaviour | Weapon / animal attack-source abstraction without target-type branching |
| Stage lifecycle | Stage-owned resettable content and centralized stage transitions |
| Design principles | SOLID, interface segregation, dependency inversion, event-driven coordination |

## Architecture at a Glance

~~~mermaid
flowchart TD
    Input[Player Input] --> AttackController[PlayerAttackController]

    AttackController --> WeaponController[WeaponController]
    AttackController --> AnimalAttackSource[AnimalAttackSource]

    WeaponController --> ProjectileProvider[ProjectileProvider]
    ProjectileProvider --> ProjectilePool[ProjectilePool]
    ProjectilePool --> ProjectileFactory[ProjectileFactory]

    AnimalAttackSource --> ActiveAnimal[PlayerActiveAnimal]

    EnemySpawn[EnemySpawn] --> EnemyFactory[EnemyFactory]
    EnemyFactory --> EnemyDirector[EnemyDirector]
    EnemyDirector --> EnemyBuilder[EnemyBuilder]
    EnemyBuilder --> Enemy[Enemy Lifecycle]

    Fruit[Fruit Pickup] --> Power[Power Controller]
    Fruit --> FruitProgress[Fruit Progress Controller]

    Power --> LivesFlow[LivesFlowCoordinator]
    FruitProgress --> LivesFlow
    LivesFlow --> StageFlow[StageFlowController]
~~~

The goal is not to apply patterns everywhere. Patterns are used only where they improve extension, ownership, or runtime composition.

## Gameplay Systems

### Combat

The player can attack through either the equipped weapon or the currently mounted animal.

~~~text
PlayerAttackInput
    -> PlayerAttackController
        -> default: WeaponController
        -> override: AnimalAttackSource
~~~

Weapons and animals do not need concrete enemy checks. Combat works through shared capabilities, allowing ordinary enemies, special enemies, hazards, and Fairy interactions to keep their own rules.

### Weapons and Projectiles

Current weapon families include:

- **Hammer** — limited accumulated throws.
- **Boomerang** — reusable outward-and-return projectile.

Projectiles are leased from a shared pool and reconfigured before every use. The pool owns reuse; factories instantiate; builders/directors configure.

### Mountable Animals

The player can mount three animal types:

- **Blue** — tail attack.
- **Red** — ranged fire projectile.
- **Green** — timed spin attack.

A shared PlayerActiveAnimal owns the current mount and handles deterministic replacement.

### Enemies

Enemy implementations share one reusable lifecycle for defeat, temporary runtime removal, respawn, and state restoration.

Concrete behaviours include:

- Vertical and static spiders.
- Bird movement with vertical oscillation.
- Jumping Snake.
- Fire Snake with pooled projectiles.
- Proximity-triggered Frog jumping.
- Ghost pursuit with special defeat rules.

### Progression and State

Gameplay progression coordinates several focused systems rather than using one large game manager:

- Power drains over time.
- Fruits restore Power and advance Fruit Progress.
- Fruit thresholds can grant Lives.
- Reaching minimum Power feeds into the Lives flow.
- Remaining Lives restart the current stage.
- Zero Lives reset the run to the first stage.
- Stage-local objects restore through IStageResettable.

## Project Structure

~~~text
Assets/
├── Prefabs/
├── Scenes/
├── Scripts/
│   ├── Animals/
│   ├── Combat/
│   ├── Enemies/
│   ├── Fairy/
│   ├── Fruit/
│   ├── FruitProgress/
│   ├── Lives/
│   ├── Power/
│   └── StageFlow/
└── Sprites/

Packages/
ProjectSettings/
docs/
├── PROJECT_STATE.md
└── jira-plan.md
~~~

## Getting Started

### Requirements

- Unity 6000.3.21f1
- Git

### Open the project

1. Clone the repository.
2. Open Unity Hub.
3. Choose **Add** and select the repository folder.
4. Open it with Unity 6000.3.21f1.
5. Open the production game scene from Assets/Scenes/.
6. Enter Play Mode.

## Controls

| Action | Input |
| --- | --- |
| Move | A / D or arrow keys |
| Jump | Space / configured Jump input |
| Attack | Project-configured attack input |

## Development Approach

Features are built as small vertical slices:

~~~text
Design
  -> Code
  -> Unity Integration
  -> Prefab / Scene Wiring
  -> Verification
  -> Playtest
  -> Documentation
~~~

The repository also keeps a detailed implementation record in [docs/PROJECT_STATE.md](docs/PROJECT_STATE.md).

## Current Status

The project contains the production gameplay foundation and multiple integrated systems, including Power/Lives/Fruit flow, combat, projectile pooling, mountable animals, enemy behaviours, eggs/rewards, Fairy protection, and stage-flow infrastructure.

Some final presentation, level-content, and Play Mode verification work may still evolve as the course project is completed.

## Tech Stack

- Unity 6
- C#
- VContainer
- TextMeshPro
- Unity 2D Physics
- Git / GitHub
- Jira

## Design Goals

The project intentionally prioritizes:

- Small, focused classes.
- Dependency inversion instead of hard-coded concrete dependencies.
- Reusable capabilities instead of target-type conditionals.
- Clear ownership of lifecycle and reset behaviour.
- Composition-root configuration rather than service locators or singletons.
- Patterns that serve the gameplay rather than patterns for their own sake.

## Notes on Assets

This repository is an educational portfolio/course project. Source code and project-specific implementation are presented for learning and demonstration purposes. Visual assets may have separate ownership or usage terms and should not be assumed to be independently licensed for reuse.

# Planning Governance

This document is the architectural and Jira planning source of truth for Adventure Island 2.

Future work must not rely on conversation memory. Before creating new Jira Stories, creating new Subtasks, restructuring an Epic, introducing a new gameplay system, or making a significant architectural decision, review:

1. /docs/jira-plan.md
2. The current Jira state in project AD
3. The current codebase in this repository

The three sources have different responsibilities:

- jira-plan.md = global intended direction and future roadmap.
- Jira = current execution backlog and work state.
- Codebase = actual implemented reality.

None of them should be used alone when planning the next batch.

## Conflict Handling

If the current codebase or Jira state conflicts with this document:

1. Identify the conflict.
2. Explain why the previous assumption may no longer fit.
3. Propose the smallest planning or architecture change.
4. Wait for explicit approval before changing this source-of-truth document if the change is significant.

Do not silently follow the old plan, silently rewrite the plan, or force implementation to match an outdated assumption. The plan is authoritative, but it is not immutable. It must evolve intentionally.

# Adventure Island 2 - Jira and Architecture Plan

## Project Context

- Unity project: Adventure Island 2.
- Jira project: AD - Adventure Game-Island.
- Jira project type: team-managed software project.
- Ownership: current implementation and planning work is assigned to Eden unless explicitly changed.
- Current Jira baseline: AD-1 through AD-9 are the nine Epics, AD-10 and AD-11 are the first two Stories, and AD-12 through AD-18 are detailed Subtasks under AD-10.
- Current Jira issues are intentionally only the initial execution batch. The future roadmap below is not a promise that all future Stories already exist in Jira.

## Jira Hierarchy

Epic
  -> Story / Task
       -> Subtask

### Epic

An Epic represents a major game system, such as Weapons or Enemies. It owns the system scope and boundaries.

### Story

A Story is a small gameplay-oriented Vertical Slice that can be run, tested, playtested, and demonstrated.

Every Story must answer:

> What working part of the game exists after this Story is complete?

### Subtask

A Subtask is one practical action that directly contributes to completing its parent Story.

Primary types are Code, Unity Integration, Test, Playtest, and Documentation.

### Task

A Task is technical work that is not itself a gameplay Feature, such as layers, Build Settings, project setup, tooling, documentation, or final video setup.

### Bug

A Bug records a problem found during implementation, testing, or Playtest. It includes reproduction steps, expected result, actual result, severity, and evidence.

## No Jira Issue Per Class

Do not create a separate Jira Issue for every Interface, Class, Method, or Script. Jira tracks outcomes and systems, not every source file.

Interfaces and abstractions are introduced inside the Story where they are required.

Example:

Story: Player attacks using a Hammer
Subtask: Create weapon abstraction and Hammer implementation

Do not create disconnected Issues named IWeapon, WeaponBase, HammerWeapon, and WeaponService merely because four code elements exist.

## Design for Extension

The project must be designed for known and plausible domain extension from the first implementation.

Before implementing a system, ask:

1. What concept is being introduced?
2. Is it part of a family of behaviours or objects?
3. What is expected to vary?
4. What should remain stable?
5. Are additional implementations already known from the game requirements?
6. Which SOLID principles apply?
7. Should the dependency be injected?
8. Is Factory appropriate?
9. Is Builder appropriate?
10. Is Strategy appropriate?
11. Is Template Method appropriate?
12. Is Pooling appropriate?
13. Is an async lifecycle involved?
14. Can a future implementation be added without modifying unrelated existing implementations?

Known families include:

- Hammer, Boomerang, and future weapons.
- Blue, Red, and Green animals and their attack behaviours.
- Spider, Bird, Jumping Snake, Fire Snake, Frog, and Ghost.
- Configurable projectiles.
- Egg rewards containing Animals, Weapons, and Fairy.

Do not wait for duplication to appear when the requirements already identify a family. At the same time, do not add meaningless abstractions or patterns merely because the assignment names them. Design for known or plausible extension, not speculative complexity.

## SOLID Rules

### Single Responsibility Principle

Each class or service has one clear responsibility. Movement, input, power, lives, weapons, projectiles, enemies, UI, and level flow are separate responsibilities.

### Open/Closed Principle

Systems with real variation allow new implementations or configurations without rewriting unrelated existing implementations.

### Liskov Substitution Principle

Implementations of an abstraction must honour its behavioural contract and remain replaceable.

### Interface Segregation Principle

Interfaces are small and focused. A collectible should not be forced to implement attack behaviour, and an enemy should not be forced to implement capabilities it does not have.

### Dependency Inversion Principle

High-level gameplay systems should not create replaceable concrete low-level dependencies. They should depend on suitable abstractions and receive dependencies from outside.

## Mandatory Pattern Strategy

The assignment requires Dependency Injection, Pooling System, Builder, Factory, MVC, Async & Tasks, and Template Pattern.

These are not academic checkboxes. A Pattern is used where it solves an extensibility, lifecycle, creation, or responsibility problem. A Pattern may be used in more than one system.

| Pattern | Main systems | Intended use |
|---|---|---|
| Dependency Injection | Player Foundation and project-wide services | Replaceable dependencies and Composition Root wiring |
| Factory | Enemies, Rewards, Weapons, Animals | Creation of object families |
| Builder | Projectiles and other configurable objects | Construction with multiple configuration values |
| Pooling | Projectiles, effects, enemies, reusable objects | Repeated rent/release lifecycle |
| Strategy | Animal attacks and interchangeable enemy behaviours | Replaceable algorithms and behaviours |
| Template Method | Enemy lifecycle and shared algorithms | Stable flow with controlled variation |
| MVC | HUD and Game Screens | Separation of state, coordination, and Unity UI |
| Async & Tasks | Fairy duration, enemy respawn, scene loading | Cancellable time-based and loading flows |

## Dependency Injection Rule

DI is a project-wide architectural rule, not a one-time Feature.

Use a clear Composition Root, beginning in Player Foundation:

GameBootstrap / CompositionRoot
        -> PlayerMovement
        -> IPlayerInput

Avoid having PlayerMovement create UnityInputReader, PlayerCombat create HammerWeapon, Enemy create a concrete SpawnService, or HUDController create a concrete Model.

Manual DI is sufficient. A DI Framework is optional and must not be introduced without a concrete benefit.

## Architecture Boundaries

- Core/Domain: game rules, data, contracts, and events that do not need Unity.
- Application/Services: use-case orchestration such as collect, attack, damage, death, respawn, and stage transition.
- Infrastructure/Unity: MonoBehaviours, Physics2D, prefabs, scenes, animation, audio, and loading.
- Presentation: HUD, screens, visual feedback, and animation presentation.

Gameplay rules must not exist only inside a View, Prefab, or collision callback. A Unity adapter receives the Unity event and delegates to the relevant service or rule.

## Approved Epic Structure

### Epic 1 - Player Foundation

#### Purpose

Build the reusable base needed for a controllable player.

#### Ownership

- Left/right movement.
- Jump.
- Input boundary/abstraction.
- Player prefab.
- Spawn and respawn points.
- GameBootstrap/Composition Root.
- Initial manual DI wiring.

#### Out of scope

Power, lives, weapons, enemies, and full HUD.

#### Major systems

Player movement, jump, input adapter, configuration, prefab setup, and spawn setup.

#### Extension points

Player Movement depends on an input contract rather than constructing a concrete input reader. A future input implementation can be added without changing movement rules.

### Epic 2 - Power, Fruits and Lives

#### Purpose

Own the player's power and life state.

#### Ownership

- Power state.
- Power drain over time.
- Fruit Type 1 (+1 Power).
- Fruit Type 2 (+2 Power).
- Fruit counter.
- Three starting lives.
- Death, respawn, stage restart, Game Over, and full reset.

#### Out of scope

Rock, Fire, Abyss, Weapon logic, Enemy logic, and full HUD presentation.

#### Major systems

Power Model/service, collectible fruit interaction, life state, respawn flow, and reset flow.

#### Extension points

The collectible contract must remain small. New fruit types or values should be added without rewriting the existing Power Model.

### Epic 3 - Weapons and Combat

#### Purpose

Build a Weapon family and the combat/projectile flow.

#### Ownership

- Weapon abstraction.
- Active weapon and inventory state.
- Pickup, equip, and unequip.
- Hammer.
- Boomerang.
- Throwing.
- Projectile configuration and collision.
- Damage.
- Valid obstacle destruction and enemy damage.

#### Out of scope

Animal-specific attack ownership, Enemy lifecycle ownership, and HUD ownership.

#### Major systems

Weapon handling, projectile creation, projectile lifetime, hit rules, and combat events.

#### Extension points

Hammer is the first implementation of a Weapon family. Boomerang and future weapons must not require modifying unrelated existing weapon implementations.

### Epic 4 - Animals and Mounting

#### Purpose

Build collectible and mountable Animals with replaceable attack behaviours.

#### Ownership

- Animal abstraction.
- Mount and unmount.
- Blue Animal with tail attack.
- Red Animal with fire attack.
- Green Animal with spin attack.
- Heart, Leaf, and Star pickups.
- Animal replacement while mounted.
- Animal collision and attack rules.

#### Out of scope

Generic Enemy lifecycle, weapon ownership, and level transition ownership.

#### Major systems

Mount state, animal movement integration, attack execution, and replacement flow.

#### Extension points

Animal attack variation is known from the requirements. Plan it from the first animal as an extension point, for example through IAnimalAttackStrategy, while implementing only the currently required strategy in the first Story.

### Epic 5 - Obstacles, Eggs and Rewards

#### Purpose

Own world obstacles, eggs, rewards, and Fairy interaction rules.

#### Ownership

- Rock.
- Fire/Bonfire.
- Abyss.
- Egg opening.
- Animal, Weapon, and Fairy rewards.
- Fairy invincibility for 10 seconds.
- Special destruction rules.

#### Out of scope

Weapon implementations, Enemy lifecycle, and Stage transition ownership.

#### Major systems

Rock reduces Power by 3; valid attacks can destroy it. Fire causes death and cannot be destroyed by normal attacks. Fairy can destroy Fire and Ghost. Abyss remains lethal even while Fairy is active.

#### Extension points

RewardFactory may represent the reward family once Egg rewards are implemented. Obstacle interaction should use capability contracts rather than hard-coded knowledge of every weapon or animal class.

### Epic 6 - Enemy System

#### Purpose

Build the extensible Enemy family and its shared lifecycle.

#### Ownership

- Enemy abstraction.
- Spider.
- Bird.
- Jumping Snake.
- Fire Snake.
- Frog.
- Ghost.
- EnemyFactory.
- Enemy lifecycle.
- Enemy death, drops, and respawn.
- Ghost special rules.

#### Out of scope

Player weapon ownership, Animal mounting ownership, and Stage transition ownership.

#### Major systems

Enemy initialization, spawn, movement/behaviour, detection, attack, hit, death, reward drop, scheduled respawn, and respawn at the original position.

#### Extension points

EnemyFactory is planned from the first Enemy. Template Method is a real lifecycle algorithm with controlled variation points. Strategies may be used for movement or attack behaviours when those behaviours are interchangeable.

### Epic 7 - Level Flow and Two Stages

#### Purpose

Connect the complete game flow and two playable stages.

#### Ownership

- Stage 1: rightward platforming.
- Stage 2: maze and jumping platforming.
- Goals and stage completion.
- Stage 1 to Stage 2 transition.
- Stage 2 to Final Complete.
- Scene loading.
- Cleanup of timers, enemies, pools, and runtime state.

#### Out of scope

Detailed Enemy, Weapon, Animal, and UI implementation ownership.

#### Major systems

Stage definitions, goals, loading, transition, reset, and completion flow.

#### Extension points

Stage configuration should allow additional stages or themes without rewriting the transition system.

### Epic 8 - MVC HUD and Game Screens

#### Purpose

Present game state through a real MVC implementation.

#### Ownership

- Power HUD.
- Lives HUD.
- Fruit count.
- Active Weapon.
- Active Animal.
- Fairy timer.
- Pause.
- Game Over.
- Stage Complete.
- Final Complete.

#### Out of scope

Gameplay rules, collision decisions, and state ownership.

#### Major systems

- Model owns displayable game state.
- Controller reacts to events and use-cases.
- View renders Unity UI only.

#### Extension points

New display fields or screens should be added without moving gameplay rules into the View.

### Epic 9 - Assets, Audio, QA and Submission

#### Purpose

Prepare a demonstrable, balanced, documented, and submittable project.

#### Ownership

- Sprites.
- Animations.
- Audio.
- Visual feedback.
- Level content placement.
- Balancing.
- Full QA and Playtest.
- Build.
- README.
- Architecture and Pattern documentation.
- Final video.

#### Out of scope

New gameplay systems that should belong to another Epic.

#### Major systems

Art/content pass, sound, balancing, build verification, documentation, and assignment video.

#### Extension points

Content should be data/prefab-driven where useful, while remaining separate from gameplay rules.

## Ownership Boundaries

| System | Owning Epic |
|---|---|
| Movement and Input | Player Foundation |
| Power and Lives | Power, Fruits and Lives |
| Rock, Fire, and Abyss | Obstacles, Eggs and Rewards |
| Weapons and Projectiles | Weapons and Combat |
| Animal Mounting and Attacks | Animals and Mounting |
| Enemy Behaviours and Respawn | Enemy System |
| Stage Transitions | Level Flow and Two Stages |
| HUD and Game Screens | MVC HUD and Game Screens |
| Art, QA, Build, and Video | Assets, Audio, QA and Submission |

A system may call another system's API without taking ownership of that system. Rock can reduce Player Power through the Power API, but Rock remains owned by the Obstacles system.

## Vertical Slice Policy

Every gameplay Story includes minimal Unity integration. A Story is not Done merely because C# code compiles.

```text
Design
  -> Code
  -> Minimal Unity Integration
  -> Prefab / Collider / Scene Configuration
  -> Test
  -> Playtest
  -> Documentation
```

Basic functional Unity integration cannot be deferred to a later Content Pass. Art, audio, additional placements, polish, and balancing may be deferred.

Example:

```text
Story: Player collects Fruit Type 1 and gains Power
```

The Story is not Done until the Model, fruit behaviour, prefab, trigger, scene placement, Power change, test, and Playtest all work.

## Story Rules and Template

Stories are gameplay-oriented and should normally fit within approximately 1–2 work days. If a Story is larger, split it by gameplay outcome, not by number of Classes.

Every Story must include:

```text
Summary
Goal
Scope
Out of scope
Related system/Epic
Dependencies
Architecture / Extension Points
Acceptance Criteria
Architectural Acceptance Criteria
Technical Direction
Relevant Patterns
SOLID Considerations
Unity Integration
Testing / Definition of Done
Evidence
```

Acceptance Criteria must cover gameplay behaviour, Unity integration, meaningful extension points, tests or Playtest, and absence of unnecessary coupling.

Examples:

```text
Adding BoomerangWeapon does not require modifying HammerWeapon.
Adding Bird does not require modifying Spider.
Adding FireAttackStrategy does not require modifying TailAttackStrategy.
```

## Subtask Rules and Template

Subtasks are practical and small, normally 30 minutes to several hours. A Subtask may cover closely related code elements when they form one deliverable. Do not make a Subtask for every line or Class.

Every Subtask must include:

```text
Summary
Description
Type
Related files/system
Done when
Evidence
```

## Progressive Planning

Future Jira expansion uses rolling-wave/progressive planning.

Normal workflow:

1. Review this document.
2. Inspect completed Jira Stories.
3. Inspect the current code.
4. Determine the next logical execution batch.
5. Create only a small number of near-term Stories.
6. Create detailed Subtasks only for the immediate Story.
7. Implement.
8. Test.
9. Playtest.
10. Close the Story.
11. Reassess the plan.
12. Expand the next batch.

Do not generate the entire project backlog in advance. Future roadmap entries are planning visibility, not already-created Jira Stories.

## Pattern Review Rule

Before planning or implementing every new Story, perform this review:

- What concept is being introduced?
- Is it part of a family of behaviours or objects?
- What is expected to vary?
- What should remain stable?
- Are future implementations already known from the requirements?
- Which SOLID principles apply?
- Should the dependency be injected?
- Is Factory appropriate?
- Is Builder appropriate?
- Is Strategy appropriate?
- Is Template Method appropriate?
- Is Pooling appropriate?
- Is an async lifecycle involved?
- Can a future implementation be added without modifying unrelated existing implementations?

Known domain variation is sufficient reason to create an extension point from the first implementation. Do not add meaningless abstractions merely because a Pattern exists.

## Development Roadmap

The roadmap is intentionally high-level. Its entries are not all Jira Stories yet.

1. Player Foundation: movement, jump, input boundary, DI, Composition Root, prefab, and spawn point.
2. Power/Fruits/Lives: Power state, Fruit Type 1, Fruit Type 2, drain, three lives, death, respawn, and reset.
3. Obstacles baseline: Rock, Fire, Abyss, and interaction rules.
4. Weapons: Weapon family, Hammer, active weapon, pickup, and throwing.
5. Projectile architecture: projectile configuration, Builder, collision, lifetime, and reusable Pooling.
6. Animals: mount, Blue Animal, Red Animal, Green Animal, pickups, replacement, and attack Strategies.
7. Enemies: Enemy family, Spider, EnemyFactory, Template lifecycle, respawn, Bird, Jumping Snake, Fire Snake, Frog, and Ghost.
8. Egg/Rewards/Fairy: Egg opening, Animal/Weapon/Fairy rewards, RewardFactory, Fairy lifetime, and cancellation.
9. Level flow: Stage 1, Stage 2, goals, transition, scene loading, and cleanup.
10. MVC HUD: Power, Lives, Fruits, Weapon, Animal, Fairy timer, pause, and completion screens.
11. Content/Art/Audio: placements, sprites, animations, audio, feedback, and balancing.
12. QA: unit tests, integration tests, full Playtest, bug fixes, and requirement verification.
13. Build: clean build and Build Settings verification.
14. Documentation: README, architecture explanation, Pattern explanation, and known limitations.
15. Final video: code explanation plus full gameplay demonstration.

Individual animal and enemy types remain visible above. Do not replace them with “Remaining animals” or “Remaining enemies.”

## Corrected Execution Order

The intended order of near-term gameplay slices is:

1. Player moves left and right.
2. Player jumps.
3. Input boundary, DI, and Composition Root.
4. Player has Power.
5. Player collects Fruit Type 1 and gains +1 Power.
6. Player collects Fruit Type 2 and gains +2 Power.
7. Power drains over time.
8. Player starts with three lives.
9. Player dies, respawns, and restarts the stage.
10. Game Over resets the game.
11. Rock reduces Power by 3.
12. Weapon family and Hammer.
13. Boomerang.
14. Projectile Builder.
15. Projectile Pooling.
16. Animal family and first mount.
17. Blue Animal tail attack.
18. Mounted animal replacement.
19. Red Animal fire attack.
20. Green Animal spin attack.
21. Enemy family and Spider.
22. EnemyFactory.
23. Enemy Template lifecycle.
24. Enemy respawn.
25. Bird.
26. Jumping Snake.
27. Fire Snake.
28. Frog.
29. Ghost.
30. Enemy reward/drop.
31. Egg opens.
32. RewardFactory.
33. Fairy.
34. Fire obstacle and Fairy interaction.
35. Stage 1.
36. Stage transition.
37. Stage 2.
38. MVC HUD.
39. Game screens.
40. Content pass.
41. Art and animation.
42. Audio and feedback.
43. Balancing.
44. QA.
45. Build.
46. Documentation.
47. Final video.

This order does not mean all architecture is postponed until a later task. Each Story must design the known family extension point that it introduces.

## First Jira Execution Batch Already Created

The initial approved batch currently in Jira is:

### Epics

- AD-1 Player Foundation
- AD-2 Power, Fruits and Lives
- AD-3 Weapons and Combat
- AD-4 Animals and Mounting
- AD-5 Obstacles, Eggs and Rewards
- AD-6 Enemy System
- AD-7 Level Flow and Two Stages
- AD-8 MVC HUD and Game Screens
- AD-9 Assets, QA and Submission

### Near-term Stories

- AD-10 Player can move and jump, parent AD-1.
- AD-11 Player collects Fruit Type 1 and gains Power, parent AD-2.
- AD-10 blocks AD-11.

### Detailed Subtasks

The following are the only detailed Subtasks in the initial batch, all under AD-10:

- AD-12 Define player movement contract and input boundary.
- AD-13 Create manual Composition Root for Player Foundation.
- AD-14 Playtest Player Movement.
- AD-15 Integrate Player Movement in Unity.
- AD-16 Document Player Foundation architecture.
- AD-17 Implement horizontal movement and jump behaviour.
- AD-18 Test Player Movement behaviour.

Do not expand the entire backlog simply because the Epics exist.

## First Jira Batch Policy for Future Expansion

The first batch has already been created. For every later batch:

1. Create only the next 1–3 Stories.
2. Fully decompose only the immediate Story into Subtasks.
3. Optionally create the next Story with a clear Goal and Dependencies but without speculative Subtasks.
4. Add blocks only for real execution dependencies.
5. Keep each Story demonstrable and small.

## Requirement Requiring Clarification: 30 Fruits

The assignment contains the ambiguous requirement:

```text
Collecting 30 fruits gives a “פסילה”.
```

Status: Needs Instructor Clarification.

Do not reinterpret this as gaining a life, losing a life, receiving a bonus, or resetting the game until the instructor confirms the intended behaviour.

## Old Mario Project Reference Rule

The existing Mario project is a reference implementation only:

```text
Inspect
  -> Understand
  -> Evaluate
  -> Adapt or Rewrite
```

Do not automatically copy its architecture. Evaluate:

- Tight coupling.
- Singleton usage.
- Reflection-based Factory.
- Concrete dependency creation inside classes.
- Mario-specific assumptions.
- Missing DI.
- Non-reusable Pooling or Builder solutions.
- Behaviour that does not fit Adventure Island.

Reusable behaviour may be migrated after evaluation. Adventure Island must keep its own architecture and boundaries.

## Definition of Done

A Story is Done only when:

- Required code exists.
- Relevant architecture follows SOLID.
- Expected extension points are represented cleanly.
- Dependencies are not unnecessarily hard-coded.
- Relevant Patterns are implemented meaningfully.
- Unity integration is complete.
- Prefab and scene setup are complete.
- No relevant compile errors exist.
- Tests exist where reasonable.
- Missing tests are justified when appropriate.
- Playtest is complete.
- Acceptance Criteria pass.
- Architectural Acceptance Criteria pass.
- No temporary TODO prevents actual usage.
- Evidence is recorded.
- Changed systems/files are documented.

## Labels

### Area labels

```text
area:player
area:power
area:weapon
area:animal
area:enemy
area:obstacle
area:level
area:ui
area:qa
```

### Type labels

```text
type:code
type:unity
type:test
type:playtest
type:docs
```

### Pattern labels

```text
pattern:di
pattern:factory
pattern:builder
pattern:pooling
pattern:strategy
pattern:template
pattern:mvc
pattern:async
```

Do not create a label for every SOLID principle unless a real tracking need appears.

## Jira Relationship Rules

### Parent/Subtask

Use the hierarchy:

```text
Epic -> Story/Task -> Subtask
```

Subtasks are not created directly under an Epic.

### Blocks

Use only when one Issue must be completed before another can be executed or verified.

### Relates to

Use for a meaningful relationship without a blocking dependency.

### Duplicates

Use only for a genuine duplicate.

Do not use Issue Links instead of Parent/Subtask relationships.

# Next Jira Execution Batch Procedure

Before creating new Jira work:

1. Read this entire planning document.
2. Inspect the current Jira status.
3. Inspect completed Stories.
4. Inspect the active Story and its Subtasks.
5. Inspect the relevant current code.
6. Compare implementation reality with this roadmap.
7. Determine the next 1–3 Stories only.
8. Fully decompose only the immediate Story into Subtasks.
9. Preserve Epic ownership and dependency order.
10. Do not create the full backlog.
11. If the plan needs modification, explain the conflict and proposed change before changing this document.

The current Jira backlog is the execution state, not proof that the code is complete. The codebase is the implementation reality, not permission to silently change the approved plan.

# Reusable Prompt for Expanding Jira

```text
Read /docs/jira-plan.md first and treat it as the architectural and Jira planning source of truth for Adventure Island 2.

Then inspect the current Jira state in project AD and the current Unity codebase. Review what has actually been completed, including the active Story and its Subtasks.

Plan only the next execution batch. Create only the next 1–3 Stories and create detailed Subtasks only for the immediate Story. Preserve Epic ownership, Vertical Slice integration, SOLID, known extension points, DI, and the relevant design patterns.

Do not create the full backlog and do not create an Issue per Class, Interface, Method, or Script.

If the current implementation or Jira state conflicts with jira-plan.md, explain the conflict and propose the smallest change before changing direction or modifying the source-of-truth document.
```

## Governance Summary

The intended workflow is:

```text
Planning source of truth
  -> Current Jira and current code inspection
  -> Next 1–3 Stories
  -> Detailed Subtasks for the immediate Story
  -> Code + Unity Integration
  -> Test + Playtest
  -> Evidence and Story closure
  -> Reassess and expand the next batch
```

The full project plan remains visible in this document, while Jira remains intentionally small and actionable.

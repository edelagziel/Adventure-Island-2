# Mario Assignment 3

A 2D physics-based platformer project built with Unity. The project includes a playable physics scene, player movement and jumping, enemies, stars, power-ups, prefabs, and level data.

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

The player uses Unity's 2D physics system for movement and jumping. Collect stars, use power-ups, and avoid or defeat enemies while navigating the level.

## Project structure

```text
Assets/
  Scenes/       Unity scenes, including Scene_Physics
  Scripts/      Player, enemy, controller, and power-up code
  Prefabs/      Reusable gameplay objects
  Resources/    Level data and runtime resources
  Sprites/      2D visual assets
Packages/       Unity package configuration
ProjectSettings/ Unity project and editor settings
```

Important scripts include:

- `PlayerMovement.cs` - horizontal player movement
- `PlayerJump.cs` - player jumping
- `Enemy/EnemyCreator.cs` - enemy creation
- `Enemy/GameManager.cs` - game management
- `Controller/StarController.cs` - star behavior
- `PowerUps/StarInvinciblePowerUp.cs` - star invincibility power-up

## Git notes

The repository includes a Unity `.gitignore`. Generated folders such as `Library`, `Temp`, `Logs`, and `UserSettings` are intentionally excluded from Git. The source project folders and Unity metadata remain tracked.

## Current status

This repository starts from the existing assignment project. The initial project snapshot is committed as `chore: initial`.
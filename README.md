# Speedrun Timer Mod

[![GitHub release](https://img.shields.io/github/release/dalet/140-speedrun-timer.svg?label=latest%20release)](https://github.com/Dalet/140-speedrun-timer/releases/latest)
[![Azure DevOps builds (branch)](https://img.shields.io/azure-devops/build/dalleth/a492cfc0-3838-42a7-9d40-ff259da56e12/1/develop.svg?label=development%20build)](https://dalleth.visualstudio.com/140-speedrun-timer/_build/latest?definitionId=1&branchName=develop)

A mod for [140](https://store.steampowered.com/app/242820/) that adds an accurate and fair ingame timer for speedrunning, as well as various useful features for practice.

![screenshot](https://i.imgur.com/ur42sz0.png)

# Installation
[Download page](https://github.com/Dalet/140-speedrun-timer/releases/)
* **Installer**
  * Select the game folder (if not auto-detected) and click install
* ~~**Manual installation**~~ (removed since v0.6)
  * ~~Open the game folder and extract the DLLs in 140_Data/Managed~~

# Features
* Speedrun timer with or without loads
* Quick reset
* [LiveSplit](http://livesplit.org/) synchronization: in-game time, autosplitting
* Cheats for practice
* Fixes a bug in v2017 that makes the beat start late or early after level load depending on framerate.

## Speedrun Log

When you complete a level, an entry is added to `speedrun-log.csv` in the game's directory.

Platform | File location
---------|-----------
Windows  | `... steamapps\common\140`
macOS    | `~/Library/Application Support/Steam/steamapps/common/140`
Linux    | `~/.local/share/Steam/steamapps/common/140`

# Keybinds
|   Key   |  Description |
| ------- | ------------ |
| `R`+`R` | Reset to hub (Resets current level in IL mode) |
| `L shift`+`R`+`R` | Reset to hub in IL mode |
| `F1`    | Show the Real Time |
| `F2`    | Hide the timers |

### Cheats

|   Key   |  Description |
| ------- | ------------ |
| `Shift` + `F12` | Enable cheats |
| `1`-`9` | Teleport to the `n`<sup>th</sup> BeatLayerSwitch and activate it |
| `Alt` + `1`-`4` | Load the `n`<sup>th</sup> level (`Right Alt` for Mirrored) |
| `Delete` | Teleport to the current checkpoint |
| `Page Up` | Teleport to the next checkpoint |
| `Page Down` | Teleport to the previous checkpoint |
|  `Home` | Teleport to the first checkpoint |
|  `End`  | Teleport to the last checkpoint |
| `Backspace` | Flip the player color |
| `Q`         | Suicide              |
| `I`         | Toggle invincibility |


# Configuration

## Command line arguments

On Steam, they can be set via the "Set launch options..." button in the game's properties. More information about this here: https://support.steampowered.com/kb_article.php?ref=1040-JWMT-2947

| Argument (case-sensitive) | Description                                             |
|---------------------------|---------------------------------------------------------|
| `-disable-timer-mod`      | Disables the mod                                        |
| `-disable-livesplit-sync` | Disables LiveSplit synchronization                      |
| `-flash-cheat-watermark`  | Makes the cheat watermark appear only on cheat triggers |
| `-timer-il-mode`          | Enables IL mode. The timer will start on level load and reset upon hub activation. |

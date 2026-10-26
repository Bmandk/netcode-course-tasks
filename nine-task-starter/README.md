# Lecture 9 task starter: wandering agents

A single-player ECS simulation for the L9 task "Make it networked". Agents wander inside a square: each walks towards a random target and picks a new one when it arrives. A `SimulationConfig` in the SubScene sets the agent count, area size, speed and random seed.

Unity **6000.6.6f1**, Entities, Entities Graphics and Netcode for Entities **6.6.0**, URP.

| Path | What it is |
|---|---|
| `Assets/Scenes/L9Task.unity` | the scene to open: camera, light and the SubScene |
| `Assets/Scenes/L9TaskSubScene.unity` | the SubScene: `Ground` and `SimulationConfig` |
| `Assets/Prefabs/Agent.prefab` | the agent: a capsule with `AgentAuthoring` |
| `Assets/Scripts/GameBootstrap.cs` | `ClientServerBootstrap` that auto-connects on port 7979 |
| `Assets/Scripts/SimulationConfigAuthoring.cs` | `SimulationConfig` singleton: the scale knobs and the agent prefab |
| `Assets/Scripts/AgentAuthoring.cs` | `Agent`: per-agent wander state |
| `Assets/Scripts/SpawnAgentsSystem.cs` | spawns `AgentCount` agents once, then disables itself |
| `Assets/Scripts/WanderSystem.cs` | moves the agents (Burst, `IJobEntity`, `ScheduleParallel`) |
| `Assets/Scripts/HPGP.L9Task.asmdef` | assembly definition, already referencing `Unity.NetCode` and `Unity.InputSystem` |

## Running it

1. Open `Assets/Scenes/L9Task.unity`.
2. **Window → Multiplayer → Play Mode Tools**: set **PlayMode Type** to *Client & Server*.
3. Press Play. 1 000 capsules wander in the Game view.

**Window → Entities → Hierarchy** has a world dropdown: use it to compare `ServerWorld` and `ClientWorld`.

The project uses the **Input System** package only (**Project Settings → Player → Active Input Handling**), so read the keyboard with `Keyboard.current` (`using UnityEngine.InputSystem;`), not `UnityEngine.Input`. **Run In Background** is on, so the worlds keep ticking when the Editor loses focus.

## The task

The steps are on the "Task: make it networked" slides of lecture 9. In short: make the agent an interpolated ghost, decide which world each system runs in, go in game, add an RPC that lets a client spawn agents, and strip what clients don't need. Then measure it with thin clients and the network emulator.

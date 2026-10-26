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

Same as the "Task" slides of lecture 9: about 25 minutes in class, finish at home.

### Part 1: make it networked

1. **Run it as is** with PlayMode Type *Client & Server*. Compare `ServerWorld` and `ClientWorld` in the Entities Hierarchy. What do you see, and why?
2. **Make the agent a ghost:** Interpolated, Dynamic.
3. **Make the simulation server-authoritative:** decide the world for every system.
4. **Go in game** on both sides.
5. **Let clients influence it:** pressing *Space* sends an RPC asking for 50 agents at a random spot. The server validates the request and spawns them. (The Game view must have focus for the key press to reach the game.)
6. **Strip what clients don't need:** which fields of `Agent` does the client use?

### Part 2: observe

In **Play Mode Tools**, set **Num Thin Clients = 2** and the network emulator to **150 ms RTT, 20 ms jitter, 5 % loss**.

1. How far behind the server is the client's view? Estimate it from what you see, then explain it.
2. Press *Space* repeatedly. How long until the agents appear? Does it ever take much longer? Why?
3. Raise the agent count to 10 000 (`SimulationConfig` in the SubScene). What happens to how smoothly agents move on the client? Use the snapshot calculator from the lecture slides to explain it.
4. Look at the server world in the Systems window (**Window → Entities → Systems**). Which netcode system costs the most, and how does it change with the thin-client count?

Write down your numbers: they're the first data points for your report's profiling chapter.

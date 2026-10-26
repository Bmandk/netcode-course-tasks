using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace HPGP.L9Task
{
    // Spawns SimulationConfig.AgentCount agents once, then switches itself off.
    [BurstCompile]
    public partial struct SpawnAgentsSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationConfig>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            var config = SystemAPI.GetSingleton<SimulationConfig>();
            var rng = new Random(config.Seed);
            var agents = state.EntityManager.Instantiate(config.AgentPrefab, config.AgentCount, Allocator.Temp);
            foreach (var agent in agents)
                InitAgent(ref state, agent, RandomPointInArea(ref rng, config.AreaHalfExtent), config, ref rng);
        }

        public static void InitAgent(ref SystemState state, Entity agent, float3 position, in SimulationConfig config, ref Random rng)
        {
            state.EntityManager.SetComponentData(agent, LocalTransform.FromPosition(position));
            state.EntityManager.SetComponentData(agent, new Agent
            {
                Target = RandomPointInArea(ref rng, config.AreaHalfExtent),
                Speed = config.AgentSpeed * rng.NextFloat(0.5f, 1.5f),
                Rng = new Random(rng.NextUInt(1, uint.MaxValue)),
            });
        }

        public static float3 RandomPointInArea(ref Random rng, float halfExtent)
        {
            return new float3(rng.NextFloat(-halfExtent, halfExtent), 0.5f, rng.NextFloat(-halfExtent, halfExtent));
        }
    }
}

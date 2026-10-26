using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace HPGP.L9Task
{
    // Moves every agent towards its target; on arrival it picks a new random target.
    [BurstCompile]
    public partial struct WanderSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationConfig>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new WanderJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
                AreaHalfExtent = SystemAPI.GetSingleton<SimulationConfig>().AreaHalfExtent,
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    partial struct WanderJob : IJobEntity
    {
        public float DeltaTime;
        public float AreaHalfExtent;

        void Execute(ref LocalTransform transform, ref Agent agent)
        {
            var toTarget = agent.Target - transform.Position;
            var distance = math.length(toTarget);
            var step = agent.Speed * DeltaTime;
            if (distance <= step)
            {
                transform.Position = agent.Target;
                agent.Target = SpawnAgentsSystem.RandomPointInArea(ref agent.Rng, AreaHalfExtent);
            }
            else
            {
                transform.Position += toTarget / distance * step;
            }
        }
    }
}

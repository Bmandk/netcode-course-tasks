using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace HPGP.L9Task
{
    // Scale knobs for the simulation. Put exactly one in the SubScene.
    public struct SimulationConfig : IComponentData
    {
        public Entity AgentPrefab;
        public int AgentCount;
        public float AreaHalfExtent;
        public float AgentSpeed;
        public uint Seed;
    }

    public class SimulationConfigAuthoring : MonoBehaviour
    {
        public GameObject AgentPrefab;
        public int AgentCount = 1000;
        [Tooltip("Agents wander inside a square of ±AreaHalfExtent on X and Z.")]
        public float AreaHalfExtent = 50f;
        public float AgentSpeed = 5f;
        public uint Seed = 1;

        class Baker : Baker<SimulationConfigAuthoring>
        {
            public override void Bake(SimulationConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new SimulationConfig
                {
                    AgentPrefab = GetEntity(authoring.AgentPrefab, TransformUsageFlags.Dynamic),
                    AgentCount = authoring.AgentCount,
                    AreaHalfExtent = authoring.AreaHalfExtent,
                    AgentSpeed = authoring.AgentSpeed,
                    // Unity.Mathematics.Random must not be seeded with 0.
                    Seed = math.max(1u, authoring.Seed),
                });
            }
        }
    }
}

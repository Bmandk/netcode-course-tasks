using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace HPGP.L9Task
{
    // Per-agent wander state. Which of these fields does a client need to draw the agent?
    public struct Agent : IComponentData
    {
        public float3 Target;
        public float Speed;
        public Unity.Mathematics.Random Rng;
    }

    public class AgentAuthoring : MonoBehaviour
    {
        class Baker : Baker<AgentAuthoring>
        {
            public override void Bake(AgentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                // Target, speed and random state are set when the agent is spawned.
                AddComponent<Agent>(entity);
            }
        }
    }
}

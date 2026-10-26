using Unity.NetCode;

namespace HPGP.L9Task
{
    // Creates a server and a client world (per PlayMode Tools) and auto-connects them on port 7979.
    // There can only be one ClientServerBootstrap subclass per project.
    [UnityEngine.Scripting.Preserve]
    public class GameBootstrap : ClientServerBootstrap
    {
        public override bool Initialize(string defaultWorldName)
        {
            AutoConnectPort = 7979;
            return base.Initialize(defaultWorldName);
        }
    }
}

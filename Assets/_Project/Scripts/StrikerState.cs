using FishNet.Object;
using FishNet.Object.Synchronizing;

public class StrikerState : NetworkBehaviour
{
    public static StrikerState Instance { get; private set; }
    
    public readonly SyncVar<NetworkObject> Striker = new SyncVar<NetworkObject>();

    public bool HasStriker => Striker.Value != null;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        Instance = this;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        if (Instance == this) Instance = null;
    }

    public bool IsStriker(NetworkObject player) => HasStriker && Striker.Value == player;
    
    public void SetStriker(NetworkObject player)
    {
        if (!IsServerInitialized) return;
        Striker.Value = player;
    }

    public void Clear()
    {
        if (!IsServerInitialized) return;
        Striker.Value = null;
    }
}
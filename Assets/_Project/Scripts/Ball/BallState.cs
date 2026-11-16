using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public enum BallStatus { Default, Active }

public class BallState : NetworkBehaviour
{
    public static BallState Instance { get; private set; }

    public readonly SyncVar<BallStatus> Status = new SyncVar<BallStatus>(BallStatus.Default);
    public readonly SyncVar<NetworkObject> Holder = new SyncVar<NetworkObject>(null);

    public Transform FeetPoint { get; private set; }

    public bool IsHeld => Status.Value == BallStatus.Active;

    public bool IsHeldBy(NetworkObject player) => IsHeld && Holder.Value == player;

    public override void OnStartServer()
    {
        base.OnStartServer();
        Instance = this;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Instance = null;
    }

    public void SetActive(NetworkObject player, Transform feetPoint)
    {
        if (!IsServerInitialized) return;

        FeetPoint = feetPoint;
        Holder.Value = player;
        Status.Value = BallStatus.Active;
    }

    public void SetDefault()
    {
        if (!IsServerInitialized) return;

        FeetPoint = null;
        Holder.Value = null;
        Status.Value = BallStatus.Default;
    }
}
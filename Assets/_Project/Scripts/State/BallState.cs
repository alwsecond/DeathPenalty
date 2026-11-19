using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public enum BallStatus { Default, Active }

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class BallState : NetworkBehaviour
{
    public static BallState Instance { get; private set; }

    public readonly SyncVar<BallStatus> Status = new SyncVar<BallStatus>(BallStatus.Default);
    public readonly SyncVar<NetworkObject> Holder = new SyncVar<NetworkObject>(null);
    public Transform FeetPoint { get; private set; }
    public bool IsHeld => Status.Value == BallStatus.Active;
    public bool IsHeldBy(NetworkObject player) => IsHeld && Holder.Value == player;

    private Rigidbody _rb;
    private Collider _collider;
    private Collider[] _holderColliders;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _rb.isKinematic = true;
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        Instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.linearDamping = 0f;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public override void OnStopServer()
    {
        RestoreCollisions();
        _rb.isKinematic = true;
        base.OnStopServer();
    }

    public override void OnStopNetwork()
    {
        if (Instance == this) Instance = null;
        base.OnStopNetwork();
    }

    public void SetActive(NetworkObject player, Transform feetPoint)
    {
        if (!IsServerInitialized || player == null || feetPoint == null || IsHeld) return;
        FeetPoint = feetPoint;
        Holder.Value = player;
        Status.Value = BallStatus.Active;
        _holderColliders = player.GetComponentsInChildren<Collider>();
        foreach (Collider other in _holderColliders)
            if (other != null) Physics.IgnoreCollision(_collider, other, true);
        _rb.angularVelocity = Vector3.zero;
        _rb.WakeUp();
    }

    public void SetDefault()
    {
        if (!IsServerInitialized) return;
        RestoreCollisions();
        FeetPoint = null;
        Holder.Value = null;
        Status.Value = BallStatus.Default;
    }

    private void RestoreCollisions()
    {
        if (_holderColliders == null) return;
        foreach (Collider other in _holderColliders)
            if (other != null) Physics.IgnoreCollision(_collider, other, false);
        _holderColliders = null;
    }
}

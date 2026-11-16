using FishNet.Object;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BallState))]
public class BallKick : NetworkBehaviour
{
    public static BallKick Instance { get; private set; }

    [SerializeField] private float _kickForce = 12f;
    [SerializeField] private float _upwardForce = 15f;
    [SerializeField] private float _pickupBlockTime = 0.4f;

    private Rigidbody _rb;
    private BallState _state;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _state = GetComponent<BallState>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Instance = this;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        if (Instance == this) Instance = null;
    }

    public void TryKick(NetworkObject kicker, Vector3 direction)
    {
        if (!IsServerInitialized) return;

        // Бить может только тот, кто сейчас владеет мячом
        if (_state.Status.Value != BallStatus.Active) return;
        
        direction.Normalize();

        _state.SetDefault();

        if (kicker.TryGetComponent(out PlayerBallFinder finder))
            finder.BlockPickup(_pickupBlockTime);

        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(direction * _kickForce + Vector3.up * _upwardForce, ForceMode.Impulse);
    }
}
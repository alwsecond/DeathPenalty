using FishNet.Object;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BallState))]
public class BallLanding : NetworkBehaviour
{
    [SerializeField] private float _minFlyTime = 0.3f;   // раньше этого времени не проверяем (иначе мяч "приземлится" сразу после удара)
    [SerializeField] private float _maxFlyTime = 5f;     // страховка: через это время вернём в Default в любом случае
    [SerializeField] private float _groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask _groundMask = ~0; // слои земли

    private Rigidbody _rb;
    private BallState _state;
    private float _flyStartTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _state = GetComponent<BallState>();
        enabled = false;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _state.Status.OnChange += OnStatusChanged;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        _state.Status.OnChange -= OnStatusChanged;
    }

    private void OnStatusChanged(BallStatus prev, BallStatus next, bool asServer)
    {
        if (!asServer) return;

        enabled = next == BallStatus.Flying;
        if (enabled)
            _flyStartTime = Time.time;
    }

    private void Update()
    {
        float flyTime = Time.time - _flyStartTime;

        // Страховка, чтобы мяч не завис в Flying навсегда
        if (flyTime >= _maxFlyTime)
        {
            _state.SetDefault();
            return;
        }

        if (flyTime < _minFlyTime) return;

        bool grounded = Physics.Raycast(transform.position, Vector3.down,
            _groundCheckDistance, _groundMask, QueryTriggerInteraction.Ignore);

        if (grounded)
            _state.SetDefault();
    }
}
using UnityEngine;

using FishNet.Object;

[RequireComponent(typeof(Rigidbody), typeof(BallState))]
public class BallDribble : NetworkBehaviour
{
    [SerializeField] private float _dribbleSpeed = 8f;  // насколько резко мяч тянется к ногам
    [SerializeField] private float _maxSpeed = 12f;     // ограничение скорости ведения

    private Rigidbody _rb;
    private BallState _state;

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
        enabled = _state.Status.Value == BallStatus.Active;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        _state.Status.OnChange -= OnStatusChanged;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        // На чистом клиенте позицию задаёт NetworkTransform, физика там не нужна
        if (!IsServerInitialized)
            _rb.isKinematic = true;
    }

    private void OnStatusChanged(BallStatus prev, BallStatus next, bool asServer)
    {
        if (!asServer) return;
        enabled = next == BallStatus.Active;
    }

    private void FixedUpdate()
    {
        Transform feet = _state.FeetPoint;
        
        if (feet == null)
        {
            _state.SetDefault();
            return;
        }
        
        Vector3 toFeet = feet.position - transform.position;
        toFeet.y = 0f;

        Vector3 velocity = Vector3.ClampMagnitude(toFeet * _dribbleSpeed, _maxSpeed);
        velocity.y = _rb.linearVelocity.y;
        _rb.linearVelocity = velocity;
    }
}

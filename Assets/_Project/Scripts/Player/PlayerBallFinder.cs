using FishNet.Object;
using UnityEngine;

public class PlayerBallFinder : NetworkBehaviour
{
    [SerializeField] private Transform _feetPoint;
    [SerializeField, Min(0.1f)] private float _magnetRadius = 1.2f;

    public Transform FeetPoint => _feetPoint != null ? _feetPoint : transform;
    
    private DynamicCrosshair _crosshair;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner)
            _crosshair = DynamicCrosshair.Instance;
    }
    
    private void FixedUpdate()
    {
        if (!IsOwner) return;

        BallState ball = BallState.Instance;
        if (ball == null || ball.IsHeld) return;
        if ((ball.transform.position - FeetPoint.position).sqrMagnitude > _magnetRadius * _magnetRadius) return;
        
        _crosshair.Expand();
        ball.SetActive(NetworkObject, FeetPoint);
    }

    public override void OnStopServer()
    {
        BallState ball = BallState.Instance;
        if (ball != null && ball.IsHeldBy(NetworkObject)) ball.SetDefault();
        StrikerState roles = StrikerState.Instance;
        if (roles != null && roles.IsStriker(NetworkObject)) roles.Clear();
        base.OnStopServer();
    }
}
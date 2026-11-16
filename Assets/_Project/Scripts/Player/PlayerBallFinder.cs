using System;
using FishNet.Object;
using UnityEngine;

public class PlayerBallFinder : NetworkBehaviour
{
    [SerializeField] private Transform _feetPoint;
    [SerializeField] private float _magnetRadius = 1.2f;
    [SerializeField] private bool _onlyHostTest = true;

    public Transform FeetPoint => _feetPoint != null ? _feetPoint : transform;

    private BallState _ball;
    private float _blockedUntil;

    private void Awake() => enabled = false;

    public override void OnStartServer()
    {
        base.OnStartServer();
        enabled = true;
        TrySubscribe();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        if (_ball != null)
            _ball.Status.OnChange -= OnBallStatusChanged;
        _ball = null;
    }

    // Вызывает BallKick после удара: "этот игрок не может взять мяч N секунд"
    public void BlockPickup(float seconds) => _blockedUntil = Time.time + seconds;

    private void TrySubscribe()
    {
        _ball = BallState.Instance;
        if (_ball == null) return;

        _ball.Status.OnChange += OnBallStatusChanged;
        enabled = _ball.Status.Value == BallStatus.Default;
    }

    private void OnBallStatusChanged(BallStatus prev, BallStatus next, bool asServer)
    {
        if (!asServer) return;
        enabled = next == BallStatus.Default;
    }

    private void Update()
    {
        if (_ball == null) { TrySubscribe(); return; }

        if (_onlyHostTest && !IsOwner) return;
        if (Time.time < _blockedUntil) return;

        float r = _magnetRadius;
        if ((_ball.transform.position - FeetPoint.position).sqrMagnitude > r * r) return;

        if (_ball.Status.Value != BallStatus.Default) return; // защита от двух игроков в одном кадре

        _ball.SetActive(NetworkObject, FeetPoint);
    }
}
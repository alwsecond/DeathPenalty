using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;

public enum BallStatus
{
    Idle,
    Active
}

public class BallState : NetworkBehaviour
{
    public static event Action<bool> OnLocalHoldChanged;

    private readonly SyncVar<BallStatus> _currentState = new SyncVar<BallStatus>(BallStatus.Idle);
    private readonly SyncVar<NetworkConnection> _currentHolder = new SyncVar<NetworkConnection>();

    private bool _predictedLocalHold;

    public BallStatus CurrentState => _currentState.Value;
    public NetworkConnection CurrentHolder => _currentHolder.Value;

    public bool IsLocallyHeld => _predictedLocalHold ||
                                 (_currentState.Value == BallStatus.Active && _currentHolder.Value == ClientManager.Connection);

    public override void OnStartNetwork()
    {
        _currentState.OnChange += OnSyncChanged;
        _currentHolder.OnChange += OnSyncChanged;
    }

    public override void OnStopNetwork()
    {
        _currentState.OnChange -= OnSyncChanged;
        _currentHolder.OnChange -= OnSyncChanged;
    }

    public void PredictLocalHold()
    {
        if (_predictedLocalHold) return;
        _predictedLocalHold = true;
        OnLocalHoldChanged?.Invoke(true);
    }

    private void OnSyncChanged<T>(T prev, T next, bool asServer)
    {
        if (asServer) return;
        _predictedLocalHold = false;
        OnLocalHoldChanged?.Invoke(IsLocallyHeld);
    }

    [Server]
    public void SetState(BallStatus newState, NetworkConnection newHolder)
    {
        _currentState.Value = newState;
        _currentHolder.Value = newHolder;
    }
}
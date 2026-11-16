using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKick : NetworkBehaviour
{
    private void Update()
    {
        if (!IsOwner) return;

        BallState ball = BallState.Instance;
        if (ball == null) return;

        // Бить можно, только если мяч Active и холдер это я
        if (ball.Status.Value != BallStatus.Active) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            KickServerRpc(transform.forward);
    }

    [ServerRpc]
    private void KickServerRpc(Vector3 direction)
    {
        if (BallKick.Instance == null) return;
        BallKick.Instance.TryKick(NetworkObject, direction);
    }
}
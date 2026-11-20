using FishNet.Object;
using UnityEngine;

public class FeetTrigger : NetworkBehaviour
{
    [SerializeField] private float _pickupRadius = 1.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;

        if (other.TryGetComponent(out BallState ball))
        {
            if (ball.CurrentState != BallStatus.Idle) return;

            RequestPickUpServerRpc(ball);
        }
    }

    [ServerRpc]
    private void RequestPickUpServerRpc(BallState ball)
    {
        if (ball == null) return;
        if (ball.CurrentState != BallStatus.Idle) return;
        
        ball.PredictLocalHold();

        float maxDist = _pickupRadius + 0.5f;
        if ((ball.transform.position - transform.position).sqrMagnitude > maxDist * maxDist) return;
        
        ball.SetState(BallStatus.Active, Owner);
    }
}
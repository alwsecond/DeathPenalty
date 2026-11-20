using FishNet.Object;
using UnityEngine;

public class BallFollow : NetworkBehaviour
{
    // [SerializeField, Min(1f)] private float _dribbleSpeed = 18f;
    // [SerializeField, Min(1f)] private float _maxSpeed = 22f;
    // [SerializeField, Min(0.5f)] private float _releaseDistance = 1.5f;
    //
    // private Rigidbody _rb;
    // private BallState _state;
    // private SphereCollider _sphere;
    //
    // private void Awake()
    // {
    //     _rb = GetComponent<Rigidbody>();
    //     _state = GetComponent<BallState>();
    //     _sphere = GetComponent<SphereCollider>();
    // }
    //
    // public override void OnStartClient()
    // {
    //     base.OnStartClient();
    //     _rb.isKinematic = !IsOwner && !IsServerInitialized; 
    // }
    //
    // private void FixedUpdate()
    // {
    //     if (!IsServerInitialized) return;
    //
    //     if (!_state.IsHeld)
    //     {
    //         _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
    //         return;
    //     }
    //
    //     Transform feet = _state.FeetPoint;
    //     if (feet == null || _state.Holder.Value == null || Vector3.Distance(feet.position, _rb.position) > _releaseDistance)
    //     {
    //         _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
    //         _state.SetDefault();
    //         return;
    //     }
    //
    //     _rb.constraints |= RigidbodyConstraints.FreezePositionY;
    //
    //     Vector3 flatTarget = new Vector3(feet.position.x, _rb.position.y, feet.position.z);
    //     Vector3 horizontalVelocity = Vector3.ClampMagnitude((flatTarget - _rb.position) * _dribbleSpeed, _maxSpeed);
    //
    //     _rb.linearVelocity = new Vector3(horizontalVelocity.x, 0f, horizontalVelocity.z);
    //
    //     ApplyRollingRotation(horizontalVelocity);
    // }
    //
    // private void ApplyRollingRotation(Vector3 horizontalVelocity)
    // {
    //     float speed = horizontalVelocity.magnitude;
    //     if (speed < 0.05f) return;
    //
    //     float radius = _sphere.radius * Mathf.Max(
    //         Mathf.Abs(transform.lossyScale.x),
    //         Mathf.Abs(transform.lossyScale.y),
    //         Mathf.Abs(transform.lossyScale.z));
    //
    //     Vector3 rotationAxis = Vector3.Cross(Vector3.up, horizontalVelocity.normalized);
    //     _rb.angularVelocity = rotationAxis * (speed / radius);
    // }
}
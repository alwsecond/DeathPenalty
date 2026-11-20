using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;

public class ChargeKick : NetworkBehaviour
{
    // [SerializeField] private float chargeTime = 1f;
    // [SerializeField] private float aimSensitivity = 0.002f;
    //
    // public event Action<float, Vector2> Kicked;
    //
    // private PlayerController _playerController;
    // private PlayerInput _input;
    // private DynamicCrosshair _crosshair;
    // private bool _charging;
    // private float _charge;
    // private Vector2 _aim;
    //
    // private BallState _ballState;
    //
    // private void Awake()
    // {
    //     _input = GetComponent<PlayerInput>();
    //     _playerController = GetComponent<PlayerController>();
    // }
    //
    // public override void OnStartClient()
    // {
    //     base.OnStartClient();
    //
    //     enabled = IsOwner;
    //     if (!IsOwner) return;
    //
    //     _crosshair = DynamicCrosshair.Instance;
    // }
    //
    // private void Update()
    // {
    //     if (_crosshair == null) return;
    //
    //     if (Cursor.lockState != CursorLockMode.Locked)
    //     {
    //         if (_charging) StopCharge();
    //         return;
    //     }
    //
    //     InputAction attack = _input.actions["Attack"];
    //
    //     if (attack.WasPressedThisFrame())
    //         StartCharge();
    //
    //     if (!_charging) return;
    //
    //     if (attack.WasReleasedThisFrame())
    //     {
    //         float offset = _aim.magnitude;
    //         Debug.Log($"Kick: сила {_charge:0.00} | сдвиг {offset:0.00} ({offset * 100f:0}%) | x {_aim.x:0.00}, y {_aim.y:0.00}");
    //
    //         Kicked?.Invoke(_charge, _aim);
    //         StopCharge();
    //         return;
    //     }
    //
    //     _charge = Mathf.MoveTowards(_charge, 1f, Time.deltaTime / chargeTime);
    //
    //     Vector2 look = _input.actions["Look"].ReadValue<Vector2>();
    //     _aim = Vector2.ClampMagnitude(_aim + look * aimSensitivity, 1f);
    //
    //     _crosshair.SetCharge(_charge);
    //     _crosshair.SetAim(_aim);
    // }
    //
    // private void StartCharge()
    // {
    //     _charging = true;
    //     _charge = 0f;
    //     _aim = Vector2.zero;
    // }
    //
    // private void StopCharge()
    // {
    //     _charging = false;
    //     _crosshair.SetCharge(0f);
    // }
}
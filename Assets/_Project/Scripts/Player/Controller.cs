using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class Controller : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float acceleration = 70f;
    [SerializeField] private float deceleration = 55f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -25f;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private Camera playerCamera;

    private CharacterController _controller;
    private PlayerInput _input;
    private AudioListener _audioListener;
    private float _pitch;
    private float _yVelocity;
    private Vector3 _horizontalVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInput>();
        _audioListener = playerCamera.GetComponent<AudioListener>();

        playerCamera.enabled = false;
        _audioListener.enabled = false;
        _input.enabled = false;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        enabled = IsOwner;
        if (!IsOwner) return;

        _input.enabled = true;
        playerCamera.enabled = true;
        _audioListener.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
        Move();
    }

    private void Look()
    {
        Vector2 look = _input.actions["Look"].ReadValue<Vector2>() * mouseSensitivity;

        _pitch = Mathf.Clamp(_pitch - look.y, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        transform.Rotate(0f, look.x, 0f);
    }

    private void Move()
    {
        Vector2 input = _input.actions["Move"].ReadValue<Vector2>();

        if (_controller.isGrounded)
        {
            _yVelocity = -2f;

            if (_input.actions["Jump"].WasPressedThisFrame())
                _yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _yVelocity += gravity * Time.deltaTime;

        Vector3 targetVelocity = (transform.right * input.x + transform.forward * input.y) * moveSpeed;
        float rate = input.sqrMagnitude > 0.01f ? acceleration : deceleration;
        _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, targetVelocity, rate * Time.deltaTime);

        _controller.Move((_horizontalVelocity + Vector3.up * _yVelocity) * Time.deltaTime);
    }
}
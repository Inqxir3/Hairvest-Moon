using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Centralized input manager that handles movement, look direction, and control mode switching.
/// Supports automatic detection between Mouse and Gamepad.
/// </summary>
public enum ControlMode { Mouse, Gamepad }

public class InputController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public static InputController Instance { get; private set; }

    [SerializeField] private PlayerInput playerInput;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    /// <summary> True if the look input was actively moved this frame. </summary>
    public bool LookInputThisFrame { get; private set; }

    public ControlMode CurrentMode { get; private set; } = ControlMode.Mouse;
    public event Action<ControlMode> OnControlModeChanged;
    public event Action OnToolNext;
    public event Action OnToolPrevious;

    private InputSystem_Actions _input;

    private Vector2 _mouseLook;
    private Vector2 _gamepadLook;
    private Vector2 _lastGamepadLookDir = Vector2.right;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        _input = new InputSystem_Actions();
        _input.Player.SetCallbacks(this);
        _input.Player.Enable();

        playerInput.onControlsChanged += HandleControlsChanged;
    }

    private void HandleControlsChanged(PlayerInput input)
    {
        var scheme = input.currentControlScheme;
        var newMode = scheme == "Gamepad" ? ControlMode.Gamepad : ControlMode.Mouse;

        if (newMode != CurrentMode)
        {
            Debug.Log($"[InputController] Switched control scheme: {scheme} → {newMode}");
            CurrentMode = newMode;
            OnControlModeChanged?.Invoke(CurrentMode);
        }
    }

    private void Update()
    {
        LookInputThisFrame = false;

        if (CurrentMode == ControlMode.Gamepad)
        {
            LookInput = _lastGamepadLookDir;

            if (_gamepadLook.sqrMagnitude > 0.1f)
            {
                _lastGamepadLookDir = _gamepadLook;
                LookInputThisFrame = true;
            }
        }
        else if (CurrentMode == ControlMode.Mouse)
        {
            LookInput = _mouseLook;

            if (Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0.01f)
            {
                LookInputThisFrame = true;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (Mouse.current != null)
        {
            _mouseLook = context.ReadValue<Vector2>();

            if (Mouse.current.delta.ReadValue().sqrMagnitude > 0.01f && CurrentMode != ControlMode.Mouse)
            {
                Debug.Log("[InputController] Switching to Mouse due to movement.");
                CurrentMode = ControlMode.Mouse;
                OnControlModeChanged?.Invoke(CurrentMode);
            }
        }
    }

    public void OnGamepadLook(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _gamepadLook = input;

        if (input.sqrMagnitude > 0.1f)
        {
            _lastGamepadLookDir = input;

            if (CurrentMode != ControlMode.Gamepad)
            {
                Debug.Log("[InputController] Switching to Controller due to movement.");
                CurrentMode = ControlMode.Gamepad;
                OnControlModeChanged?.Invoke(CurrentMode);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context) { }
    public void OnInteract(InputAction.CallbackContext context) { }
    public void OnCrouch(InputAction.CallbackContext context) { }
    public void OnJump(InputAction.CallbackContext context) { }
    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnToolNext?.Invoke();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnToolPrevious?.Invoke();
    }
    public void OnSprint(InputAction.CallbackContext context) { }
}

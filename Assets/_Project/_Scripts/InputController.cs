using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlMode { Mouse, Gamepad }


public class InputController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public static InputController Instance { get; private set; }

    [SerializeField] private PlayerInput playerInput;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public ControlMode CurrentMode { get; private set; } = ControlMode.Mouse;
    public event Action<ControlMode> OnControlModeChanged;

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
            CurrentMode = newMode;
            OnControlModeChanged?.Invoke(CurrentMode);
        }
    }

    private void Update()
    {
        LookInput = CurrentMode switch
        {
            ControlMode.Gamepad => _lastGamepadLookDir,
            ControlMode.Mouse => _mouseLook,
            _ => Vector2.zero
        };
        Debug.Log($"[InputController] Mode: {CurrentMode}, Move: {MoveInput}, Look: {LookInput}");
    }

    // === Input System Callbacks ===

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (CurrentMode == ControlMode.Mouse)
            _mouseLook = context.ReadValue<Vector2>(); // this stays for mouse
    }


    public void OnGamepadLook(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Debug.Log($"[GamepadLook] Raw input: {input}");

        _gamepadLook = input;

        if (input.sqrMagnitude > 0.1f)
        {
            _lastGamepadLookDir = input;
            Debug.Log($"[GamepadLook] Set last direction: {_lastGamepadLookDir}");
        }
    }


    public void OnAttack(InputAction.CallbackContext context) { /* Forward to listeners if needed */ }
    public void OnInteract(InputAction.CallbackContext context) { }
    public void OnCrouch(InputAction.CallbackContext context) { }
    public void OnJump(InputAction.CallbackContext context) { }
    public void OnPrevious(InputAction.CallbackContext context) { }
    public void OnNext(InputAction.CallbackContext context) { }
    public void OnSprint(InputAction.CallbackContext context) { }
}

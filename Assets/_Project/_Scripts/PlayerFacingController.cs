using UnityEngine;

public class PlayerFacingController : MonoBehaviour
{
    public static PlayerFacingController Instance { get; private set; }

    public enum FacingDirection { Up, Down, Left, Right }
    public FacingDirection CurrentFacing { get; private set; } = FacingDirection.Right;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void UpdateFacing(Vector2 moveInput, Vector2 lookInput, ControlMode mode)
    {
        // Priority 1: Movement input
        if (moveInput.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
                CurrentFacing = moveInput.x > 0 ? FacingDirection.Right : FacingDirection.Left;
            else
                CurrentFacing = moveInput.y > 0 ? FacingDirection.Up : FacingDirection.Down;

            return;
        }

        // Priority 2: Look input (when idle)
        if (lookInput.sqrMagnitude < 0.01f) return;

        if (mode == ControlMode.Gamepad)
        {
            if (lookInput.sqrMagnitude < 0.05f) return;  // ← consider removing this check now

            if (Mathf.Abs(lookInput.x) > Mathf.Abs(lookInput.y))
                CurrentFacing = lookInput.x > 0 ? FacingDirection.Right : FacingDirection.Left;
            else
                CurrentFacing = lookInput.y > 0 ? FacingDirection.Up : FacingDirection.Down;
        }
        else if (mode == ControlMode.Mouse)
        {
            Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(Player_Controller.Position);
            Vector2 delta = lookInput - playerScreenPos;
            if (delta.sqrMagnitude < 100f) return;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                CurrentFacing = delta.x > 0 ? FacingDirection.Right : FacingDirection.Left;
            else
                CurrentFacing = delta.y > 0 ? FacingDirection.Up : FacingDirection.Down;
        }
        Debug.Log($"[Facing] Input: {lookInput}, Mode: {mode}, Result: {CurrentFacing}");

    }

    public Vector3Int GetFacingOffset()
    {
        return CurrentFacing switch
        {
            FacingDirection.Up => Vector3Int.up,
            FacingDirection.Down => Vector3Int.down,
            FacingDirection.Left => Vector3Int.left,
            FacingDirection.Right => Vector3Int.right,
            _ => Vector3Int.zero
        };
    }
}



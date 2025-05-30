using HairvestMoon.Core;
using UnityEngine;

namespace HairvestMoon.Player
{
    /// <summary>
    /// Determines the player's current facing direction based on movement or look input.
    /// Prioritizes movement, and defers to look input only after intentional use.
    /// </summary>
    public class PlayerFacingController : MonoBehaviour
    {
        public static PlayerFacingController Instance { get; private set; }

        public enum FacingDirection { Up, Down, Left, Right }
        public FacingDirection CurrentFacing { get; private set; } = FacingDirection.Right;

        private FacingDirection _lastMoveFacing = FacingDirection.Right;
        private FacingSource _currentSource = FacingSource.Movement;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;
        }

        /// <summary>
        /// Updates the facing direction each frame based on input and control mode.
        /// </summary>
        public void UpdateFacing(Vector2 moveInput, Vector2 lookInput, ControlMode mode)
        {
            bool isMoving = moveInput.sqrMagnitude > 0.01f;
            bool lookInputActive = InputController.Instance.LookInputThisFrame;

            if (isMoving)
            {
                _currentSource = FacingSource.Movement;
                CurrentFacing = FromVector(moveInput);
                _lastMoveFacing = CurrentFacing;
                return;
            }

            if (_currentSource == FacingSource.Movement && lookInputActive)
            {
                _currentSource = FacingSource.Look;
            }

            if (_currentSource == FacingSource.Look && lookInputActive)
            {
                Vector2 direction;

                if (mode == ControlMode.Mouse)
                {
                    Vector2 worldMouse = Camera.main.ScreenToWorldPoint(lookInput);
                    direction = worldMouse - (Vector2)transform.position;
                }
                else
                {
                    direction = lookInput; // already a direction vector from stick
                }

                CurrentFacing = FromVector(direction);
            }
            else
            {
                CurrentFacing = _lastMoveFacing;
            }
        }

        private FacingDirection FromVector(Vector2 input)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                return input.x > 0 ? FacingDirection.Right : FacingDirection.Left;
            else
                return input.y > 0 ? FacingDirection.Up : FacingDirection.Down;
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

        private enum FacingSource
        {
            Movement,
            Look
        }
    }
}
using HairvestMoon.Core;
using UnityEngine;

namespace HairvestMoon.Player
{
    [SelectionBase]
    public class Player_Controller : MonoBehaviour
    {
        public static Vector3 Position => Instance?.transform.position ?? Vector3.zero;

        [Header("Dependencies")]
        [SerializeField] private Rigidbody2D _rb;

        private static Player_Controller Instance;

        private bool _canMove = true;
        private Vector2 _moveDir = Vector2.zero;

        private Animator _animator => PlayerStateController.Instance.CurrentAnimator;
        private SpriteRenderer _spriteRenderer => PlayerStateController.Instance.CurrentSpriteRenderer;
        private float MoveSpeed => PlayerStateController.Instance.MoveSpeed;

        private readonly int _animeIdleSide = Animator.StringToHash("AN_Character_Farmer_Idle_Side");
        private readonly int _animeIdleUp = Animator.StringToHash("AN_Character_Farmer_Idle_Up");
        private readonly int _animeIdleDown = Animator.StringToHash("AN_Character_Farmer_Idle_Down");
        private readonly int _animeMoveSide = Animator.StringToHash("AN_Character_Farmer_Walk_Side");
        private readonly int _animeMoveUp = Animator.StringToHash("AN_Character_Farmer_Walk_Up");
        private readonly int _animeMoveDown = Animator.StringToHash("AN_Character_Farmer_Walk_Down");

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void OnEnable()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void Update()
        {
            _moveDir = _canMove ? InputController.Instance.MoveInput : Vector2.zero;

            PlayerFacingController.Instance.UpdateFacing(
                _moveDir,
                InputController.Instance.LookInput,
                InputController.Instance.CurrentMode
            );

            UpdateAnimation(PlayerFacingController.Instance.CurrentFacing);
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _moveDir.normalized * MoveSpeed * Time.fixedDeltaTime;
        }

        private void OnGameStateChanged(GameState newState)
        {
            _canMove = newState == GameState.FreeRoam;
        }

        private void UpdateAnimation(PlayerFacingController.FacingDirection facing)
        {
            switch (facing)
            {
                case PlayerFacingController.FacingDirection.Left: _spriteRenderer.flipX = true; break;
                case PlayerFacingController.FacingDirection.Right: _spriteRenderer.flipX = false; break;
            }

            bool isMoving = _moveDir.sqrMagnitude > 0.01f;
            int anim = _animeIdleSide;

            switch (facing)
            {
                case PlayerFacingController.FacingDirection.Up:
                    anim = isMoving ? _animeMoveUp : _animeIdleUp; break;
                case PlayerFacingController.FacingDirection.Down:
                    anim = isMoving ? _animeMoveDown : _animeIdleDown; break;
                case PlayerFacingController.FacingDirection.Left:
                case PlayerFacingController.FacingDirection.Right:
                    anim = isMoving ? _animeMoveSide : _animeIdleSide; break;
            }

            _animator.CrossFade(anim, 0);
        }
    }
}
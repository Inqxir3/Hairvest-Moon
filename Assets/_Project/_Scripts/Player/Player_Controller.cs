using HairvestMoon.Core;
using UnityEngine;

namespace HairvestMoon.Player
{
    [SelectionBase]
    public class Player_Controller : MonoBehaviour, IBusListener
    {
        public Vector3 Position => transform.position;

        [Header("Dependencies")]
        [SerializeField] private Rigidbody2D _rb;

        private bool _canMove = true;
        private Vector2 _moveDir = Vector2.zero;

        private Animator _animator => ServiceLocator.Get<PlayerStateController>().CurrentAnimator;
        private SpriteRenderer _spriteRenderer => ServiceLocator.Get<PlayerStateController>().CurrentSpriteRenderer;
        private float MoveSpeed => ServiceLocator.Get<PlayerStateController>().MoveSpeed;

        private readonly int _animeIdleSide = Animator.StringToHash("AN_Character_Farmer_Idle_Side");
        private readonly int _animeIdleUp = Animator.StringToHash("AN_Character_Farmer_Idle_Up");
        private readonly int _animeIdleDown = Animator.StringToHash("AN_Character_Farmer_Idle_Down");
        private readonly int _animeMoveSide = Animator.StringToHash("AN_Character_Farmer_Walk_Side");
        private readonly int _animeMoveUp = Animator.StringToHash("AN_Character_Farmer_Walk_Up");
        private readonly int _animeMoveDown = Animator.StringToHash("AN_Character_Farmer_Walk_Down");


        public void RegisterBusListeners()
        {
            var bus = ServiceLocator.Get<GameEventBus>();
            bus.GameStateChanged += OnGameStateChanged;
        }

        private void Update()
        {
            _moveDir = _canMove ? ServiceLocator.Get<InputController>().MoveInput : Vector2.zero;

            ServiceLocator.Get<PlayerFacingController>().UpdateFacing(
                _moveDir,
                ServiceLocator.Get<InputController>().LookInput,
                ServiceLocator.Get<InputController>().CurrentMode
            );

            UpdateAnimation(ServiceLocator.Get<PlayerFacingController>().CurrentFacing);
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _moveDir.normalized * MoveSpeed * Time.fixedDeltaTime;
        }

        private void OnGameStateChanged(GameStateChangedArgs args)
        {
            var newState = args.State;
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
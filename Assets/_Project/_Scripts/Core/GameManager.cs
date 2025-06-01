using HairvestMoon.Player;
using HairvestMoon.UI;
using UnityEngine;

namespace HairvestMoon.Core
{
    // Singleton
    // Subscribes to OnDusk and OnDawn
    // Triggers werewolf transformation and state changes

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private GameTimeManager _timeManager;
        [SerializeField] private PlayerStateController _playerState;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            _timeManager.OnDusk += () => _playerState.EnterWerewolfForm();
            _timeManager.OnDawn += () => {
                GameStateManager.Instance.SetState(GameState.FreeRoam);
                _playerState.ExitWerewolfForm();
            };

            InputController.Instance.OnMenuToggle += HandleMenuToggle;
        }

        private void HandleMenuToggle()
        {
            if (GameStateManager.Instance.CurrentState == GameState.FreeRoam)
                MainMenuUIManager.Instance.OpenMenu();
            else if (GameStateManager.Instance.CurrentState == GameState.Menu)
                MainMenuUIManager.Instance.CloseMenu();
        }
    }
}
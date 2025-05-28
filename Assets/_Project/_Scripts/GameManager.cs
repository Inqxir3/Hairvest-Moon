using UnityEngine;

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
        _timeManager.OnDusk += () =>
        {
            GameStateManager.Instance.SetState(GameState.Transformation);
            _playerState.EnterWerewolfForm();
        };

        _timeManager.OnDawn += () =>
        {
            GameStateManager.Instance.SetState(GameState.Gameplay);
            _playerState.ExitWerewolfForm();
        };
    }
}

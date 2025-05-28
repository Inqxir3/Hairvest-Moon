using UnityEngine;
using TMPro;

// UI script for debugging: shows time, day, current form, and game state
// Automatically updates from GameTimeManager, GameStateManager, PlayerStateController

public class DebugUIOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI formText;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI toolText;
    [SerializeField] private TextMeshProUGUI lastActionText;

    public static DebugUIOverlay Instance { get; private set; }

    private void Awake() => Instance = this;

    private void Start()
    {
        if (GameTimeManager.Instance != null)
        {
            UpdateTimeText(GameTimeManager.Instance.CurrentHour, GameTimeManager.Instance.CurrentMinute);
            GameTimeManager.Instance.OnTimeChanged += UpdateTimeText;
        }

        if (GameStateManager.Instance != null)
        {
            UpdateStateText(GameStateManager.Instance.CurrentState);
            GameStateManager.Instance.OnGameStateChanged += UpdateStateText;
        }
    }

    private void Update()
    {
        if (GameTimeManager.Instance != null)
            dayText.text = $"Day: {GameTimeManager.Instance.Day}";

        if (PlayerStateController.Instance != null)
            formText.text = $"Form: {PlayerStateController.Instance.CurrentForm}";

        //toolText.text = $"Tool: {FarmToolHandler.CurrentSlot}";
    }

    private void UpdateTimeText(int hour, int minute)
    {
        timeText.text = $"Time: {hour:00}:{minute:00}";
    }

    private void UpdateStateText(GameState state)
    {
        stateText.text = $"GameState: {state}";
    }

    public void ShowLastAction(string text) => lastActionText.text = $"Last Action: {text}";

    private void OnDestroy()
    {
        if (GameTimeManager.Instance != null)
            GameTimeManager.Instance.OnTimeChanged -= UpdateTimeText;

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged -= UpdateStateText;
    }
}



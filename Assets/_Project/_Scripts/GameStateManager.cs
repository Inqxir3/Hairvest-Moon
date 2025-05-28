using System;
using UnityEngine;

public enum GameState
{
    Gameplay,
    Dialogue,
    Cutscene,
    Pause,
    Transformation,
    NightTask
}

// Enum-driven state machine (Gameplay, Dialogue, Pause, etc.)
// Broadcasts changes via OnGameStateChanged
// Used to lock input, pause systems, manage cutscenes or tasks


public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
        SetState(GameState.Gameplay);
    }

    public void SetState(GameState newState)
    {
        if (newState == CurrentState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(CurrentState);
    }

    public bool IsGameplay() => CurrentState == GameState.Gameplay;
}
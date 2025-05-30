using System;
using UnityEngine;
namespace HairvestMoon.Core
{
    // Enum-driven state machine (Gameplay, Dialogue, Pause, etc.)
    // Broadcasts changes via OnGameStateChanged
    // Used to lock input, pause systems, manage cutscenes or tasks
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public GameState CurrentState { get; private set; }

        public event Action<GameState> OnGameStateChanged;
        public event Action<bool> OnInputLockChanged;

        public bool IsInputLocked { get; private set; } = false;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            SetState(GameState.FreeRoam);
        }

        public void SetState(GameState newState)
        {
            if (newState == CurrentState) return;

            CurrentState = newState;
            OnGameStateChanged?.Invoke(CurrentState);

            HandleTimeControl();
            HandleInputLock();
        }

        private void HandleTimeControl()
        {
            if (CurrentState == GameState.Menu || CurrentState == GameState.Dialogue || CurrentState == GameState.Cutscene)
                GameTimeManager.Instance.FreezeTime();
            else
                GameTimeManager.Instance.ResumeTime();
        }

        private void HandleInputLock()
        {
            bool shouldLock = (CurrentState != GameState.FreeRoam);
            if (shouldLock != IsInputLocked)
            {
                IsInputLocked = shouldLock;
                OnInputLockChanged?.Invoke(IsInputLocked);
            }
        }

        public bool IsFreeRoam() => CurrentState == GameState.FreeRoam;
    }
}

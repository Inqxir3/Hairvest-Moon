using System;
using UnityEngine;

namespace HairvestMoon.Core
{
    // Manages in-game clock, fires events on dawn and dusk
    // Notifies listeners every in-game minute
    // Provides GetFormattedTime() and IsNight() helpers

    public class GameTimeManager : MonoBehaviour
    {
        public static GameTimeManager Instance { get; private set; }

        [Header("Time Settings")]
        [SerializeField] private float secondsPerGameMinute = 1f;
        [SerializeField] private int dawnHour = 6;
        [SerializeField] private int duskHour = 18;

        public int CurrentHour { get; private set; } = 6;
        public int CurrentMinute { get; private set; } = 0;
        public int Day { get; private set; } = 1;
        public bool IsTimeFrozen { get; private set; } = false;
        public float TimeScale { get; private set; } = 1f;

        public event Action OnNewDay;
        public event Action OnDawn;
        public event Action OnDusk;
        public event Action OnNewHour;
        public event Action<int, int> OnTimeChanged;

        private float _timer;
        private bool _isNight = false;

        public void InitializeSingleton() { Instance = this; }

        private void Update()
        {
            if (IsTimeFrozen) return;

            _timer += Time.deltaTime * TimeScale;
            if (_timer >= secondsPerGameMinute)
            {
                _timer = 0f;
                AdvanceMinute();
            }
        }

        private void AdvanceMinute()
        {
            CurrentMinute++;
            if (CurrentMinute >= 60)
            {
                CurrentMinute = 0;
                CurrentHour++;

                if (CurrentHour >= 24)
                {
                    CurrentHour = 0;
                    AdvanceDay();
                }

                CheckTimeTriggers();
            }

            OnTimeChanged?.Invoke(CurrentHour, CurrentMinute);
        }


        public void AdvanceDay()
        {
            Day++;
            OnDawn?.Invoke();
        }

        private void CheckTimeTriggers()
        {
            if (!_isNight && CurrentHour >= duskHour)
            {
                _isNight = true;
                OnDusk?.Invoke();
            }
            else if (_isNight && CurrentHour >= dawnHour && CurrentHour < duskHour)
            {
                _isNight = false;
                OnDawn?.Invoke();
            }
        }

        public void SetTimeScale(float scale)
        {
            TimeScale = Mathf.Max(0f, scale);
        }

        public void FastForwardToHour(int targetHour)
        {
            while (CurrentHour != targetHour)
            {
                AdvanceMinute(); // or AdvanceHour() if you make that helper
            }
        }


        public float GetCurrentHourProgress()
        {
            return (float)CurrentMinute / 60f;
        }

        public string GetFormattedTime() => $"Day {Day} - {CurrentHour:00}:{CurrentMinute:00}";

        public void FreezeTime() => IsTimeFrozen = true;
        public void ResumeTime() => IsTimeFrozen = false;
        public bool IsNight() => _isNight;
        public bool IsMorning() => CurrentHour >= 6 && CurrentHour < 12;
        public bool IsEvening() => CurrentHour >= 17 && CurrentHour < 20;


    }
}
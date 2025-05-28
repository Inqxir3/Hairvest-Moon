using System;
using UnityEngine;

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

    public event Action OnDawn;
    public event Action OnDusk;
    public event Action<int, int> OnTimeChanged;

    private float _timer;
    private bool _isNight = false;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
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
                Day++;
            }

            CheckTimeTriggers();
        }

        OnTimeChanged?.Invoke(CurrentHour, CurrentMinute);
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

    public bool IsNight() => _isNight;
    public string GetFormattedTime() => $"Day {Day} - {CurrentHour:00}:{CurrentMinute:00}";
}

using HairvestMoon.Core;
using UnityEngine;

namespace HairvestMoon.Farming
{
    /// <summary>
    /// Handles daily crop growth only.
    /// Subscribes to GameTimeManager.OnDawn to tick crops once per day.
    /// </summary>
    public class FarmGrowthSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            GameTimeManager.Instance.OnTimeChanged += HandleMinuteGrowthTick;
        }

        private void OnDisable()
        {
            GameTimeManager.Instance.OnTimeChanged -= HandleMinuteGrowthTick;
        }

        private void HandleMinuteGrowthTick(int hour, int minute)
        {
            foreach (var entry in FarmTileDataManager.Instance.AllTileData)
            {
                var data = entry.Value;

                if (!data.isTilled || data.plantedCrop == null || !data.isWatered)
                    continue;

                float growthPerMinute = data.plantedCrop.growthRateModifier;
                data.wateredMinutesAccumulated += growthPerMinute;
            }
        }
    }

}
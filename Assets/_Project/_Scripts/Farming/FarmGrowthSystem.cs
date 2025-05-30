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
            GameTimeManager.Instance.OnDawn += HandleDailyGrowthTick;
        }

        private void OnDisable()
        {
            GameTimeManager.Instance.OnDawn -= HandleDailyGrowthTick;
        }

        private void HandleDailyGrowthTick()
        {
            foreach (var entry in FarmTileDataManager.Instance.AllTileData)
            {
                var pos = entry.Key;
                var data = entry.Value;

                if (!data.isTilled)
                    continue;

                if (data.plantedCrop != null && data.isWatered)
                {
                    data.growthDays++;

                    if (data.growthDays >= data.plantedCrop.growthDuration)
                    {
                        Debug.Log($"Crop at {pos} fully grown!");
                        // Future: Mark as ready to harvest.
                    }
                }

                // No water decay here — handled by WaterDecaySystem
            }
        }
    }
}
using HairvestMoon.Core;
using UnityEngine;

namespace HairvestMoon.Farming
{

    /// <summary>
    /// Handles hourly water decay.
    /// Subscribes to GameTimeManager.OnNewHour.
    /// </summary>
    public class WaterDecaySystem : MonoBehaviour
    {
        private void OnEnable()
        {
            GameTimeManager.Instance.OnNewHour += HandleHourlyWaterDecay;
        }

        private void OnDisable()
        {
            GameTimeManager.Instance.OnNewHour -= HandleHourlyWaterDecay;
        }

        private void HandleHourlyWaterDecay()
        {
            foreach (var entry in FarmTileDataManager.Instance.AllTileData)
            {
                var pos = entry.Key;
                var data = entry.Value;

                if (!data.isWatered)
                    continue;

                data.waterHoursRemaining--;

                if (data.waterHoursRemaining <= 0)
                {
                    data.isWatered = false;
                    data.waterHoursRemaining = 0f;
                    FarmTileDataManager.Instance.UpdateWaterVisual(pos, data);
                }
            }
        }
    }

}
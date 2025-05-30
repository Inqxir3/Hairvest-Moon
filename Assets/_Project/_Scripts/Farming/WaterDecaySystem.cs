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

        [SerializeField] private WaterVisualSystem _waterVisualSystem;

        private void OnEnable()
        {
            GameTimeManager.Instance.OnTimeChanged += HandleWaterDecayPerMinute;
        }

        private void OnDisable()
        {
            GameTimeManager.Instance.OnTimeChanged -= HandleWaterDecayPerMinute;
        }

        private void HandleWaterDecayPerMinute(int hour, int minute)
        {
            foreach (var entry in FarmTileDataManager.Instance.AllTileData)
            {
                var pos = entry.Key;
                var data = entry.Value;

                if (!data.isWatered) continue;

                data.waterMinutesRemaining--;

                if (data.waterMinutesRemaining <= 0)
                {
                    data.isWatered = false;
                    data.waterMinutesRemaining = 0f;
                    FarmTileDataManager.Instance.UpdateWaterVisual(pos, data);
                    _waterVisualSystem.HandleWateredTile(pos, data); // Ensure visual despawn
                }
            }
        }
    }

}
using HairvestMoon.Core;
using UnityEngine;

namespace HairvestMoon.Farming
{
    /// <summary>
    /// Handles hourly water decay.
    /// Subscribes to GameTimeManager.OnNewHour.
    /// </summary>
    public class WaterDecaySystem : MonoBehaviour, IBusListener
    {
        public void RegisterBusListeners()
        {
            var bus = ServiceLocator.Get<GameEventBus>();
            bus.TimeChanged += OnWaterDecayTick;
        }

        private void OnWaterDecayTick(TimeChangedArgs args)
        {
            foreach (var entry in ServiceLocator.Get<FarmTileDataManager>().AllTileData)
            {
                var pos = entry.Key;
                var data = entry.Value;

                if (!data.isWatered) continue;

                data.waterMinutesRemaining--;

                if (data.waterMinutesRemaining <= 0)
                {
                    data.isWatered = false;
                    data.waterMinutesRemaining = 0f;
                    ServiceLocator.Get<FarmTileDataManager>().UpdateWaterVisual(pos, data);
                    ServiceLocator.Get<WaterVisualSystem>().HandleWateredTile(pos, data);
                }
            }
        }

    }

}
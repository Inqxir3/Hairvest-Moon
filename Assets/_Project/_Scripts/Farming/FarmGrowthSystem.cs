using HairvestMoon.Core;
using UnityEngine;

namespace HairvestMoon.Farming
{
    /// <summary>
    /// Handles daily crop growth only.
    /// Subscribes to GameTimeManager.OnDawn to tick crops once per day.
    /// </summary>
    public class FarmGrowthSystem : MonoBehaviour, IBusListener
    {
        public void RegisterBusListeners()
        {
            var bus = ServiceLocator.Get<GameEventBus>();
            bus.TimeChanged += OnMinuteGrowthTick;
        }

        private void OnMinuteGrowthTick(TimeChangedArgs args)
        {
            var hour = args.Hour;
            var minute = args.Minute;

            foreach (var entry in ServiceLocator.Get<FarmTileDataManager>().AllTileData)
            {
                var data = entry.Value;
                if (!data.isTilled || data.plantedCrop == null || !data.isWatered)
                    continue;

                data.wateredMinutesAccumulated += 1f;
            }
        }
    }

}
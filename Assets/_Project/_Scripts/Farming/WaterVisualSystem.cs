using HairvestMoon.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HairvestMoon.Farming
{
    /// <summary>
    /// Manages water progress bar visuals above watered tiles.
    /// Subscribes to hourly water updates.
    /// </summary>
    public class WaterVisualSystem : MonoBehaviour
    {
        [SerializeField] private GameObject waterSliderPrefab; 
        [SerializeField] private Grid farmGrid;
        [SerializeField] private float tileOffsetY = 1.0f;

        private Dictionary<Vector3Int, WaterSliderInstance> activeSliders = new();

        private void OnEnable()
        {
            GameTimeManager.Instance.OnNewHour += RefreshSliders;
        }

        private void OnDisable()
        {
            GameTimeManager.Instance.OnNewHour -= RefreshSliders;
        }

        private void Update()
        {
            foreach (var entry in activeSliders)
            {
                var pos = entry.Key;
                var data = FarmTileDataManager.Instance.GetTileData(pos);

                float partialHourProgress = GameTimeManager.Instance.GetCurrentHourProgress();
                float totalHoursRemaining = data.waterHoursRemaining - partialHourProgress;

                float fill = totalHoursRemaining / FarmTileDataManager.HoursPerWatering;
                entry.Value.SetFill(Mathf.Clamp01(fill));
            }
        }


        private void RefreshSliders()
        {
            foreach (var entry in FarmTileDataManager.Instance.AllTileData)
            {
                var pos = entry.Key;
                var data = entry.Value;

                if (!data.isTilled)
                {
                    RemoveSlider(pos);
                    continue;
                }

                if (data.isWatered)
                {
                    if (!activeSliders.ContainsKey(pos))
                        CreateSlider(pos);
                }
                else
                {
                    RemoveSlider(pos);
                }
            }
        }


        private void CreateSlider(Vector3Int pos)
        {
            var worldPos = farmGrid.CellToWorld(pos) + new Vector3(0.5f, tileOffsetY, 0);
            var instance = Instantiate(waterSliderPrefab, worldPos, Quaternion.identity, transform);
            instance.transform.position = worldPos;  // This stays world-space
            activeSliders[pos] = new WaterSliderInstance(instance);
            Debug.Log($"Creating Water Slider at {worldPos}");

        }

        private void RemoveSlider(Vector3Int pos)
        {
            if (!activeSliders.ContainsKey(pos)) return;

            Destroy(activeSliders[pos].gameObject);
            activeSliders.Remove(pos);
        }
        public void HandleWateredTile(Vector3Int pos, FarmTileData data)
        {
            if (data.isWatered)
            {
                if (!activeSliders.ContainsKey(pos))
                    CreateSlider(pos);

                UpdateSlider(pos, data);
            }
            else
            {
                RemoveSlider(pos);
            }
        }


        private void UpdateSlider(Vector3Int pos, FarmTileData data)
        {
            float progress = data.waterHoursRemaining / FarmTileDataManager.HoursPerWatering;
            activeSliders[pos].SetFill(progress);
        }

        // Simple internal helper class
        private class WaterSliderInstance
        {
            public readonly GameObject gameObject;
            private readonly UnityEngine.UI.Slider slider;

            public WaterSliderInstance(GameObject obj)
            {
                gameObject = obj;
                slider = obj.GetComponentInChildren<UnityEngine.UI.Slider>();
            }

            public void SetFill(float fillAmount)
            {
                slider.value = Mathf.Clamp01(fillAmount);
            }
        }
    }

}
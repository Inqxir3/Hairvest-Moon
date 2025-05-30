using HairvestMoon.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HairvestMoon.Farming
{
    /// <summary>
    /// Handles rendering crop sprites based on growth stages after growth tick occurs.
    /// </summary>
    public class CropVisualSystem : MonoBehaviour
    {
        private Tilemap cropTilemap;

        private void Start()
        {
            cropTilemap = FarmTileDataManager.Instance.CropTilemap;
        }

        private void OnEnable()
        {
            GameTimeManager.Instance.OnTimeChanged += RefreshCropVisuals;
        }

        private void OnDisable()
        {
            GameTimeManager.Instance.OnTimeChanged -= RefreshCropVisuals;
        }

        private void RefreshCropVisuals(int hour, int minute)
        {
            foreach (var entry in FarmTileDataManager.Instance.AllTileData)
            {
                var pos = entry.Key;
                var data = entry.Value;

                if (data.plantedCrop == null)
                {
                    cropTilemap.SetTile(pos, null);
                    continue;
                }

                float growthPercent = data.GetGrowthProgressPercent();

                int stage = Mathf.Clamp(
                    (int)(growthPercent * data.plantedCrop.growthStages.Length),
                    0, data.plantedCrop.growthStages.Length - 1
                );

                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = data.plantedCrop.growthStages[stage];
                cropTilemap.SetTile(pos, tile);
            }
        }
    }
}
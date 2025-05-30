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
            GameTimeManager.Instance.OnDawn += RefreshCropVisuals;
        }

        private void OnDisable()
        {
            GameTimeManager.Instance.OnDawn -= RefreshCropVisuals;
        }

        private void RefreshCropVisuals()
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

                int stage = Mathf.Clamp(data.growthDays, 0, data.plantedCrop.growthStages.Length - 1);
                var sprite = data.plantedCrop.growthStages[stage];

                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = sprite;
                cropTilemap.SetTile(pos, tile);
            }
        }
    }
}
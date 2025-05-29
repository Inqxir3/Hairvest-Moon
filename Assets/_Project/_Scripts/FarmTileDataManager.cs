using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Stores logical gameplay state for each farm tile. Tracks tilling, watering, and crop growth.
/// Interfaces with tilemaps and farming tools to apply gameplay effects.
/// </summary>
public class FarmTileDataManager : MonoBehaviour
{
    public static FarmTileDataManager Instance { get; private set; }

    private Dictionary<Vector3Int, FarmTileData> _tileDataMap = new();

    [Header("References")]
    [SerializeField] private Tilemap tillableTilemap;
    [SerializeField] private Tilemap tilledTilemap;
    [SerializeField] private Tilemap wateredTilemap;
    [SerializeField] private Tile tilledTile;
    [SerializeField] private Tile wateredOverlayTile;

    public bool IsTileTillable(Vector3Int pos)
    {
        return tillableTilemap.GetTile(pos) != null;
    }

    public FarmTileData GetTileData(Vector3Int pos)
    {
        if (!_tileDataMap.TryGetValue(pos, out var data))
        {
            data = new FarmTileData();
            _tileDataMap[pos] = data;
        }
        return data;
    }

    public void SetTilled(Vector3Int pos, bool value)
    {
        var data = GetTileData(pos);
        data.isTilled = value;

        tilledTilemap.SetTile(pos, value ? tilledTile : null);

        // Watered visuals may be affected
        UpdateWaterVisual(pos, data);
    }

    public void SetWatered(Vector3Int pos, bool value)
    {
        var data = GetTileData(pos);
        data.isWatered = value;

        UpdateWaterVisual(pos, data);
    }

    private void UpdateWaterVisual(Vector3Int pos, FarmTileData data)
    {
        if (data.isTilled && data.isWatered)
            wateredTilemap.SetTile(pos, wateredOverlayTile);
        else
            wateredTilemap.SetTile(pos, null);
    }


    public bool IsTilled(Vector3Int pos) => GetTileData(pos).isTilled;
    public bool IsWatered(Vector3Int pos) => GetTileData(pos).isWatered;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}

/// <summary>
/// Struct for storing tile-specific farm state.
/// </summary>
public class FarmTileData
{
    public bool isTilled = false;
    public bool isWatered = false;
    public CropData plantedCrop = null;
    public int growthDays = 0;

    public bool HasRipeCrop()
    {
        return plantedCrop != null && growthDays >= plantedCrop.growthDuration;
    }
}


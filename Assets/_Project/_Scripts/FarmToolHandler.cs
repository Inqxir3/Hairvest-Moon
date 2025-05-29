using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles farm tool interactions (Hoe, Water, Plant, Harvest) using the new input system.
/// </summary>
public class FarmToolHandler : MonoBehaviour
{
    public enum ToolSlot { None, Hoe = 1, Water = 2, Plant = 3, Harvest = 4 }

    [Header("Tool Settings")]
    [SerializeField] private float interactionHoldDuration = 0.1f;
    [SerializeField] private Transform progressSlider;

    [Header("References")]
    [SerializeField] private InputActionReference interactAction;

    private TileTargetingSystem targetingSystem;
    private float currentHoldTime;
    private bool isInteracting;
    private Vector3Int? targetTile;

    private void Awake()
    {
        targetingSystem = TileTargetingSystem.Instance;
    }

    private void OnEnable()
    {
        interactAction.action.performed += OnInteractPerformed;
        interactAction.action.canceled += OnInteractCanceled;
    }

    private void OnDisable()
    {
        interactAction.action.performed -= OnInteractPerformed;
        interactAction.action.canceled -= OnInteractCanceled;
    }

    private void Update()
    {
        if (!isInteracting) return;

        targetTile = targetingSystem.CurrentTargetedTile;
        if (!targetTile.HasValue) return;

        currentHoldTime += Time.deltaTime;
        UpdateSliderVisual();

        if (currentHoldTime >= interactionHoldDuration)
        {
            CompleteInteraction();
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (!GameStateManager.Instance.IsGameplay()) return;

        targetTile = targetingSystem.CurrentTargetedTile;
        if (!targetTile.HasValue)
        {
            DebugUIOverlay.Instance.ShowLastAction("No valid tile");
            return;
        }

        isInteracting = true;
        currentHoldTime = 0f;
        progressSlider.gameObject.SetActive(true);
        PositionSliderAtTarget();
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        if (!isInteracting) return;

        isInteracting = false;
        progressSlider.gameObject.SetActive(false);

        if (currentHoldTime < interactionHoldDuration)
        {
            DebugUIOverlay.Instance.ShowLastAction("Interaction cancelled");
        }
    }

    private void CompleteInteraction()
    {
        isInteracting = false;
        progressSlider.gameObject.SetActive(false);

        if (!targetTile.HasValue) return;

        var tile = targetTile.Value;
        var data = FarmTileDataManager.Instance.GetTileData(tile);

        switch (ToolSystem.CurrentTool)
        {
            case ToolSystem.ToolType.Hoe:
                if (FarmTileDataManager.Instance.IsTileTillable(tile) && !data.isTilled)
                {
                    FarmTileDataManager.Instance.SetTilled(tile, true);
                    DebugUIOverlay.Instance.ShowLastAction("Tilled soil");
                }
                else
                {
                    DebugUIOverlay.Instance.ShowLastAction("Can't till here");
                }
                break;

            case ToolSystem.ToolType.WateringCan:
                if (data.isTilled && !data.isWatered)
                {
                    FarmTileDataManager.Instance.SetWatered(tile, true);
                    DebugUIOverlay.Instance.ShowLastAction("Watered crop");
                }
                else
                {
                    DebugUIOverlay.Instance.ShowLastAction("Nothing to water");
                }
                break;

            case ToolSystem.ToolType.Seed:
                TryPlantSeed(tile, data);
                break;

            case ToolSystem.ToolType.Harvest:
                TryHarvest(tile, data);
                break;

            default:
                DebugUIOverlay.Instance.ShowLastAction("No tool selected");
                break;
        }
    }

    private void TryPlantSeed(Vector3Int tile, FarmTileData data)
    {
        if (data.isTilled && data.plantedCrop == null)
        {
            // Placeholder: use dummy crop data
            data.plantedCrop = new CropData { cropName = "Carrot", growthDuration = 3 };
            data.growthDays = 0;
            DebugUIOverlay.Instance.ShowLastAction("Planted seed");
        }
        else
        {
            DebugUIOverlay.Instance.ShowLastAction("Can't plant here");
        }
    }

    private void TryHarvest(Vector3Int tile, FarmTileData data)
    {
        if (data.HasRipeCrop())
        {
            DebugUIOverlay.Instance.ShowLastAction($"Harvested {data.plantedCrop.cropName}");
            data.plantedCrop = null;
            data.growthDays = 0;
        }
        else
        {
            DebugUIOverlay.Instance.ShowLastAction("Nothing to harvest");
        }
    }

    private void PositionSliderAtTarget()
    {
        if (!targetTile.HasValue) return;
        Vector3 worldPos = targetingSystem.Grid.CellToWorld(targetTile.Value);
        progressSlider.position = worldPos + Vector3.up;
    }

    private void UpdateSliderVisual()
    {
        float progress = Mathf.Clamp01(currentHoldTime / interactionHoldDuration);
        progressSlider.localScale = new Vector3(progress, 1f, 1f);
    }
}


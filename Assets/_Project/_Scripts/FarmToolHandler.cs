using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles farm tool interactions (Hoe, Water, Plant, Harvest) using the new input system
/// </summary>
public class FarmToolHandler : MonoBehaviour
{
    public enum ToolSlot { None, Hoe = 1, Water = 2, Plant = 3, Harvest = 4 }

    [Header("Tool Settings")]
    [SerializeField] private float interactionHoldDuration = 0.5f;
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
        if (!targetTile.HasValue) return;

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

        switch (ToolSystem.CurrentTool)
        {
            case ToolSystem.ToolType.Hoe:
                FarmTileDataManager.Instance.SetTilled(targetTile.Value, true);
                break;

            case ToolSystem.ToolType.WateringCan:
                FarmTileDataManager.Instance.SetWatered(targetTile.Value, true);
                break;

            case ToolSystem.ToolType.Seed:
                TryPlantSeed(targetTile.Value);
                break;

            case ToolSystem.ToolType.Harvest:
                TryHarvest(targetTile.Value);
                break;
        }
    }

    private void TryPlantSeed(Vector3Int tilePos)
    {
        // Your planting logic here
    }

    private void TryHarvest(Vector3Int tilePos)
    {
        // Your harvest logic here
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

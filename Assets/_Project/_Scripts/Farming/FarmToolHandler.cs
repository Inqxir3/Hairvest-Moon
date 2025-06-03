using HairvestMoon.Core;
using HairvestMoon.Interaction;
using HairvestMoon.Inventory;
using HairvestMoon.Tool;
using HairvestMoon.UI;
using HairvestMoon.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HairvestMoon.Farming
{
    /// <summary>
    /// Handles farm tool interactions (Hoe, Water, Plant, Harvest) using the new input system.
    /// </summary>
    public class FarmToolHandler : MonoBehaviour
    {
        public enum ToolSlot { None, Hoe = 1, Water = 2, Plant = 3, Harvest = 4 }

        [Header("Tool Settings")]
        [SerializeField] private float interactionHoldDuration = 0.1f;
        [SerializeField] private Transform progressSlider;
        [SerializeField] private SeedData selectedSeed;

        [Header("References")]
        [SerializeField] private InputActionReference interactAction;

        private TileTargetingSystem targetingSystem;
        private float currentHoldTime;
        private bool isInteracting;
        private Vector3Int? targetTile;

        public void Initialize()
        {
            targetingSystem = TileTargetingSystem.Instance;
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
            if (!GameStateManager.Instance.IsFreeRoam()) return;

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

            switch (ToolSystem.Instance.CurrentTool)
            {
                case ToolType.Hoe:
                    TryTill(tile, data);
                    break;

                case ToolType.WateringCan:
                    TryWater(tile, data);
                    break;

                case ToolType.Seed:
                    TryPlantSeed(tile, data);
                    break;

                case ToolType.Harvest:
                    TryHarvest(tile, data);
                    break;

                default:
                    DebugUIOverlay.Instance.ShowLastAction("No tool selected");
                    break;
            }
        }

        private void TryTill(Vector3Int tile, FarmTileData data)
        {
            FarmTileDataManager.Instance.SetTilled(tile, true);
            DebugUIOverlay.Instance.ShowLastAction("Tile tilled");

            // Apply Upgrade Behavior if selected
            var hoeUpgrade = BackpackEquipSystem.Instance.hoeUpgrade;
            var selectedOption = HoeSelectionUI.Instance.GetCurrentSelectedItem();

            if (selectedOption != null && hoeUpgrade != null)
            {
                DebugUIOverlay.Instance.ShowLastAction($"Hoe Upgrade Used: {selectedOption.itemName}");

                // Apply bonus tilling logic here:
                ApplyExtraTilling(tile);
            }
        }

        private void TryWater(Vector3Int tile, FarmTileData data)
        {
            ToolSystem.Instance.ConsumeWaterFromCan();

            if (!data.isTilled)
            {
                DebugUIOverlay.Instance.ShowLastAction("Water wasted — not tilled");
                return;
            }

            // Always apply water normally
            FarmTileDataManager.Instance.SetWatered(tile, true);
            DebugUIOverlay.Instance.ShowLastAction("Water applied");

            // Check if Fertilizer Sprayer is equipped
            var wateringUpgrade = BackpackEquipSystem.Instance.wateringUpgrade;

            if (wateringUpgrade != null)
            {
                // Read selected fertilizer from WateringSelectionUI
                ItemData selectedFertilizer = WateringSelectionUI.Instance.GetCurrentSelectedItem();

                if (selectedFertilizer != null)
                {
                    bool removed = BackpackInventorySystem.Instance.RemoveItem(selectedFertilizer, 1);
                    if (removed)
                    {
                        DebugUIOverlay.Instance.ShowLastAction($"Fertilizer applied: {selectedFertilizer.itemName}");
                        // You can expand here to actually apply fertilizer effects to tile data later.
                    }
                    else
                    {
                        DebugUIOverlay.Instance.ShowLastAction("No fertilizer available.");
                    }
                }
            }
        }


        private void TryPlantSeed(Vector3Int tile, FarmTileData data)
        {
            if (data.isTilled && data.plantedCrop == null && selectedSeed != null)
            {
                int seedCount = InventorySystem.Instance.GetQuantity(selectedSeed.seedItem);
                if (seedCount <= 0)
                {
                    DebugUIOverlay.Instance.ShowLastAction("No seeds available");
                    return;
                }

                bool removed = InventorySystem.Instance.RemoveItem(selectedSeed.seedItem, 1);
                if (!removed)
                {
                    DebugUIOverlay.Instance.ShowLastAction("Failed to consume seed");
                    return;
                }

                data.plantedCrop = selectedSeed.cropData;
                data.wateredMinutesAccumulated = 0f;
                DebugUIOverlay.Instance.ShowLastAction($"Planted {selectedSeed.cropData.cropName}");
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
                var harvestedItem = data.plantedCrop.harvestedItem;
                var yield = data.plantedCrop.harvestYield;

                bool added = InventorySystem.Instance.AddItem(harvestedItem, yield);

                if (added)
                {
                    DebugUIOverlay.Instance.ShowLastAction($"Harvested {yield}x {harvestedItem.itemName}");
                    data.plantedCrop = null;
                    data.wateredMinutesAccumulated = 0f;
                }
                else
                {
                    DebugUIOverlay.Instance.ShowLastAction("Inventory Full - Harvest Failed");
                }
            }
            else
            {
                DebugUIOverlay.Instance.ShowLastAction("Nothing to harvest");
            }
        }

        private void ApplyExtraTilling(Vector3Int centerTile)
        {
            // Simple 3x3 till radius as an example:
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    Vector3Int nearbyTile = new Vector3Int(centerTile.x + dx, centerTile.y + dy, 0);
                    var tileData = FarmTileDataManager.Instance.GetTileData(nearbyTile);
                    if (!tileData.isTilled)
                    {
                        FarmTileDataManager.Instance.SetTilled(nearbyTile, true);
                    }
                }
            }
        }

        public void SetSelectedSeed(SeedData newSeed)
        {
            selectedSeed = newSeed;
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
}
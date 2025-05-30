using HairvestMoon.Core;
using HairvestMoon.Interaction;
using HairvestMoon.Tool;
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

        private void Start()
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
                case ToolSystem.ToolType.Hoe:
                    TryTill(tile, data);
                    break;

                case ToolSystem.ToolType.WateringCan:
                    TryWater(tile, data);
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
        private void TryTill(Vector3Int tile, FarmTileData data)
        {
            if (FarmTileDataManager.Instance.IsTileTillable(tile) && !data.isTilled)
            {
                FarmTileDataManager.Instance.SetTilled(tile, true);
                DebugUIOverlay.Instance.ShowLastAction("Tilled soil");
            }
            else
            {
                DebugUIOverlay.Instance.ShowLastAction("Can't till here");
            }
        }

        private void TryWater(Vector3Int tile, FarmTileData data)
        {
            ToolSystem.Instance.ConsumeWaterFromCan();  // (Prepped for future water usage system)

            if (!data.isTilled)
            {
                DebugUIOverlay.Instance.ShowLastAction("Water wasted — not tilled");
                return;
            }

            FarmTileDataManager.Instance.SetWatered(tile, true);
            DebugUIOverlay.Instance.ShowLastAction("Water applied");
        }


        private void TryPlantSeed(Vector3Int tile, FarmTileData data)
        {
            if (data.isTilled && data.plantedCrop == null)
            {
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
                DebugUIOverlay.Instance.ShowLastAction($"Harvested {data.plantedCrop.cropName}");
                data.plantedCrop = null;
                data.wateredMinutesAccumulated = 0f;
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

}
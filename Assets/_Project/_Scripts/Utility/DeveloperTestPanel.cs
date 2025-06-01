using UnityEngine;
using UnityEngine.UI;
using HairvestMoon.Inventory;
using HairvestMoon.Farming;
using HairvestMoon.Tool;

namespace HairvestMoon.Utility
{
    public class DeveloperTestPanel : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button addSeedsButton;
        [SerializeField] private Button addCropsButton;
        [SerializeField] private Button clearInventoryButton;
        [SerializeField] private Button refillWaterButton;
        [SerializeField] private Button addBackpackItemButton;
        [SerializeField] private Button upgradeBackpackButton;

        [Header("Test Items")]
        [SerializeField] private ItemData testBackpackItem;

        private void Start()
        {
            addSeedsButton.onClick.AddListener(AddTestSeeds);
            addCropsButton.onClick.AddListener(AddTestCrops);
            clearInventoryButton.onClick.AddListener(ClearInventory);
            refillWaterButton.onClick.AddListener(RefillWater);
            addBackpackItemButton.onClick.AddListener(AddBackpackItem);
            upgradeBackpackButton.onClick.AddListener(UpgradeBackpack);
        }

        private void AddTestSeeds()
        {
            foreach (var seedData in SeedDatabase.Instance.AllSeeds)
            {
                InventorySystem.Instance.AddItem(seedData.seedItem, 5);
            }
        }

        private void AddTestCrops()
        {
            foreach (var seedData in SeedDatabase.Instance.AllSeeds)
            {
                CropData cropData = seedData.cropData;
                InventorySystem.Instance.AddItem(cropData.harvestedItem, 5);
            }
        }

        private void ClearInventory()
        {
            InventorySystem.Instance.inventory.Clear();
            InventorySystem.Instance.discoveredItems.Clear();
            BackpackInventorySystem.Instance.backpack.Clear();

            InventorySystem.Instance.ForceRefresh();
            BackpackInventorySystem.Instance.ForceRefresh();
        }

        private void RefillWater()
        {
            ToolSystem.Instance.RefillWaterToFull();
        }

        private void AddBackpackItem()
        {
            if (testBackpackItem != null)
            {
                BackpackInventorySystem.Instance.AddItem(testBackpackItem, 1);
            }
            else
            {
                Debug.LogWarning("Assign a testBackpackItem in the inspector.");
            }
        }

        private void UpgradeBackpack()
        {
            BackpackUpgradeManager.Instance.UpgradeBackpack();
        }
    }
}

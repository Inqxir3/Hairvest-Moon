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

        private void Start()
        {
            addSeedsButton.onClick.AddListener(AddTestSeeds);
            addCropsButton.onClick.AddListener(AddTestCrops);
            clearInventoryButton.onClick.AddListener(ClearInventory);
            refillWaterButton.onClick.AddListener(RefillWater);
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
                //InventorySystem.Instance.AddItem(cropData.harvestItem, 5);
            }
        }

        private void ClearInventory()
        {
            InventorySystem.Instance.inventory.Clear();
            InventorySystem.Instance.discoveredItems.Clear();
            //invoke necessary inventory and ui change logic
        }

        private void RefillWater()
        {
            ToolSystem.Instance.RefillWaterToFull();
        }
    }
}

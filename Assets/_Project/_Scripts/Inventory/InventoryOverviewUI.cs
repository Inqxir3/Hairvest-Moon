using System.Collections.Generic;
using UnityEngine;
using HairvestMoon.Farming;

namespace HairvestMoon.Inventory
{
    public class InventoryOverviewUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform cropGridParent;
        [SerializeField] private Transform seedGridParent;
        [SerializeField] private GameObject inventorySlotPrefab;

        private readonly Dictionary<ItemData, InventorySlotUI> cropSlots = new();
        private readonly Dictionary<ItemData, InventorySlotUI> seedSlots = new();

       
        private void OnDisable()
        {
            InventorySystem.Instance.OnInventoryChanged -= RefreshUI;
        }

        private void Start()
        {
            InventorySystem.Instance.OnInventoryChanged += RefreshUI;

            BuildUI();
            RefreshUI();
        }

        private void BuildUI()
        {
            // Build Seeds
            foreach (var seedData in SeedDatabase.Instance.AllSeeds)
            {
                var item = seedData.seedItem;
                var slotGO = Instantiate(inventorySlotPrefab, seedGridParent);
                var slot = slotGO.GetComponent<InventorySlotUI>();
                slot.Initialize(item);
                seedSlots[item] = slot;
            }

            // Build Crops
            foreach (var seedData in SeedDatabase.Instance.AllSeeds)
            {
                if (seedData?.cropData?.harvestedItem == null) continue; // safety null check

                var cropData = seedData.cropData;
                var item = cropData.GetHarvestItem();

                if (cropSlots.ContainsKey(item)) continue;

                var slotGO = Instantiate(inventorySlotPrefab, cropGridParent);
                var slot = slotGO.GetComponent<InventorySlotUI>();
                slot.Initialize(item);
                cropSlots[item] = slot;
            }
        }

        public void RefreshUI()
        {
            foreach (var pair in cropSlots)
                pair.Value.UpdateDisplay();

            foreach (var pair in seedSlots)
                pair.Value.UpdateDisplay();
        }
    }
}

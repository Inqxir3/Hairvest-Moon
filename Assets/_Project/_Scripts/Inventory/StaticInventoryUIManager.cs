using System.Collections.Generic;
using UnityEngine;

namespace HairvestMoon.Inventory
{
    public class StaticInventoryUIManager : MonoBehaviour
    {
        [Header("Data References")]
        [SerializeField] private CropInventoryData inventoryData;

        [Header("UI References")]
        [SerializeField] private Transform cropGridParent;
        [SerializeField] private Transform seedGridParent;
        [SerializeField] private GameObject inventorySlotPrefab;

        private Dictionary<ItemData, StaticInventorySlot> cropSlots = new();
        private Dictionary<ItemData, StaticInventorySlot> seedSlots = new();

        private void Start()
        {
            BuildStaticUI();
            RefreshUI();
        }

        private void BuildStaticUI()
        {
            // Build crop grid
            foreach (var item in inventoryData.allCropItems)
            {
                var slotObj = Instantiate(inventorySlotPrefab, cropGridParent);
                var slot = slotObj.GetComponent<StaticInventorySlot>();
                slot.Initialize(item);
                cropSlots.Add(item, slot);
            }

            // Build seed grid
            foreach (var item in inventoryData.allSeedItems)
            {
                var slotObj = Instantiate(inventorySlotPrefab, seedGridParent);
                var slot = slotObj.GetComponent<StaticInventorySlot>();
                slot.Initialize(item);
                seedSlots.Add(item, slot);
            }
        }

        public void RefreshUI()
        {
            foreach (var pair in cropSlots)
                pair.Value.UpdateDisplay();

            foreach (var pair in seedSlots)
                pair.Value.UpdateDisplay();
        }

        private void Update()
        {
            // Keep refreshing for live test (optional: optimize later)
            RefreshUI();
        }
    }
}
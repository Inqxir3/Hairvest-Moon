using HairvestMoon.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace HairvestMoon.Farming
{
    public class SeedSelectionUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject seedSlotPrefab;
        [SerializeField] private Transform seedGridParent;
        [SerializeField] private FarmToolHandler farmToolHandler;

        private List<SeedSelectionSlot> slots = new();
        private ItemData currentSelectedItem;

        public static SeedSelectionUI Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnEnable()
        {
            InventorySystem.Instance.OnInventoryChanged += RefreshUI;
            BuildUI();
        }

        private void OnDisable()
        {
            InventorySystem.Instance.OnInventoryChanged -= RefreshUI;
        }

        private void BuildUI()
        {
            // Clear old
            foreach (Transform child in seedGridParent)
                Destroy(child.gameObject);
            slots.Clear();

            var discoveredSeeds = InventorySystem.Instance.GetDiscoveredItemsByType(ItemType.Seed);

            // Handle empty case
            if (discoveredSeeds.Count == 0)
            {
                currentSelectedItem = null;
                farmToolHandler.SetSelectedSeed(null);
                return;
            }

            // If we have no valid selection, auto-select first seed
            if (currentSelectedItem == null || !discoveredSeeds.Contains(currentSelectedItem))
            {
                currentSelectedItem = discoveredSeeds[0];
                SetSelectedSeed(currentSelectedItem);
            }

            foreach (var itemData in discoveredSeeds)
            {
                var slotGO = Instantiate(seedSlotPrefab, seedGridParent);
                var slot = slotGO.GetComponent<SeedSelectionSlot>();
                slot.Initialize(itemData, OnSeedSelected);
                slot.SetSelected(itemData == currentSelectedItem);
                slots.Add(slot);
            }
        }

        private void OnSeedSelected(ItemData selectedItem)
        {
            currentSelectedItem = selectedItem;
            SetSelectedSeed(currentSelectedItem);

            // Update highlights
            foreach (var slot in slots)
            {
                slot.SetSelected(slot.GetItemData() == currentSelectedItem);
            }
        }

        private void SetSelectedSeed(ItemData selectedItem)
        {
            SeedData seedData = SeedDatabase.Instance.GetSeedDataByItem(selectedItem);
            farmToolHandler.SetSelectedSeed(seedData);
        }

        private void RefreshUI()
        {
            Debug.Log("Refreshing Seed Selection UI");
            BuildUI();
        }

        public void OpenSeedMenu()
        {
            gameObject.SetActive(true);
            BuildUI();
        }

        public void CloseSeedMenu()
        {
            gameObject.SetActive(false);
        }
    }
}

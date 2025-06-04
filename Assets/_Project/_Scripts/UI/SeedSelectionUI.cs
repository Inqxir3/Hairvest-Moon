using HairvestMoon.Core;
using HairvestMoon.Farming;
using HairvestMoon.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace HairvestMoon.UI
{
    public class SeedSelectionUI : MonoBehaviour, IBusListener
    {
        [Header("UI References")]
        [SerializeField] private GameObject seedSlotPrefab;
        [SerializeField] private Transform seedGridParent;
        [SerializeField] private FarmToolHandler farmToolHandler;

        private List<UpgradeSelectionSlot> slots = new();
        private ItemData currentSelectedItem;

        public void InitializeUI()
        {
            BuildUI();
        }
        public void RegisterBusListeners()
        {
            var bus = ServiceLocator.Get<GameEventBus>();
            bus.InventoryChanged += RefreshUI;
        }


        private void OnDisable()
        {
            var bus = ServiceLocator.Get<GameEventBus>();
            bus.InventoryChanged -= RefreshUI;
        }

        private void BuildUI()
        {
            foreach (Transform child in seedGridParent)
                Destroy(child.gameObject);

            slots.Clear();

            // Build list of seeds the player has in stock
            List<ItemData> seedsInInventory = new();

            foreach (var seedData in ServiceLocator.Get<SeedDatabase>().AllSeeds)
            {
                int quantity = ServiceLocator.Get<InventorySystem>().GetQuantity(seedData.seedItem);
                if (quantity > 0)
                {
                    seedsInInventory.Add(seedData.seedItem);
                }
            }

            if (seedsInInventory.Count == 0)
            {
                currentSelectedItem = null;
                farmToolHandler.SetSelectedSeed(null);
                return;
            }

            // Build UI for each owned seed
            foreach (var item in seedsInInventory)
            {
                var slotGO = Instantiate(seedSlotPrefab, seedGridParent);
                var slotUI = slotGO.GetComponent<UpgradeSelectionSlot>();
                slotUI.Initialize(item, OnSeedSelected);
                slotUI.SetSelected(item == currentSelectedItem);
                slots.Add(slotUI);
            }

            // If no selection is active, auto-select first seed
            if (currentSelectedItem == null)
            {
                OnSeedSelected(seedsInInventory[0]);
            }
        }


        private void OnSeedSelected(ItemData selectedItem)
        {
            currentSelectedItem = selectedItem;
            SetSelectedSeed(currentSelectedItem);

            // Update highlights
            foreach (var slot in slots)
            {
                slot.SetSelected(slot.Item == currentSelectedItem);
            }
        }

        private void SetSelectedSeed(ItemData selectedItem)
        {
            SeedData seedData = ServiceLocator.Get<SeedDatabase>().GetSeedDataByItem(selectedItem);
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
        }

        public void CloseSeedMenu()
        {
            gameObject.SetActive(false);
        }
    }
}

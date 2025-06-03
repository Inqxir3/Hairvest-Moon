using System.Collections.Generic;
using UnityEngine;
using HairvestMoon.Inventory;
using HairvestMoon.Farming;
using HairvestMoon.UI;

namespace HairvestMoon.UI
{
    public class HarvestSelectionUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject harvestSelectionSlotPrefab;
        [SerializeField] private Transform gridParent;

        private List<UpgradeSelectionSlot> slots = new();
        private ItemData currentSelectedHarvestOption;

        public static HarvestSelectionUI Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void InitializeUI()
        {
            BuildUI();
            BackpackInventorySystem.Instance.OnBackpackChanged += RefreshUI;
        }

        private void OnDisable()
        {
            BackpackInventorySystem.Instance.OnBackpackChanged -= RefreshUI;
        }

        public void OpenHarvestMenu()
        {
            gameObject.SetActive(true);
            BuildUI();
        }

        public void CloseHarvestMenu()
        {
            gameObject.SetActive(false);
        }

        private void BuildUI()
        {
            foreach (Transform child in gridParent)
                Destroy(child.gameObject);
            slots.Clear();

            // Always add Normal Harvest option
            var slotGO = Instantiate(harvestSelectionSlotPrefab, gridParent);
            var slotUI = slotGO.GetComponent<UpgradeSelectionSlot>();
            slotUI.Initialize(null, OnHarvestOptionSelected);
            slotUI.SetSelected(currentSelectedHarvestOption == null);
            slots.Add(slotUI);

            // If we have Harvest Upgrade equipped, enable selection
            var harvestUpgrade = BackpackEquipSystem.Instance.harvestUpgrade;
            if (harvestUpgrade != null)
            {
                var upgradeGO = Instantiate(harvestSelectionSlotPrefab, gridParent);
                var upgradeSlotUI = upgradeGO.GetComponent<UpgradeSelectionSlot>();
                upgradeSlotUI.Initialize(harvestUpgrade, OnHarvestOptionSelected);
                upgradeSlotUI.SetSelected(harvestUpgrade == currentSelectedHarvestOption);
                slots.Add(upgradeSlotUI);
            }
        }

        private void RefreshUI()
        {
            BuildUI();
        }

        private void OnHarvestOptionSelected(ItemData selectedItem)
        {
            currentSelectedHarvestOption = selectedItem;

            foreach (var slot in slots)
                slot.SetSelected(slot.Item == currentSelectedHarvestOption);
        }

        public ItemData GetCurrentSelectedItem()
        {
            return currentSelectedHarvestOption;
        }
    }
}

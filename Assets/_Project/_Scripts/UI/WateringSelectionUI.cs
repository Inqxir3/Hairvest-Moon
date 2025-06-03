using System.Collections.Generic;
using UnityEngine;
using HairvestMoon.Inventory;
using HairvestMoon.Farming;

namespace HairvestMoon.UI
{
    public class WateringSelectionUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject waterSelectionSlotPrefab;
        [SerializeField] private Transform gridParent;
        [SerializeField] private FarmToolHandler farmToolHandler;

        private List<UpgradeSelectionSlot> slots = new();
        private ItemData currentSelectedWatering;

        public static WateringSelectionUI Instance { get; private set; }

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

        public void OpenWateringMenu()
        {
            gameObject.SetActive(true);
            BuildUI();
        }

        public void CloseWateringMenu()
        {
            gameObject.SetActive(false);
        }

        private void BuildUI()
        {
            foreach (Transform child in gridParent)
                Destroy(child.gameObject);
            slots.Clear();

            // Always add Normal Water option first
            var slotGO = Instantiate(waterSelectionSlotPrefab, gridParent);
            var slotUI = slotGO.GetComponent<UpgradeSelectionSlot>();
            slotUI.Initialize(null, OnWateringOptionSelected);
            slotUI.SetSelected(currentSelectedWatering == null);
            slots.Add(slotUI);

            // If we have a Fertilizer Sprayer equipped, enable fertilizer selection
            var wateringUpgrade = BackpackEquipSystem.Instance.wateringUpgrade;
            if (wateringUpgrade != null)
            {
                var allBackpackSlots = BackpackInventorySystem.Instance.GetAllSlots();

                foreach (var slot in allBackpackSlots)
                {
                    if (slot.item.itemType == ItemType.Fertilizer)
                    {
                        var fertSlotGO = Instantiate(waterSelectionSlotPrefab, gridParent);
                        var fertSlotUI = fertSlotGO.GetComponent<UpgradeSelectionSlot>();
                        fertSlotUI.Initialize(slot.item, OnWateringOptionSelected);
                        fertSlotUI.SetSelected(slot.item == currentSelectedWatering);
                        slots.Add(fertSlotUI);
                    }
                }
            }
        }

        private void RefreshUI()
        {
            BuildUI();
        }

        private void OnWateringOptionSelected(ItemData selectedItem)
        {
            currentSelectedWatering = selectedItem;
            //farmToolHandler.SetSelectedWateringOption(selectedItem);  // We'll wire this into actual logic later

            foreach (var slot in slots)
            {
                slot.SetSelected(slot.Item == currentSelectedWatering);
            }
        }

        public ItemData GetCurrentSelectedItem()
        {
            return currentSelectedWatering;
        }
    }
}

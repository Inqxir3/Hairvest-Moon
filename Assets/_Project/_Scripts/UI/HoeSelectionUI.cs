using UnityEngine;
using HairvestMoon.Inventory;
using System.Collections.Generic;

namespace HairvestMoon.UI
{
    public class HoeSelectionUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject hoeSelectionSlotPrefab;
        [SerializeField] private Transform gridParent;

        private List<UpgradeSelectionSlot> slots = new();
        private ItemData currentSelectedHoeOption;

        public static HoeSelectionUI Instance { get; private set; }

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

        public void OpenHoeMenu()
        {
            gameObject.SetActive(true);
            BuildUI();
        }

        public void CloseHoeMenu()
        {
            gameObject.SetActive(false);
        }

        private void BuildUI()
        {
            foreach (Transform child in gridParent)
                Destroy(child.gameObject);
            slots.Clear();

            // Always add Normal Hoe option
            var slotGO = Instantiate(hoeSelectionSlotPrefab, gridParent);
            var slotUI = slotGO.GetComponent<UpgradeSelectionSlot>();
            slotUI.Initialize(null, OnHoeOptionSelected);
            slotUI.SetSelected(currentSelectedHoeOption == null);
            slots.Add(slotUI);

            // If we have Hoe Upgrade equipped, enable selection
            var hoeUpgrade = BackpackEquipSystem.Instance.hoeUpgrade;
            if (hoeUpgrade != null)
            {
                var upgradeGO = Instantiate(hoeSelectionSlotPrefab, gridParent);
                var upgradeSlotUI = upgradeGO.GetComponent<UpgradeSelectionSlot>();
                upgradeSlotUI.Initialize(hoeUpgrade, OnHoeOptionSelected);
                upgradeSlotUI.SetSelected(hoeUpgrade == currentSelectedHoeOption);
                slots.Add(upgradeSlotUI);
            }
        }

        private void RefreshUI()
        {
            BuildUI();
        }

        private void OnHoeOptionSelected(ItemData selectedItem)
        {
            currentSelectedHoeOption = selectedItem;

            foreach (var slot in slots)
                slot.SetSelected(slot.Item == currentSelectedHoeOption);
        }

        public ItemData GetCurrentSelectedItem()
        {
            return currentSelectedHoeOption;
        }
    }
}

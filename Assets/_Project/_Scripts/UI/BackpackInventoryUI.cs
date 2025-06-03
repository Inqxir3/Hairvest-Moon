using System.Collections.Generic;
using UnityEngine;
using HairvestMoon.Inventory;

namespace HairvestMoon.UI
{
    public class BackpackInventoryUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform backpackGridParent;
        [SerializeField] private GameObject backpackSlotPrefab;
        [SerializeField] private GameObject emptyGridPrefab;
        [SerializeField] private GameObject lockedGridPrefab;
        [SerializeField] private ItemDescriptionUI itemDescriptionUI;
        [SerializeField] private BackpackCapacityBarUI capacityBarUI;

        [Header("Tool Slots")]
        [SerializeField] private EquipSlotUI hoeToolSlot;
        [SerializeField] private EquipSlotUI wateringToolSlot;
        [SerializeField] private EquipSlotUI seedToolSlot;
        [SerializeField] private EquipSlotUI harvestToolSlot;

        [Header("Upgrade Slots")]
        [SerializeField] private EquipSlotUI hoeUpgradeSlot;
        [SerializeField] private EquipSlotUI wateringUpgradeSlot;
        [SerializeField] private EquipSlotUI seedUpgradeSlot;
        [SerializeField] private EquipSlotUI harvestUpgradeSlot;

        private readonly Dictionary<ItemData, BackpackSlotUI> slots = new();
        private ItemData currentSelectedItem;


        public void InitializeUI()
        {
            BackpackInventorySystem.Instance.OnBackpackChanged += RefreshUI;
            BuildUI();
        }

        private void OnDisable()
        {
            BackpackInventorySystem.Instance.OnBackpackChanged -= RefreshUI;
        }

        private void BuildUI()
        {
            foreach (Transform child in backpackGridParent)
                Destroy(child.gameObject);

            slots.Clear();

            int totalSlots = BackpackUpgradeManager.Instance.GetMaxUpgrades() * BackpackUpgradeManager.Instance.SlotsPerUpgrade + BackpackUpgradeManager.Instance.BaseSlots;
            int unlockedSlots = BackpackUpgradeManager.Instance.GetCurrentSlots();

            var allBackpackSlots = BackpackInventorySystem.Instance.GetAllSlots();
            int filledSlots = allBackpackSlots.Count;

            int filledIndex = 0;

            for (int i = 0; i < totalSlots; i++)
            {
                if (i < unlockedSlots)
                {
                    if (filledIndex < filledSlots)
                    {
                        var slotGO = Instantiate(backpackSlotPrefab, backpackGridParent);
                        var slot = slotGO.GetComponent<BackpackSlotUI>();
                        var backpackData = allBackpackSlots[filledIndex];
                        slot.Initialize(backpackData.item, backpackData.quantity, OnSlotSelected);
                        slots[backpackData.item] = slot;
                        filledIndex++;
                    }
                    else
                    {
                        Instantiate(emptyGridPrefab, backpackGridParent);
                    }
                }
                else
                {
                    Instantiate(lockedGridPrefab, backpackGridParent);
                }
            }
        }

        public void RefreshUI()
        {
            BuildUI();
            UpdateSelection(currentSelectedItem);
        }

        private void OnSlotSelected(ItemData selectedItem)
        {
            currentSelectedItem = selectedItem;
            UpdateSelection(selectedItem);

            if (BackpackEquipInstallManager.Instance.TryEquip(selectedItem))
            {
                Debug.Log("Installed " + selectedItem.itemName);
                RefreshEquipSlots();
            }
        }

        private void RefreshEquipSlots()
        {
            hoeToolSlot.SetSlot(BackpackEquipSystem.Instance.hoeTool);
            wateringToolSlot.SetSlot(BackpackEquipSystem.Instance.wateringTool);
            seedToolSlot.SetSlot(BackpackEquipSystem.Instance.seedTool);
            harvestToolSlot.SetSlot(BackpackEquipSystem.Instance.harvestTool);

            hoeUpgradeSlot.SetSlot(BackpackEquipSystem.Instance.hoeUpgrade);
            wateringUpgradeSlot.SetSlot(BackpackEquipSystem.Instance.wateringUpgrade);
            seedUpgradeSlot.SetSlot(BackpackEquipSystem.Instance.seedUpgrade);
            harvestUpgradeSlot.SetSlot(BackpackEquipSystem.Instance.harvestUpgrade);
        }


        private void UpdateSelection(ItemData selectedItem)
        {
            foreach (var pair in slots)
                pair.Value.SetSelected(pair.Key == selectedItem);

            if (selectedItem != null)
                itemDescriptionUI.SetItem(selectedItem);
            else
                itemDescriptionUI.Clear();
        }
    }
}

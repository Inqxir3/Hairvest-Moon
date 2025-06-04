using System.Collections.Generic;
using UnityEngine;
using HairvestMoon.Inventory;
using HairvestMoon.Core;

namespace HairvestMoon.UI
{
    public class BackpackInventoryUI : MonoBehaviour, IBusListener
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
            BuildUI();
        }

        public void RegisterBusListeners()
        {
            var bus = ServiceLocator.Get<GameEventBus>();
            bus.BackpackChanged += RefreshUI;
        }

        private void OnDisable()
        {
            var bus = ServiceLocator.Get<GameEventBus>();
            bus.BackpackChanged -= RefreshUI;
        }

        private void BuildUI()
        {
            foreach (Transform child in backpackGridParent)
                Destroy(child.gameObject);

            slots.Clear();

            int totalSlots = ServiceLocator.Get<BackpackUpgradeManager>().GetMaxUpgrades() * ServiceLocator.Get<BackpackUpgradeManager>().SlotsPerUpgrade + ServiceLocator.Get<BackpackUpgradeManager>().BaseSlots;
            int unlockedSlots = ServiceLocator.Get<BackpackUpgradeManager>().GetCurrentSlots();

            var allBackpackSlots = ServiceLocator.Get<BackpackInventorySystem>().GetAllSlots();
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

            if (ServiceLocator.Get<BackpackEquipInstallManager>().TryEquip(selectedItem))
            {
                Debug.Log("Installed " + selectedItem.itemName);
                RefreshEquipSlots();
            }
        }

        private void RefreshEquipSlots()
        {
            hoeToolSlot.SetSlot(ServiceLocator.Get<BackpackEquipSystem>().hoeTool);
            wateringToolSlot.SetSlot(ServiceLocator.Get<BackpackEquipSystem>().wateringTool);
            seedToolSlot.SetSlot(ServiceLocator.Get<BackpackEquipSystem>().seedTool);
            harvestToolSlot.SetSlot(ServiceLocator.Get<BackpackEquipSystem>().harvestTool);

            hoeUpgradeSlot.SetSlot(ServiceLocator.Get<BackpackEquipSystem>().hoeUpgrade);
            wateringUpgradeSlot.SetSlot(ServiceLocator.Get<BackpackEquipSystem>().wateringUpgrade);
            seedUpgradeSlot.SetSlot(ServiceLocator.Get<BackpackEquipSystem>().seedUpgrade);
            harvestUpgradeSlot.SetSlot(ServiceLocator.Get<BackpackEquipSystem>().harvestUpgrade);
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

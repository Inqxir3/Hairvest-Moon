using HairvestMoon.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace HairvestMoon.UI
{
    public class BackpackInventoryUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform backpackGridParent;
        [SerializeField] private GameObject backpackSlotPrefab;

        private readonly Dictionary<ItemData, BackpackSlotUI> slots = new();

        private void OnEnable()
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

            foreach (var slot in BackpackInventorySystem.Instance.GetAllSlots())
            {
                var slotGO = Instantiate(backpackSlotPrefab, backpackGridParent);
                var uiSlot = slotGO.GetComponent<BackpackSlotUI>();
                uiSlot.Initialize(slot.item, slot.quantity);
                slots[slot.item] = uiSlot;
            }
        }

        public void RefreshUI()
        {
            BuildUI();
        }
    }
}

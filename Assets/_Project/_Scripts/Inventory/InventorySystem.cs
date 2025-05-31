using System.Collections.Generic;
using UnityEngine;

namespace HairvestMoon.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        public static InventorySystem Instance { get; private set; }

        [System.Serializable]
        public class InventorySlot
        {
            public ItemData item;
            public int quantity;
        }

        [Header("Inventory Settings")]
        public int maxSlots = 20;
        public List<InventorySlot> inventory = new();

        public HashSet<ItemData> discoveredItems = new HashSet<ItemData>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void MarkDiscovered(ItemData item)
        {
            if (!discoveredItems.Contains(item))
            {
                discoveredItems.Add(item);
                Debug.Log($"Discovered new item: {item.itemName}");
            }
        }

        public bool AddItem(ItemData newItem, int amount = 1)
        {
            // Try stacking first
            foreach (var slot in inventory)
            {
                if (slot.item == newItem)
                {
                    slot.quantity += amount;
                    return true;
                }
            }

            // Create new slot if room
            if (inventory.Count < maxSlots)
            {
                var newSlot = new InventorySlot { item = newItem, quantity = amount };
                inventory.Add(newSlot);
                MarkDiscovered(newItem);
                return true;
            }

            Debug.Log("Inventory Full");
            return false;
        }

        public void DebugPrintInventory()
        {
            Debug.Log("Inventory Contents:");
            foreach (var slot in inventory)
            {
                Debug.Log($"{slot.quantity}x {slot.item.itemName}");
            }
        }

        public int GetQuantity(ItemData queryItem)
        {
            foreach (var slot in inventory)
            {
                if (slot.item == queryItem)
                    return slot.quantity;
            }
            return 0;
        }






        [ContextMenu("Debug Print Inventory")]
        public void DebugPrintFromInspector()
        {
            DebugPrintInventory();
        }
    }
}


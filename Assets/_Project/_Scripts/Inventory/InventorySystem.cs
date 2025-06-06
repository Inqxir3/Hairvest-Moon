﻿using HairvestMoon.Inventory;
using HairvestMoon;
using System.Collections.Generic;
using UnityEngine;
using System;
using HairvestMoon.Farming;
using HairvestMoon.Core;

namespace HairvestMoon.Inventory
{
    /// <summary>
    /// Manages the player's inventory, including adding items, tracking discovered items, and querying item quantities.
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
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

        public void MarkDiscovered(ItemData item)
        {
            if (!discoveredItems.Contains(item))
            {
                discoveredItems.Add(item);
                Debug.Log($"Discovered new item: {item.itemID}");
            }
        }

        public bool AddItem(ItemData newItem, int amount = 1)
        {
            // Farmables: unlimited stacking
            if (newItem.itemType == ItemType.Seed || newItem.itemType == ItemType.Crop)
            {
                foreach (var slot in inventory)
                {
                    if (slot.item == newItem)
                    {
                        slot.quantity += amount;
                        NotifyInventoryChanged();
                        return true;
                    }
                }

                var newSlot = new InventorySlot { item = newItem, quantity = amount };
                inventory.Add(newSlot);
                MarkDiscovered(newItem);
                NotifyInventoryChanged();
                return true;
            }

            // Backpack items: slot limited
            foreach (var slot in inventory)
            {
                if (slot.item == newItem)
                {
                    slot.quantity += amount;
                    NotifyInventoryChanged();
                    return true;
                }
            }

            if (GetCurrentBackpackSlotCount() < maxSlots)
            {
                var newSlot = new InventorySlot { item = newItem, quantity = amount };
                inventory.Add(newSlot);
                MarkDiscovered(newItem);
                NotifyInventoryChanged();
                return true;
            }

            Debug.Log("Backpack Inventory Full");
            return false;
        }

        private int GetCurrentBackpackSlotCount()
        {
            int count = 0;
            foreach (var slot in inventory)
            {
                if (slot.item.itemType != ItemType.Seed && slot.item.itemType != ItemType.Crop)
                    count++;
            }
            return count;
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

        public bool RemoveItem(ItemData item, int amount)
        {
            foreach (var slot in inventory)
            {
                if (slot.item == item)
                {
                    if (slot.quantity < amount)
                        return false;

                    slot.quantity -= amount;
                    if (slot.quantity <= 0)
                        inventory.Remove(slot);

                    NotifyInventoryChanged();
                    return true;
                }
            }
            return false;
        }

        public List<ItemData> GetOwnedItemsByType(ItemType type)
        {
            List<ItemData> result = new();

            foreach (var slot in inventory)
            {
                if (slot.item.itemType == type)
                {
                    result.Add(slot.item);
                }
            }
            return result;
        }

        public List<ItemData> GetDiscoveredItemsByType(ItemType type)
        {
            List<ItemData> result = new();

            foreach (var item in discoveredItems)
            {
                if (item.itemType == type)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public List<InventorySlot> GetAllSlots()
        {
            return inventory;
        }

        public void ForceRefresh()
        {
            NotifyInventoryChanged();
        }

        private void NotifyInventoryChanged()
        {
            ServiceLocator.Get<GameEventBus>().RaiseInventoryChanged();
        }




        [ContextMenu("Debug Print Inventory")]
        public void DebugPrintFromInspector()
        {
            Debug.Log("Inventory Contents:");
            foreach (var slot in inventory)
            {
                Debug.Log($"{slot.quantity}x {slot.item.itemName}");
            }
        }

        [ContextMenu("Debug Add Test Seeds")]
        public void DebugAddTestSeeds()
        {
            foreach (var seedData in ServiceLocator.Get<SeedDatabase>().AllSeeds)
            {
                AddItem(seedData.seedItem, 5);
            }
        }
    }
}
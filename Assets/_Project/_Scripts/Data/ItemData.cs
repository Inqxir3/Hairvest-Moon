using HairvestMoon.Inventory;
using UnityEngine;

namespace HairvestMoon
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite itemIcon;
        public ItemType itemType;
        public int sellValue;

        // Only used if itemType == Seed
        public SeedData associatedSeed;
    }
}

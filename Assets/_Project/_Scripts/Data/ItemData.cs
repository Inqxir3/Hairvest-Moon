using HairvestMoon.Inventory;
using UnityEngine;

namespace HairvestMoon
{
    [CreateAssetMenu(menuName = "Data/Item")]
    public class ItemData : ScriptableObject
    {
        public string itemID;
        public string itemName;
        public string description;
        public Sprite itemIcon;
        public ItemType itemType;
        public int sellPrice;
    }
}

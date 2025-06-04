using HairvestMoon.Core;
using HairvestMoon.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HairvestMoon.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI quantityText;

        private ItemData item;

        public void Initialize(ItemData itemData)
        {
            item = itemData;
        }

        public void UpdateDisplay()
        {
            bool discovered = ServiceLocator.Get<InventorySystem>().discoveredItems.Contains(item);
            iconImage.sprite = discovered ? item.itemIcon : null;
            iconImage.color = discovered ? Color.white : Color.gray;
            nameText.text = discovered ? item.itemName : "????";

            int quantity = ServiceLocator.Get<InventorySystem>().GetQuantity(item);
            quantityText.text = discovered ? quantity.ToString() : "";
        }
    }
}

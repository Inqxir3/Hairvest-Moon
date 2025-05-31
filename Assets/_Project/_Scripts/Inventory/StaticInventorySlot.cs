using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HairvestMoon.Inventory
{
    public class StaticInventorySlot : MonoBehaviour
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
            bool discovered = InventorySystem.Instance.discoveredItems.Contains(item);
            iconImage.sprite = discovered ? item.itemIcon : null;
            iconImage.color = discovered ? Color.white : Color.gray;
            nameText.text = discovered ? item.itemName : "????";

            int quantity = InventorySystem.Instance.GetQuantity(item);
            quantityText.text = discovered ? quantity.ToString() : "";
        }
    }
}
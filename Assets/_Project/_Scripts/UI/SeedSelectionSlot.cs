using HairvestMoon.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HairvestMoon.UI
{
    public class SeedSelectionSlot : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button selectButton;
        [SerializeField] private Image highlightImage;

        private ItemData itemData;
        private System.Action<ItemData> onClick;

        public void Initialize(ItemData item, System.Action<ItemData> clickCallback)
        {
            itemData = item;
            onClick = clickCallback;

            bool discovered = InventorySystem.Instance.discoveredItems.Contains(itemData);
            iconImage.sprite = discovered ? itemData.itemIcon : null;
            iconImage.color = discovered ? Color.white : Color.gray;
            nameText.text = discovered ? itemData.itemName : "????";

            selectButton.onClick.AddListener(() => onClick?.Invoke(itemData));
        }

        public void SetSelected(bool isSelected)
        {
            highlightImage.gameObject.SetActive(isSelected);
        }

        public ItemData GetItemData() => itemData;
    }
}
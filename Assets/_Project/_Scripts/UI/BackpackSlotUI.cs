using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HairvestMoon.UI
{
    public class BackpackSlotUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private Image highlightImage;
        [SerializeField] private Button selectButton;

        private ItemData item;
        private System.Action<ItemData> onClick;

        public void Initialize(ItemData itemData, int quantity, System.Action<ItemData> clickCallback)
        {
            item = itemData;
            onClick = clickCallback;

            iconImage.sprite = item.itemIcon;
            iconImage.color = Color.white;
            quantityText.text = quantity > 1 ? quantity.ToString() : "";

            selectButton.onClick.AddListener(() => onClick?.Invoke(item));
            SetSelected(false);
        }

        public void SetSelected(bool isSelected)
        {
            highlightImage.gameObject.SetActive(isSelected);
        }
    }
}

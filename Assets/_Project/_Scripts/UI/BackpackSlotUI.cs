using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HairvestMoon.UI
{
    public class BackpackSlotUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI quantityText;

        private ItemData item;

        public void Initialize(ItemData itemData, int quantity)
        {
            item = itemData;

            iconImage.sprite = item.itemIcon;
            iconImage.color = Color.white;
            nameText.text = item.itemName;
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
        }
    }
}

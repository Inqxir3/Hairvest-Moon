using TMPro;
using UnityEngine;
using HairvestMoon.Inventory;

namespace HairvestMoon.UI
{
    public class ItemDescriptionUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text typeText;
        [SerializeField] private TMP_Text sellPriceText;

        public void SetItem(ItemData item)
        {
            nameText.text = item.itemName;
            descriptionText.text = item.description;
            typeText.text = item.itemType.ToString();
            sellPriceText.text = $"Sell Price: {item.sellPrice}";
        }

        public void Clear()
        {
            nameText.text = "";
            descriptionText.text = "";
            typeText.text = "";
            sellPriceText.text = "";
            descriptionText.text = "";
        }
    }
}

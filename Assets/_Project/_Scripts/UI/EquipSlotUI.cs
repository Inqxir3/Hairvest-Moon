using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HairvestMoon.Inventory;

namespace HairvestMoon.UI
{
    public class EquipSlotUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text nameText;

        public void SetSlot(ItemData item)
        {
            if (item != null)
            {
                iconImage.sprite = item.itemIcon;
                iconImage.color = Color.white;
                nameText.text = item.itemName;
            }
            else
            {
                iconImage.color = Color.clear;
                nameText.text = "";
            }
        }
    }
}

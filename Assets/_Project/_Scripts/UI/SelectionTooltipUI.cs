using TMPro;
using UnityEngine;
using HairvestMoon.Inventory;

namespace HairvestMoon.UI
{
    public class SelectionTooltipUI : MonoBehaviour
    {
        [SerializeField] private GameObject tooltipPanel;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;

        public void InitializeUI() { HideTooltip(); }

        public void ShowTooltip(ItemData item)
        {
            if (item == null)
            {
                nameText.text = "Normal Water";
                descriptionText.text = "";
            }
            else
            {
                nameText.text = item.itemName;
                descriptionText.text = item.description;
            }

            tooltipPanel.SetActive(true);
        }

        public void HideTooltip()
        {
            tooltipPanel.SetActive(false);
        }
    }
}

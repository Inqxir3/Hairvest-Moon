using UnityEngine;
using TMPro;
using UnityEngine.UI;
using HairvestMoon.Inventory;
using HairvestMoon.Core;

namespace HairvestMoon.UI
{
    public class UpgradeSelectionSlot : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Image highlightImage;
        [SerializeField] private Button selectButton;
        [SerializeField] private Sprite defaultNormalSprite;

        public ItemData Item { get; private set; }

        public void Initialize(ItemData item, System.Action<ItemData> clickCallback)
        {
            Item = item;

            if (item != null)
            {
                iconImage.sprite = item.itemIcon;
            }
            else
            {
                iconImage.sprite = defaultNormalSprite;
            }

            selectButton.onClick.AddListener(() => clickCallback?.Invoke(item));
        }

        public void SetSelected(bool selected)
        {
            highlightImage.gameObject.SetActive(selected);
        }

        private void OnMouseEnter()
        {
            ServiceLocator.Get<SelectionTooltipUI>().ShowTooltip(Item);
        }

        private void OnMouseExit()
        {
            ServiceLocator.Get<SelectionTooltipUI>().HideTooltip();
        }
    }
}

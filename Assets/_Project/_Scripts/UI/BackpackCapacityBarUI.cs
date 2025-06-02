using HairvestMoon.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace HairvestMoon.UI
{
    public class BackpackCapacityBarUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        private void Start()
        {
            BackpackInventorySystem.Instance.OnBackpackChanged += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            BackpackInventorySystem.Instance.OnBackpackChanged -= Refresh;
        }

        private void Refresh()
        {
            int current = BackpackInventorySystem.Instance.GetAllSlots().Count;
            int total = BackpackUpgradeManager.Instance.GetCurrentSlots();

            float fillAmount = (float)current / total;
            fillImage.fillAmount = fillAmount;
        }
    }
}

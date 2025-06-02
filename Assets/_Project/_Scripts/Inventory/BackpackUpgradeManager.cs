using UnityEngine;

namespace HairvestMoon.Inventory
{
    public class BackpackUpgradeManager : MonoBehaviour
    {
        public static BackpackUpgradeManager Instance { get; private set; }

        [Header("Upgrade Settings")]
        [SerializeField] private int baseSlots = 10;
        [SerializeField] private int maxUpgrades = 10;
        [SerializeField] private int slotsPerUpgrade = 2;

        public int SlotsPerUpgrade => slotsPerUpgrade;
        public int BaseSlots => baseSlots;

        private int upgradeLevel = 0;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            ApplyUpgrade();
        }

        public void UpgradeBackpack()
        {
            if (upgradeLevel < maxUpgrades)
            {
                upgradeLevel++;
                ApplyUpgrade();
                Debug.Log($"Backpack upgraded to level {upgradeLevel}");
            }
            else
            {
                Debug.Log("Backpack fully upgraded.");
            }
        }

        private void ApplyUpgrade()
        {
            int totalSlots = baseSlots + (upgradeLevel * slotsPerUpgrade);
            BackpackInventorySystem.Instance.maxBackpackSlots = totalSlots;
            BackpackInventorySystem.Instance.ForceRefresh();
        }


        public int GetUpgradeLevel() => upgradeLevel;
        public int GetMaxUpgrades() => maxUpgrades;
        public int GetCurrentSlots() => BackpackInventorySystem.Instance.maxBackpackSlots;
    }
}

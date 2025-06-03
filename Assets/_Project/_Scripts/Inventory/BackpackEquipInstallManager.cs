using UnityEngine;

namespace HairvestMoon.Inventory
{
    public class BackpackEquipInstallManager : MonoBehaviour
    {
        public static BackpackEquipInstallManager Instance { get; private set; }

        public void InitializeSingleton()
        {
            Instance = this;
        }

        public bool TryEquip(ItemData item)
        {
            if (item == null) return false;

            if (item.itemType == ItemType.Tool)
            {
                BackpackEquipSystem.Instance.EquipTool(item);
                return true;
            }

            if (item.itemType == ItemType.Upgrade)
            {
                BackpackEquipSystem.Instance.EquipUpgrade(item);
                return true;
            }

            return false;
        }
    }
}

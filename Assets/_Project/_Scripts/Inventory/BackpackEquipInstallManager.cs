using HairvestMoon.Core;
using UnityEngine;

namespace HairvestMoon.Inventory
{
    public class BackpackEquipInstallManager : MonoBehaviour
    {
        public bool TryEquip(ItemData item)
        {
            if (item == null) return false;

            if (item.itemType == ItemType.Tool)
            {
                ServiceLocator.Get<BackpackEquipSystem>().EquipTool(item);
                return true;
            }

            if (item.itemType == ItemType.Upgrade)
            {
                ServiceLocator.Get<BackpackEquipSystem>().EquipUpgrade(item);
                return true;
            }

            return false;
        }
    }
}

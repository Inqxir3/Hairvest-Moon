using System.Collections.Generic;
using UnityEngine;

namespace HairvestMoon
{
    [CreateAssetMenu(fileName = "CropInventoryData", menuName = "Inventory/Crop Inventory Data")]
    public class CropInventoryData : ScriptableObject
    {
        public List<ItemData> allCropItems;
        public List<ItemData> allSeedItems;
    }
}


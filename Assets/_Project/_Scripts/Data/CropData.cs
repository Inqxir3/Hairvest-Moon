using UnityEngine;

namespace HairvestMoon
{
    [CreateAssetMenu(menuName = "Data/Crop")]
    public class CropData : ScriptableObject
    {
        public string cropName; 
        public Sprite cropIcon;
        public Sprite[] growthStages;
        public int growthDurationMinutes = 720;
        public int harvestYield = 1;
        public ItemData harvestedItem;

        public ItemData GetHarvestItem()
        {
            return harvestedItem;
        }
    }
}
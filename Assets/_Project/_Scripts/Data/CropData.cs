using UnityEngine;

namespace HairvestMoon
{
    [CreateAssetMenu(fileName = "NewCropData", menuName = "Crops/Crop Data")]
    public class CropData : ScriptableObject
    {
        public string cropName; 
        public float growthDurationMinutes;
        public float growthRateModifier = 1f;
        public Sprite[] growthStages;

        // Optional extras for the future:
        public int sellValue;
        public bool regrowsAfterHarvest;
    }
}
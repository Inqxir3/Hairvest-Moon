using UnityEngine;

namespace HairvestMoon
{
    [CreateAssetMenu(fileName = "NewCropData", menuName = "Crops/Crop Data")]
    public class CropData : ScriptableObject
    {
        public string cropName;
        public int growthDuration;
        public Sprite[] growthStages;

        // Optional extras for the future:
        public int sellValue;
        public bool regrowsAfterHarvest;
    }
}
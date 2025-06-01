using UnityEngine;
using UnityEngine.UI;

namespace HairvestMoon
{
    [CreateAssetMenu(menuName = "Data/Seed")]
    public class SeedData : ScriptableObject
    {
        public ItemData seedItem; 
        public CropData cropData;
    }
}
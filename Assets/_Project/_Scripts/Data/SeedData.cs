using UnityEngine;
using UnityEngine.UI;

namespace HairvestMoon
{
    [CreateAssetMenu(fileName = "NewSeed", menuName = "Crops/Seed")]
    public class SeedData : ScriptableObject
    {
        public string seedName;
        public CropData cropData;
        public Sprite seedSprite;
    }
}
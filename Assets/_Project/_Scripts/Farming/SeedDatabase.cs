using System.Collections.Generic;
using UnityEngine;

namespace HairvestMoon.Farming
{
    public class SeedDatabase : MonoBehaviour
    {
        [SerializeField] private List<SeedData> allSeeds;

        private Dictionary<ItemData, SeedData> lookup = new();

        public List<SeedData> AllSeeds => allSeeds;

        public void InitializeSeedDatabase()
        {
            // Build dictionary now that list is loaded
            foreach (var seed in allSeeds)
            {
                if (seed.seedItem != null)
                    lookup[seed.seedItem] = seed;
            }
        }

        public SeedData GetSeedDataByItem(ItemData item)
        {
            lookup.TryGetValue(item, out var seedData);
            return seedData;
        }
    }
}
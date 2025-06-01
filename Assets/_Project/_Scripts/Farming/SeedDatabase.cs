using System.Collections.Generic;
using UnityEngine;

namespace HairvestMoon.Farming
{
    public class SeedDatabase : MonoBehaviour
    {
        public static SeedDatabase Instance { get; private set; }

        [SerializeField] private List<SeedData> allSeeds;

        private Dictionary<ItemData, SeedData> lookup = new();

        public List<SeedData> AllSeeds => allSeeds;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

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
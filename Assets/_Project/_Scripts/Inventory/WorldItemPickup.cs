using UnityEngine;

namespace HairvestMoon.Inventory
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WorldItemPickup : MonoBehaviour
    {
        public ItemData itemData;
        public int quantity = 1;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            ApplyItemVisual();
        }

        private void ApplyItemVisual()
        {
            if (itemData != null)
            {
                spriteRenderer.sprite = itemData.itemIcon;
            }
            else
            {
                Debug.LogWarning("ItemData not assigned on WorldItemPickup.");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (itemData.itemType == ItemType.Seed || itemData.itemType == ItemType.Crop)
                {
                    InventorySystem.Instance.AddItem(itemData, quantity);
                }
                else
                {
                    BackpackInventorySystem.Instance.AddItem(itemData, quantity);
                }
                Destroy(gameObject);
            }
        }

    }
}
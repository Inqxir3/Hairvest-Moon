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
                bool added = InventorySystem.Instance.AddItem(itemData, quantity);
                if (added)
                {
                    Debug.Log($"Picked up {quantity}x {itemData.itemID}");
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Inventory full - could not pick up item");
                }
            }
        }
    }
}
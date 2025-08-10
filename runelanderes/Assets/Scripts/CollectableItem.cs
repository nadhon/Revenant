using System;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private CollectableItemData collectableItemData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public event Action<CollectableItemData> OnItemCollected;

    void OnValidate()
    {
        if (collectableItemData != null)
        {
            spriteRenderer.sprite = collectableItemData.sprite;
        }
        else
        {
            Debug.LogError(message: "CollectableItemData is not assigned in the CollectableItem script.");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnItemCollected?.Invoke(collectableItemData);
            Destroy(gameObject);
        }
    }
}

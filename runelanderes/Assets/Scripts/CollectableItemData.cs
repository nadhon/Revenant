using UnityEditor;
using UnityEngine;



[CreateAssetMenu(fileName = "CollectableItemData", menuName = "ScriptableObjects/CollectableItem", order = 1)]

public class CollectableItemData : ScriptableObject
{
    public Sprite sprite;
    public GameObject prefab;
    public CollectableItemType Type;
}

public enum CollectableItemType
{
    Coin,
    Gem,
    PowerUp
}

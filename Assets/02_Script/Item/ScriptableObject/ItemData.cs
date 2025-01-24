using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Resource,
    Equippable,
    Edible
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("ItemData")]
    [SerializeField] public ItemType   ItemType;
    [SerializeField] public string     Name;
    [SerializeField] public GameObject Prefab;
    [SerializeField] public Sprite     Image;

    [SerializeField] public bool IsStackable;  // 인벤토리에 중첩해서 쌓을 수 있나요?
    [SerializeField] public int  MaxStackSize; // 그렇다면 몇 개 까지 쌓을 수 있나요?
}

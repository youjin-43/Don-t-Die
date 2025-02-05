using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Resource,
    Equippable,
    Edible,
    Installable,
    Inventory
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("ItemData")]
    [SerializeField] public ItemType   ItemType;
    [SerializeField] public string     Name;
    [SerializeField] public string     NameKR;
    [SerializeField] public GameObject Prefab;
    [SerializeField] public Sprite     Image;

    [SerializeField] public bool IsCountable;  // 인벤토리에 중첩해서 쌓을 수 있나요?
    [SerializeField] public int  MaxCountSize; // 그렇다면 몇 개 까지 쌓을 수 있나요?
}

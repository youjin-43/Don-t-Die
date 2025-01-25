using UnityEngine;

[CreateAssetMenu(fileName = "ChestItemData", menuName = "Item/ChestItemData")]
public class ChestItemData : EquippableItemData
{
    [Header("ChestItemData")]
    [SerializeField] float def;
}

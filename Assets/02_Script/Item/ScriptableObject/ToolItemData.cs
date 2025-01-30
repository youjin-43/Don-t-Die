using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolItemData", menuName = "Item/ToolItemData")]
public class ToolItemData : EquippableItemData
{
    [Header("ToolItemData")]
    [SerializeField] List<ObjectType> availableTypes;
    [SerializeField] float atk;

    public List<ObjectType> AvailableTypes { get { return availableTypes; } }
}

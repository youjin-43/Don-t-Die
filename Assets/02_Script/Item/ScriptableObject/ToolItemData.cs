using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    Unknown,
    Sword,
    Axe,
    Pickaxe,
    Rod
}

[CreateAssetMenu(fileName = "ToolItemData", menuName = "Item/ToolItemData")]
public class ToolItemData : EquippableItemData
{
    [Header("ToolItemData")]
    [SerializeField] List<ObjectType> availableTypes; // 상호작용 가능한 오브젝트 종류 
    [SerializeField] float atk;

    public List<ObjectType> AvailableTypes { get { return availableTypes; } }
    public float Atk { get{return atk;}}

    public ToolType Type;

}

using UnityEngine;

public enum EquipmentSlot
{
    Unknown,
    Head,
    Hand,
    Chest
}

public class EquippableItemData : ItemData
{
    [SerializeField] EquipmentSlot equipSlot;
    
    // 방어력, 공격력 등 추가 작성

    public EquipmentSlot EquipSlot { get { return equipSlot; } }
}
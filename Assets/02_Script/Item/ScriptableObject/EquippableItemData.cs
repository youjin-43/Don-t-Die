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

    public EquipmentSlot EquipSlot { get { return equipSlot; } }
}
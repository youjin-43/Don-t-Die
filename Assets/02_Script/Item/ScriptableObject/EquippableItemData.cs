using UnityEngine;

public enum EquipmentSlot
{
    Unknown,
    Head,
    Hand,
    Chest
}

[CreateAssetMenu(menuName = "Item/EquippableItemData")]
public class EquippableItemData : ItemData
{
    [SerializeField] EquipmentSlot equipSlot;
    [SerializeField] float maxDurability;

    public EquipmentSlot EquipSlot { get { return equipSlot; } }
}
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
    [Header("EquipmentItemData")]
    [SerializeField] EquipmentSlot equipSlot;

    [SerializeField] public int maxDurability;
    [SerializeField] public int currentDurability;

    public EquipmentSlot EquipSlot { get { return equipSlot; } }
}
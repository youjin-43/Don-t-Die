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

    [Header("BottleItem Only")]
    [SerializeField] public int ChargePerUse;
    [SerializeField] public bool ThisissBottle;

    public EquipmentSlot EquipSlot { get { return equipSlot; } }
}
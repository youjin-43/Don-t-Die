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

    [SerializeField] public float maxDurability;
    [SerializeField] public float currentDurability;

    public EquipmentSlot EquipSlot { get { return equipSlot; } }
}
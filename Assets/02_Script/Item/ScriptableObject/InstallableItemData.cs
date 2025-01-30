using UnityEngine;

[CreateAssetMenu(menuName = "Item/InstallableItemData")]
public class InstallableItemData : ItemData
{
    [Header("InstallableItemData")]
    [SerializeField] int TestData;
    [SerializeField] int width;
    [SerializeField] int height;
}

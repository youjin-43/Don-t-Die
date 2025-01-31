using UnityEngine;

[CreateAssetMenu(menuName = "Item/InstallableItemData")]
public class InstallableItemData : ItemData
{
    [Header("InstallableItemData")]
    [SerializeField] int TestData;
    [SerializeField] int width;
    [SerializeField] int height;

    public int Width { get { return width; } }
    public int Height { get { return height; } }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeadItemData", menuName = "Item/HeadItemData")]
public class HeadItemData : EquippableItemData
{
    [Header("HeadItemData")]
    [SerializeField] float def;
}

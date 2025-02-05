using System;
using UnityEngine;

[Serializable]
public class DropItem
{
    public ItemData data;
    public int minAmount;   // 드랍되는 최소 수량
    public int maxAmount;   // 드랍되는 최대 수량
}
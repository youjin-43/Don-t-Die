using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Environment/Object/NatureResourceData")]
public class NatureResourceData : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] BiomeType biomeType;
    [SerializeField] ObjectType objectType;
    [SerializeField] int width;
    [SerializeField] int height;

    [Serializable]
    public struct DropItem
    {
        // TODO: Item 정보 넣기
        public int minAmount;   // 드랍되는 최소 수량
        public int maxAmount;   // 드랍되는 최대 수량
    }

    public List<DropItem> dropItems = new List<DropItem>();

    public GameObject Prefab { get { return prefab; } }
    public BiomeType BiomeType { get { return biomeType; } }
    public ObjectType ObjectType { get { return objectType; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }
}

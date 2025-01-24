using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string MonsterName;
    public float MaxHP;
    public float MoveSpeed;
    public float moveInterval;
    public float AttackDamage;
    public BiomeType biomeType;

    public List<ItemData> dropItems;
    public int MaxDrops;
    public int MinDrops;
}

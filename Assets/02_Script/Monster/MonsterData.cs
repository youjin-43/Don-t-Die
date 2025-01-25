using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string MonsterName;
    public BiomeType biomeType;
    [Space(10f)]
    public float MoveSpeed;
    public float moveInterval;
    public float MaxHP;    
    public float AttackDamage;
    [Space(10f)]
    public List<ItemData> dropItems;
    public int MaxDrops;
    public int MinDrops;
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string MonsterName;
    public BiomeType MyBiomeType;
    [Space(10f)]

    public float MoveSpeed;
    public float MoveInterval;
    public float MaxHP;
    public float ChaseSpeed;
    public float AttackDamage;
    [Space(10f)]

    public List<DropItem> DropItems;
}

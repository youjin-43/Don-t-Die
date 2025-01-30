using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string MonsterName;
    public BiomeType MyBiomeType;

    [Space(10f)]
    [Header("Move")]
    public float MoveSpeed;
    public float MoveInterval;

    [Space(10f)]
    [Header("Hit")]
    public float MaxHP;
    public float ChaseOrFleeSpeed;
    public float AttackDamage;

    [Space(10f)]
    [Header("Knockback")]
    public float KnockbackForce;
    public float KnockbackDuration;

    [Space(10f)]
    public List<DropItem> DropItems;
}

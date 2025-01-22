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

    //TODO : dropItems 추가 
}

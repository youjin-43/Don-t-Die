using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    [SerializeField]  public string MonsterName;
    [SerializeField]  public float MaxHP;
    [SerializeField]  public float MoveSpeed;
    [SerializeField]  public float AttackDamage;
    public BiomeType biomeType;

    //TODO : dropItems 추가 
}

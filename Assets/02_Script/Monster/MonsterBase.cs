using UnityEngine;

// 몬스터들의 공통 속성 및 동작 정의
public abstract class MonsterBase : MonoBehaviour
{
    [Header("Monster Base Attributes")]
    public MonsterData monsterData;

    public string MonsterName;
    public float CurrentHp;
    public float MoveSpeed;
    public float AttackDamage;
    //TODO : dropItems 추가 

    public abstract void Move(Vector3 targetPosition);

    public abstract void Attack(GameObject target); // 공격 로직 (구체적 동작은 각 몬스터가 구현)

    public virtual void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0) Die();
    }

    protected virtual void Die()
    {
        Debug.Log($"{MonsterName} has died!");
        Destroy(gameObject);
    }

}

using UnityEngine;

// 몬스터들의 공통 속성 및 동작 정의
public abstract class MonsterBase : MonoBehaviour, IDamageable
{
    
    public MonsterData monsterData;

    [Header("Monster Base Attributes")]
    public string MonsterName;
    public float CurrentHp;
    public float MoveSpeed;
    public float MoveInterval;
    public float AttackDamage;
    public BiomeType MybiomeType;
    //TODO : dropItems 추가

    protected Animator monsterAnimator;

    protected void SetCompoenets()
    {
        monsterAnimator = GetComponent<Animator>();
    }

    protected void SetData()
    {
        MonsterName = monsterData.MonsterName;
        CurrentHp = monsterData.MaxHP;
        MoveSpeed = monsterData.MoveSpeed;
        MoveInterval = monsterData.moveInterval;
        AttackDamage = monsterData.AttackDamage;
        MybiomeType = monsterData.biomeType;
    }

    public abstract void Move();
    //public abstract void Attack(GameObject target); // 공격 로직 (구체적 동작은 각 몬스터가 구현)

    public virtual void TakeDamage(int damage)
    {
        DebugController.Log($"{transform.name} took {damage} damage : Current HP : {CurrentHp}");
        monsterAnimator.SetTrigger("TakeDamage");
        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            Die();
            monsterAnimator.SetTrigger("Die");
        }
    }

    protected virtual void Die()
    {
        DebugController.Log($"{transform.name} has died!");
        Destroy(gameObject,3f); // TODO : 오브젝트 풀로 변경 
    }


}

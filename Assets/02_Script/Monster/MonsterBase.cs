using UnityEngine;

// 몬스터들의 공통 속성 및 동작 정의
public abstract class MonsterBase : MonoBehaviour
{
    
    public MonsterData monsterData;

<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
    [Header("Monster Base Attributes")]
>>>>>>> Stashed changes
=======
    [Header("Monster Base Attributes")]
>>>>>>> Stashed changes
    public string MonsterName;
    public float CurrentHp;
    public float MoveSpeed;
    public float AttackDamage;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    //TODO : dropItems 추가 

    public abstract void Move(Vector3 targetPosition);

    public abstract void Attack(GameObject target); // 공격 로직 (구체적 동작은 각 몬스터가 구현)
=======
=======
>>>>>>> Stashed changes
    public BiomeType MybiomeType;
    

    //TODO : dropItems 추가

    protected void SetData()
    {
        MonsterName = monsterData.MonsterName;
        CurrentHp = monsterData.MaxHP;
        MoveSpeed = monsterData.MoveSpeed;
        AttackDamage = monsterData.AttackDamage;
        MybiomeType = monsterData.biomeType;
    }

    public abstract void Move();
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

    public virtual void TakeDamage(float damage)
    {
        DebugController.Log($"{transform.name} took {damage} damage : Current HP : {CurrentHp}");
        CurrentHp -= damage;
        if (CurrentHp <= 0) Die();
    }

    protected virtual void Die()
    {
        DebugController.Log($"{transform.name} has died!");
        Destroy(gameObject);
    }

}

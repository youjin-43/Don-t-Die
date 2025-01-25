using System.Collections.Generic;
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

    //public List<ItemData> dropItems;
    //public int MaxDrops;
    //public int MinDrops;

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

        //dropItems = monsterData.dropItems;
        //MaxDrops = monsterData.MaxDrops;
        //MinDrops = monsterData.MinDrops;

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
            //아이템 스폰 
            monsterAnimator.SetTrigger("Die");
        }
    }

    protected virtual void Die()
    {
        DebugController.Log($"{transform.name} has died!");
        Destroy(gameObject, 3f); // TODO : 오브젝트 풀로 변경 
    }

    protected virtual void SpreadItems()
    {
        foreach (var item in monsterData.dropItems)
        {
            int count = UnityEngine.Random.Range(monsterData.MinDrops, monsterData.MaxDrops+ 1);

            while (count > 0)
            {
                Item go = PoolManager.Instance.InstantiateItem(item);

                // 긴데 별거 없습니다.. 플레이어 반대 방향으로 뿌리겠다는 뜻
                Vector3 dir = transform.position +
                    (transform.position - GameManager.Instance.GetPlayerPos()
                    + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));

                go.Spread(transform.position, dir, Random.Range(2f, 2.5f));
                count--;
            }
        }
    }

}
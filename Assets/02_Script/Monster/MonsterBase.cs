using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Idle,
    Attack,
    Die
}

// 몬스터들의 공통 속성 및 동작 정의
public abstract class MonsterBase : MonoBehaviour, IDamageable, IItemDroppable
{
    [Header("Monster Base Attributes")]
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected MonsterState currnetState;
    [SerializeField] protected float moveRange = 3f;
    [SerializeField] float CurrentHp;

    protected Animator monsterAnimator;

    protected void SetCompoenets()
    {
        monsterAnimator = GetComponent<Animator>();
    }

    protected void SetData()
    {
        CurrentHp = monsterData.MaxHP;
        currnetState = MonsterState.Idle; //Idle로 시작 
    }

    public abstract void Move();
    //public abstract void Attack(GameObject target); // 공격 로직 (구체적 동작은 각 몬스터가 구현) // TODO : 공격가능한 애들도 인터페이스로? 

    public virtual void TakeDamage(int damage) //TODO : 선택된 도구의 공경력을 받아오도록 
    {
        DebugController.Log($"{transform.name} took {damage} damage -> Current HP : {CurrentHp}");

        monsterAnimator.SetTrigger("TakeDamage"); //TODO : 피격 이미지 박쥐 참고해서 좀 수정하면 좋을것 같음 
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
        DropItems(); //아이템 스폰 
        ChangeToDieState();
        Destroy(gameObject, 3f); // TODO : 오브젝트 풀로 변경 
    }


    public void DropItems()
    {
        foreach (var item in monsterData.dropItems)
        {
            int count = UnityEngine.Random.Range(monsterData.MinDrops, monsterData.MaxDrops + 1);

            while (count > 0)
            {
                Item go = PoolManager.Instance.InstantiateItem(item);

                // 긴데 별거 없습니다.. 플레이어 반대 방향으로 뿌리겠다는 뜻
                Vector3 dir = transform.position +
                    (transform.position - GameManager.Instance.GetPlayerPos()
                    + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));

                go.Spread(transform.position, dir, UnityEngine.Random.Range(2f, 2.5f));
                count--;
            }
        }
    }

    #region ChangeStateFunc
    public void ChangeToIdleState(){ currnetState = MonsterState.Idle; }
    public void ChangeToDieState(){ currnetState = MonsterState.Die; }
    public void ChangeToAttackState(){ currnetState = MonsterState.Attack; }
    #endregion

}
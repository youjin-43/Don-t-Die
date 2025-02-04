using UnityEngine;

/// <summary>
/// 플레이어를 공격
/// </summary>
public class AttackMonsterState : IMonsterState
{
    MonsterBase monster;

    //private Transform target;
    private IDamageable target;
    private float attackCooldown = 3f; // 공격 간격 타이머
    private float lastAttackTime; // 마지막 공격 시점

    public AttackMonsterState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Attack 상태로 진입!");

        target = monster.Target.GetComponent<IDamageable>();
        lastAttackTime = -attackCooldown; // 상태 진입 시 즉시 공격
    }

    public void ExitState() { 
    }

    public void UpdateState()
    {
        // 공격 대상이 없으면 idle로 상태 전환 
        if (target == null || target.IsDead())
        {
            Debug.Log("공격 대상이 없어 idle 상태로 전환합니다");
            monster.OnIdle();
            return;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            monster.PerformAttack();
        }
    }
}

using UnityEngine;
/// <summary>
/// 플레이어를 추적하는 상태
/// </summary>
public class ChaseMonsterState : IMonsterState
{
    MonsterBase monster;

    Transform target;
    float attackRange; // 공격 사거리


    public ChaseMonsterState(MonsterBase monster)
    {
        this.monster = monster;
        attackRange = monster.monsterData.AttackRange;
    }


    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Chase 상태로 진입!");
        target = monster.Target;
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
        // 공격 대상이 없으면 idle로 상태 전환 
        if (target == null || target.GetComponent<IDamageable>().IsDead())
        {
            Debug.Log("공격 대상이 없어 idle 상태로 전환합니다");
            monster.OnIdle();
            return;
        }

        // Vector3.Distance를 sqrMagnitude로 변경하여 성능 개선
        Vector3 dir = target.position - monster.transform.position;
        float sqrDistance = dir.sqrMagnitude;

        if (sqrDistance <= attackRange * attackRange)
        {
            Debug.Log("target이 공격범위 안으로 들어왔으므로 공격상태로 전환합니다 ");
            monster.OnAttack();
        }
        else
        {
            monster.ChaseTarget();
            HandleAnimation();
        }
    }

    // TODO : 이거 꼭 매프레임 파라미터를 설정해야하나..?
    void HandleAnimation()
    {
        // 애니메이션 설정
        Vector3 dir = target.position - monster.transform.position;
        monster.SetIsMovingAnimation(true);
        monster.SetDirnimaiton(dir.x, dir.y);

    }
}

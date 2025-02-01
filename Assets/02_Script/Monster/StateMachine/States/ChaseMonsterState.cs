using UnityEngine;
/// <summary>
/// 플레이어를 추적하는 상태
/// </summary>
public class ChaseMonsterState : IMonsterState
{
    MonsterBase monster;

    Transform target;
    float chaseSpeed;
    float attackRange; // 공격 사거리


    public ChaseMonsterState(MonsterBase monster)
    {
        this.monster = monster;
        chaseSpeed = monster.monsterData.ChaseOrFleeSpeed;
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
        if (target != null)
        {
            float distance = Vector3.Distance(monster.transform.position, target.position);

            if (distance <= attackRange)
            {
                monster.OnAttack();  // 공격 상태로 전환
            }
            else
            {
                ChaseTarget();
                HandleAnimation();
            }
        }
        else
        {
            monster.OnIdle();
        }
    }

    void ChaseTarget()
    {
        //Debug.Log($"내 위치 : {monster.transform.position}, 타겟 위치 : {target.position}");
        monster.transform.position = Vector3.MoveTowards(monster.transform.position, target.position, chaseSpeed * Time.deltaTime);

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

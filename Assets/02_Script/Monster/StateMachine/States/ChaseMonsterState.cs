using UnityEngine;
/// <summary>
/// 플레이어를 추적하는 상태
/// </summary>
public class ChaseMonsterState : IMonsterState
{
    MonsterBase monster;
    Animator monsterAnimator;
    Transform target;
    float chaseSpeed;

    public ChaseMonsterState(MonsterBase monster)
    {
        this.monster = monster;
        monsterAnimator = monster.MonsterAnimator;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Chase 상태로 진입!");
        target = monster.Target;
        chaseSpeed = monster.ChaseSpeed;
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
        ChaseTarget();
        HandleAnimation();
    }

    void ChaseTarget()
    {
        if (target != null) monster.transform.position = Vector3.MoveTowards(monster.transform.position, target.position, chaseSpeed * Time.deltaTime);
    }

    void HandleAnimation()
    {
        // 애니메이션 설정
        Vector3 dir = target.position - monster.transform.position;
        monsterAnimator.SetBool("IsMoving", true);
        monsterAnimator.SetFloat("dirX", dir.x);
        monsterAnimator.SetFloat("dirY", dir.y);
    }


}

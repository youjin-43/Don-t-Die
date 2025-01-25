using UnityEngine;
/// <summary>
/// 플레이어를 추적하는 상태
/// </summary>
public class ChaseMonsterState : IMonsterState
{
    MonsterBase monster;

    public ChaseMonsterState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Chase 상태로 진입!");
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
        Debug.Log("");
        //if (Vector3.Distance(transform.position, playerTransform.position) <= monsterData.AttackRange)
        //{
        //    //ChangeState(MonsterState.Attack);
        //    return;
        //}

        //Vector3 direction = (playerTransform.position - transform.position).normalized;

        //// 애니메이션 설정
        //monsterAnimator.SetBool("IsMoving", true);
        //monsterAnimator.SetFloat("dirX", direction.x);
        //monsterAnimator.SetFloat("dirY", direction.y);

        //// 플레이어를 향해 이동
        //transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, monsterData.MoveSpeed * Time.deltaTime);
    }
}

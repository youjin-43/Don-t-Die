using UnityEngine;
/// <summary>
/// 플레이어를 추적하는 상태
/// </summary>
public class ChaseMonsterState : IMonsterState
{
    MonsterBase monster;

    Animator monsterAnimator;
    string isMoving = MonsterAimatorParams.IsMoving.ToString();
    string dirX = MonsterAimatorParams.dirX.ToString();
    string dirY = MonsterAimatorParams.dirY.ToString();

    Transform target;
    float chaseSpeed;

    

    public ChaseMonsterState(MonsterBase monster)
    {
        this.monster = monster;
        monsterAnimator = monster.MonsterAnimator;
        chaseSpeed = monster.monsterData.ChaseOrFleeSpeed;
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
        ChaseTarget();
        HandleAnimation(); // TODO : 이거 꼭 매프레임 파라미터를 설정해야하나..?
    }

    void ChaseTarget()
    {
        if (target != null)
        {
            //Debug.Log($"내 위치 : {monster.transform.position}, 타겟 위치 : {target.position}");
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, target.position, chaseSpeed * Time.deltaTime);
        }
    }

    void HandleAnimation()
    {
        // 애니메이션 설정
        Vector3 dir = target.position - monster.transform.position;
        monsterAnimator.SetBool(isMoving, true);
        monsterAnimator.SetFloat(dirX, dir.x);
        monsterAnimator.SetFloat(dirY, dir.y);
    }


}

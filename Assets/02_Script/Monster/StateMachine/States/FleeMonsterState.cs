using UnityEngine;
/// <summary>
/// 플레이어에게서 도망가는 상태 
/// </summary>
public class FleeMonsterState : IMonsterState
{
    MonsterBase monster;

    Animator monsterAnimator;
    string isMoving = MonsterAimatorParams.IsMoving.ToString();
    string dirX = MonsterAimatorParams.dirX.ToString();
    string dirY = MonsterAimatorParams.dirY.ToString();

    Transform target;
    float fleeSpeed;
    private float fleeDistance = 3f; // 도망칠 거리
    private Vector3 fleeDirection; // 도망가는 방향

    public FleeMonsterState(MonsterBase monster)
    {
        this.monster = monster;
        monsterAnimator = monster.MonsterAnimator;
        fleeSpeed = monster.monsterData.ChaseOrFleeSpeed;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Flee 상태로 진입!");
        target = monster.Target;
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
        // 도망가기 
        Vector3 dir = (monster.transform.position - target.position).normalized;
        monster.transform.position += dir * fleeSpeed * Time.deltaTime;

        // 애니메이션 설정 // TODO : 이거 꼭 매프레임 파라미터를 설정해야하나..?
        monsterAnimator.SetBool(isMoving, true);
        monsterAnimator.SetFloat(dirX, dir.x);
        monsterAnimator.SetFloat(dirY, dir.y);
    }
}

using UnityEngine;
/// <summary>
/// 플레이어에게서 도망가는 상태 
/// </summary>
public class FleeMonsterState : IMonsterState
{
    MonsterBase monster;

    Transform target;
    float fleeSpeed;

    public FleeMonsterState(MonsterBase monster)
    {
        this.monster = monster;
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
        monster.SetIsMovingAnimation(true);
        monster.SetDirnimaiton(dir.x, dir.y);
    }
}

using UnityEngine;
/// <summary>
/// 플레이어에게서 도망가는 상태 
/// </summary>
public class FleeMonsterState : IMonsterState
{
    MonsterBase monster;

    Transform target;

    float fleeDistance = 3f;
    Vector3 fleeDirection;
    float fleeSpeed;
    float fleeTime = 1.0f; // 후퇴하는 시간
    float startTime;

    public FleeMonsterState(MonsterBase monster)
    {
        this.monster = monster;
        fleeSpeed = monster.monsterData.ChaseOrFleeSpeed;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Flee 상태로 진입!");
        target = monster.Target;

        // 플레이어 반대 방향으로 후퇴
        fleeDirection = (monster.transform.position - target.position).normalized;
        startTime = Time.time;
        HandleAnimation();
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {

        if (Time.time - startTime < fleeTime)
        {
            monster.transform.position += fleeDirection * fleeSpeed * Time.deltaTime;
        }
        else
        {
            // 후퇴 후 어떤 상태로 갈 것인지 
            monster.AfterFleeState();
        }
    }

    void HandleAnimation()
    {
        // 애니메이션 설정
        monster.SetIsMovingAnimation(true);
        monster.SetDirnimaiton(fleeDirection.x, fleeDirection.y);
    }
}

using System.Collections;
using UnityEngine;

public class Bear : MonsterBase
{

    // 공격받으면 상태 전환
    protected override void HandleMonsterHit(Transform attacker)
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState) OnChase();
    }

    public override void SetDirnimaiton(float dir_x, float dir_y)
    {
        MonsterAnimator.SetFloat("dirX", dir_x);
        MonsterAnimator.SetFloat("dirY", dir_y);
    }

    // 곰은 사용하지 않음 
    public override void AfterFleeState()
    {
        OnIdle();
    }
}


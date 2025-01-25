using System.Collections;
using UnityEngine;

public class Bear : MonsterBase
{
    protected override void HandleMonsterHit(Transform attacker)
    {
        // 공격받으면 상태 전환
        monsterStateMachine.TransitionTo(monsterStateMachine.chaseMonsterState);
    }

}


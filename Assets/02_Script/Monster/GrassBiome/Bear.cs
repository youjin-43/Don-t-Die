using System.Collections;
using UnityEngine;

public class Bear : MonsterBase
{
    protected override void HandleMonsterHit(Transform attacker)
    {
        // 공격받으면 상태 전환
        OnChase();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"트리거 발동!!! staet : {monsterStateMachine.CurrentState}, other.transform :{other.transform}, Target : {Target}");
        // 쫒는 중인데 타겟이 공격 범위 안에 들어오면  
        if (monsterStateMachine.CurrentState == monsterStateMachine.chaseMonsterState && other.transform == Target)
        {
            Debug.Log("target이 범위 안에 들어옴!");
            OnAttack();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 공격 범위를 벗어나면 다시 쫒기
        if (monsterStateMachine.CurrentState == monsterStateMachine.attackMonsterState && other.transform == Target)
        {
            OnChase();
        }
    }


}


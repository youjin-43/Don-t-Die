using System.Collections;
using UnityEngine;

public class Bear : MonsterBase
{

    // 공격받으면 상태 전환
    protected override void HandleMonsterHit(Transform attacker)
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState) OnChase();
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


    // TODO : 이거 방식이 좀 꼬름 한데 
    void OnTriggerExit2D(Collider2D other)
    {
        // 공격 범위를 벗어나면 다시 쫒기
        if (monsterStateMachine.CurrentState == monsterStateMachine.attackMonsterState && other.transform == Target)
        {
            OnChase();
        }
    }

    public override void SetDirnimaiton(float dir_x, float dir_y)
    {
        MonsterAnimator.SetFloat("dirX", dir_x);
        MonsterAnimator.SetFloat("dirY", dir_y);
    }


}


using UnityEngine;

public class Rat : MonsterBase
{
    protected override void HandleMonsterHit(Transform attacker)
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState) OnFlee();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState)
        {
            if (other.CompareTag("Player")) // 플레이어가 감지 범위 안으로 들어오면
            {
                Target = other.transform;
                OnFlee();
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState)
        {
            if (other.CompareTag("Player")) // 플레이어가 감지 범위에서 벗어나면 
            {
                Target = other.transform;
                OnIdle();
            }
        }
    }
}

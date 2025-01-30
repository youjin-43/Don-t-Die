using UnityEngine;

public class Bat : MonsterBase
{
    protected override void HandleMonsterHit(Transform attacker)
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState) OnChase();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState)
        {
            if (other.CompareTag("Player")) // 플레이어가 감지 범위 안으로 들어오면
            {
                Target = other.transform;
                OnChase();
            }
        }

    }
}

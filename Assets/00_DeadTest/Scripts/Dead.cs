using System.Collections;
using UnityEngine;

public class Dead : MonsterBase
{
    [SerializeField] GameObject fire; // 인스펙터에서 할당
    [SerializeField] float throwSpeed = 5f;

    // 공격받으면 상태 전환
    protected override void HandleMonsterHit(Transform attacker)
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState) OnChase();
    }

    public void ThrownApple()
    {
        if (fire != null && Target != null)
        {
            GameObject thrownApple = Instantiate(fire, transform.position, Quaternion.identity);
            thrownApple.GetComponent<Fire>().Initialize((Target.position - transform.position).normalized, gameObject);
        }
    }
}


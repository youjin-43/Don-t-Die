using System.Collections;
using UnityEngine;

public class Bear : MonsterBase
{
    [SerializeField] GameObject apple; // 인스펙터에서 할당
    [SerializeField] float throwSpeed = 5f;

    // 공격받으면 상태 전환
    protected override void HandleMonsterHit(Transform attacker)
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState) OnChase();
    }

    public void ThrownApple()
    {
        if (apple != null && Target != null)
        {
            GameObject thrownApple = Instantiate(apple, transform.position, Quaternion.identity);
            thrownApple.GetComponent<Apple>().Initialize((Target.position - transform.position).normalized, gameObject);
        }
    }

    public override void SetDirnimaiton(float dir_x, float dir_y)
    {
        MonsterAnimator.SetFloat("dirX", dir_x);
        MonsterAnimator.SetFloat("dirY", dir_y);
    }
}


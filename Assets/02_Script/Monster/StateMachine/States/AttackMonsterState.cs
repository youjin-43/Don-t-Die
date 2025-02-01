using UnityEngine;

/// <summary>
/// 플레이어를 공격
/// </summary>
public class AttackMonsterState : IMonsterState
{
    MonsterBase monster;

    Transform target;

    public AttackMonsterState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Attack 상태로 진입!");
        target = monster.Target;
    }

    public void ExitState() { 

    }

    public void UpdateState()
    {
        // 공격 대상이 없으면 idle로 상태 전환 
        if (target == null)
        {
            Debug.Log("공격 대상이 없어 idle 상태로 전환합니다");
            monster.OnIdle();
            return;
        }

        monster.TriggerAttackAnimaiton();
    }
}

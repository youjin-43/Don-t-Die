using UnityEngine;

public class AttackMonsterState
{
    public void EnterState(MonsterBase monster)
    {
        Debug.Log($"{monster.name} entered Attack state.");
    }

    public void UpdateState(MonsterBase monster)
    {
        // 공격 행동
        //monster.Attack();
    }

    public void ExitState(MonsterBase monster)
    {
        Debug.Log($"{monster.name} exited Attack state.");
    }
}

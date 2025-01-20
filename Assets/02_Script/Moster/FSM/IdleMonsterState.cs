using UnityEngine;

public class IdleMonsterState : IMonsterState
{
    public void EnterState(MonsterBase monster)
    {
        Debug.Log($"{monster.name} entered Idle state.");
    }

    public void ExecuteState(MonsterBase monster)
    {
        // Idle 상태에서 할 행동
    }

    public void ExitState(MonsterBase monster)
    {
        Debug.Log($"{monster.name} exited Idle state.");
    }
}

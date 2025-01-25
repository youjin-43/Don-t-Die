using UnityEngine;

/// <summary>
/// 몬스터 사망
/// </summary>
public class DieMonsterState : IMonsterState
{
    MonsterBase monster;

    public DieMonsterState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Die 상태로 진입!");
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
    }
}

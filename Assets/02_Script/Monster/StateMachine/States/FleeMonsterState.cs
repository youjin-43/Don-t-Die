using UnityEngine;
/// <summary>
/// 플레이어에게서 도망가는 상태 
/// </summary>
public class FleeMonsterState : IMonsterState
{
    MonsterBase monster;

    public FleeMonsterState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Flee 상태로 진입!");
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
    }
}

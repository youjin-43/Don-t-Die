using UnityEngine;

/// <summary>
/// 플레이어를 공격
/// </summary>
public class AttackMonsterState : IMonsterState
{
    MonsterBase monster;

    public AttackMonsterState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Attack 상태로 진입!");
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
    }
}

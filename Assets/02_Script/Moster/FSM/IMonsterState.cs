using UnityEngine;

public interface IMonsterState 
{
    public void EnterState(MonsterBase monster);
    public void ExecuteState(MonsterBase monster);
    public void ExitState(MonsterBase monster);
}

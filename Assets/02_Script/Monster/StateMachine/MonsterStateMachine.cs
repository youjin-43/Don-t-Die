using UnityEngine;

[System.Serializable]
public class MonsterStateMachine 
{
    public IMonsterState CurrentState { get; private set; }

    MonsterBase monster;
    public IdleMonsterState idleMonsterState;
    public FleeMonsterState fleeMonsterState;
    public ChaseMonsterState chaseMonsterState;
    public AttackMonsterState attackMonsterState;
    public DieMonsterState dieMonsterState;

    //생성자
    public MonsterStateMachine(MonsterBase monster)
    {
        this.monster = monster;
        idleMonsterState = new IdleMonsterState(monster);
        fleeMonsterState = new FleeMonsterState(monster);
        chaseMonsterState = new ChaseMonsterState(monster);
        attackMonsterState = new AttackMonsterState(monster);
        dieMonsterState = new DieMonsterState(monster);
    }

    public void Initialize(IMonsterState state)
    {
        CurrentState = state;
        state.EnterState();
    }

    public void TransitionTo(IMonsterState newState)
    {
        if(CurrentState != newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }

    public void Execute()
    {
        CurrentState.UpdateState();
    }
}

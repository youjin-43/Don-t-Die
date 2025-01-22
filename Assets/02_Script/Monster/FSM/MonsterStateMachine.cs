using UnityEngine;

public class MonsterStateMachine
{
    private IMonsterState currentState;

    public void ChangeState(IMonsterState newState, MonsterBase monster)
    {
        if (currentState != null)
        {
            currentState.ExitState(monster); // 기존 상태 종료
        }

        currentState = newState; // 새로운 상태 설정
        currentState.EnterState(monster); // 새로운 상태 시작
    }

    public void UpdateState(MonsterBase monster)
    {
        if (currentState != null)
        {
            currentState.ExecuteState(monster); // 현재 상태 업데이트
        }
    }
}

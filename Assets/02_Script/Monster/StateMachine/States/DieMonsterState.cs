using UnityEngine;

/// <summary>
/// 몬스터 사망
/// </summary>
public class DieMonsterState : IMonsterState
{
    MonsterBase monster;
    Animator monsterAnimator;

    public DieMonsterState(MonsterBase monster)
    {
        this.monster = monster;
        monsterAnimator = monster.MonsterAnimator;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Die 상태로 진입!");
        monsterAnimator.SetTrigger(MonsterAimatorParams.Die.ToString());
        monster.DropItems(); //아이템 스폰 
        //monster.Destroy(monster.gameObject, 3f); // TODO : 오브젝트 풀로 변경 
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
    }
}

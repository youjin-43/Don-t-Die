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
        //Debug.Log($"{monster.gameObject.name} 이 Die 상태로 진입!");

        monster.SetDieAnimation(); // 애니메이션 설정 

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

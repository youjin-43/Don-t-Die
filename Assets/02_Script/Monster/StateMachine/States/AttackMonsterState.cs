using UnityEngine;

/// <summary>
/// 플레이어를 공격
/// </summary>
public class AttackMonsterState : IMonsterState
{
    MonsterBase monster;

    Animator monsterAnimator;
    string Attack = MonsterAimatorParams.Attack.ToString();

    private IDamageable target;
    private float attackPower;
    private float attackCooldown = 0.5f; // 공격 간격 타이머
    private float lastAttackTime; // 마지막 공격 시점

    public AttackMonsterState(MonsterBase monster)
    {
        this.monster = monster;
        monsterAnimator = monster.MonsterAnimator;
        attackPower = monster.monsterData.AttackDamage;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Attack 상태로 진입!");

        target = monster.Target.GetComponent<IDamageable>();
        lastAttackTime = -attackCooldown; // 상태 진입 시 즉시 공격
    }

    public void ExitState() { 

    }

    public void UpdateState()
    {
        // 공격 대상이 없으면 idle로 상태 전환 
        if (target == null)
        {
            Debug.Log("공격 대상이 없어 idle 상태로 전환합니다");
            monster.OnIdle();
            return;
        }

        // 공격 간격이 지났는지 확인
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time; // 마지막 공격 시간 갱신
        }
    }

    private void PerformAttack()
    {
        Debug.Log("공격!");

        // 공격 애니메이션 재생
        monsterAnimator.SetTrigger(Attack);

        // 데미지 계산 및 적용 -> 지금은 플레이어 밖에 공격을 하지 않지만 나중에 적대 몬스터끼리 or 몬스터가 오브젝트를 공격할 수 도 있기 때문에 이렇게 해놓음.
        //if (target.TryGetComponent<IDamageable>(out IDamageable target))
        //{
            target.TakeDamage((int)attackPower); // 몬스터 공격력 기반 데미지 전달
            Debug.Log($"몬스터가 {monster.Target.name}에게 {attackPower} 데미지를 입혔습니다.");

            // 타겟이 죽었는지 확인하고 죽었다면 Idle 상태로 전환
            if (target.IsDead())
            {
                Debug.Log($"{monster.Target.name}이 죽었으므로 공격을 멈춥니다.");
                monster.Target = null; // 타겟 초기화 
                monster.OnIdle();  // 몬스터 공격을 멈추고 Idle 상태로 전환
            }
        //}
    }
}

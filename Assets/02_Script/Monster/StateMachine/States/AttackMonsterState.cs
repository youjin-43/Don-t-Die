using UnityEngine;

/// <summary>
/// 플레이어를 공격
/// </summary>
public class AttackMonsterState : IMonsterState
{
    MonsterBase monster;

    private Transform target;
    IDamageable damageable;
    Rigidbody2D targetRb;
    private float attackPower;
    private float attackCooldown = 2f; // 공격 간격 타이머
    private float lastAttackTime; // 마지막 공격 시점

    float attackRange;

    float fleeDistance = 2.0f; // 공격 후 후퇴할 거리


    public AttackMonsterState(MonsterBase monster)
    {
        this.monster = monster;
        attackPower = monster.monsterData.AttackDamage;
        attackRange = monster.monsterData.AttackRange;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Attack 상태로 진입!");

        target = monster.Target;
        damageable = target.GetComponent<IDamageable>();
        targetRb = monster.Target.GetComponent<Rigidbody2D>();
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

        float distance = Vector3.Distance(monster.transform.position, target.position);

        //// 공격 범위를 벗어나면 다시 추격 상태로
        //if (distance > 1.5f)  // TODO : 공격 범위 받아오기 
        //{
        //    monster.OnChase();
        //}
        //// 공격 간격이 지났는지 확인
        //else if (Time.time - lastAttackTime >= attackCooldown)
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            PerformAttack();
        }
    }

    // TODO : 이거 트리거 기반으로 바꿔야할것 같음
    private void PerformAttack()
    {
        Debug.Log("공격!");

        // 공격 애니메이션 재생
        monster.TriggerAttackAnimaiton();

        // 데미지 계산 및 적용 -> 지금은 플레이어 밖에 공격을 하지 않지만 나중에 적대 몬스터끼리 or 몬스터가 오브젝트를 공격할 수 도 있기 때문에 이렇게 해놓음.
        //damageable.TakeDamage((int)attackPower); // 몬스터 공격력 기반 데미지 전달

        //monster.ApplyKnockback(targetRb, monster.Target.position);

        //Debug.Log($"몬스터가 {monster.Target.name}에게 {attackPower} 데미지를 입혔습니다.");

        // 타겟이 죽었는지 확인하고 죽었다면 Idle 상태로 전환
        //if (damageable.IsDead())
        //{
        //    Debug.Log($"{monster.Target.name}이 죽었으므로 공격을 멈춥니다.");

        //    monster.Target = null; // 타겟 초기화 
        //    monster.OnIdle();  // 몬스터 공격을 멈추고 Idle 상태로 전환
        //}
        //else
        //{
        //    // 공격 후 일정 거리만큼 후퇴하는 상태로 전환
        //    monster.OnFlee();
        //}
    }
}

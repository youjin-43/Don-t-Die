using UnityEngine;

public class Bat : MonsterBase
{
    //인스펙터에서 할당
    [SerializeField] Collider2D detectCollider;
    [SerializeField] Collider2D attackCollider;

    // 플레이어를 공격 후 잠시 거리를 두는 행동
    [SerializeField] private bool isKnockedBack = false; // 넉백 상태 확인
    private Vector2 knockbackDirection; // 넉백 방향
    private float knockbackDuration = 1f; // 넉백 지속 시간
    private float knockbackTime = 0f; // 넉백 시간 카운트

    protected override void HandleMonsterHit(Transform attacker)
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState) OnChase();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (monsterStateMachine.CurrentState == monsterStateMachine.chaseMonsterState)
            {
                // 공격 범위에서 플레이어와 충돌했을 경우
                IDamageable target = other.GetComponent<IDamageable>();
                if (target != null)
                {
                    TriggerAttackAnimaiton(); // 공격 애니메이션 
                    target.TakeDamage(atkDamage);

                    // 넉백 적용
                    Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        ApplyKnockbackToTarget(playerRb, other.transform.position);
                    }

                    attackCollider.enabled = false; // 공격 후 잠시 비활성화
                    Invoke(nameof(EnableAttackCollider), 1f); // 1초 후 다시 활성화
                }
            }
            else if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState)
            {
                // 감지 범위에서 플레이어를 찾았을 경우
                Target = other.transform;
                detectCollider.enabled = false; // 감지 범위 비활성화 (한 번 감지하면 비활성화)
                attackCollider.enabled = true; // 공격 가능 상태로 변경
                OnChase();
            }
        }
    }

    void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public override void ApplyKnockbackToTarget(Rigidbody2D playerRb, Vector2 playerPosition)
    {
        base.ApplyKnockbackToTarget(playerRb, playerPosition); // 부모 클래스의 ApplyKnockback 호출

        // 넉백 후 박쥐가 플레이어와 반대 방향으로 이동하도록 설정
        knockbackDirection = (playerPosition - (Vector2)transform.position).normalized * -1; // 반대 방향 설정
        isKnockedBack = true;
        knockbackTime = knockbackDuration; // 넉백 시간을 설정
    }

    protected override void Update()
    {
        if (isKnockedBack)
        {
            // 넉백 후 일정 시간 동안 플레이어와 반대 방향으로 이동
            if (knockbackTime > 0f)
            {
                // 넉백 시간동안 반대 방향으로 이동
                transform.Translate(knockbackDirection * Time.deltaTime * chaseOrFleeSpeed); // 3f는 이동 속도
                knockbackTime -= Time.deltaTime; // 넉백 시간 차감
            }
            else
            {
                // 넉백 시간이 끝났으면 추적 상태로 돌아가도록 설정
                isKnockedBack = false;
                OnChase(); // 추적 상태로 돌아가기
            }
        }
        else
        {
            base.Update();
        }
    }

    public override void AfterFleeState()
    {
        OnChase();
    }
}
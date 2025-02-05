using UnityEngine;

public class Rat : MonsterBase
{
    // 인스펙터에서 할당
    [SerializeField] Collider2D detectCollider;

    // 쥐의 이동 관련 변수들
    private Vector2 fleeDirection; // 도망가는 방향
    [SerializeField] private float fleeDistance = 6f; // 도망갈 거리
    private float fleeDuration = 1f; // 도망가는 시간
    private float fleeTime = 0f; // 도망가고 있는 시간
    [SerializeField] private bool isFleeing = false; // 도망 중인지 확인하는 변수

    // 쥐가 잠시 쉬는 시간
    private float restTime = 3f; // 쉬는 시간
    private float restTimer = 0f; // 쉬는 시간 카운트

    // 도망가는 속도 (fleeDistance와 fleeDuration에 따라 계산)
    private float fleeSpeed;

    protected override void Start()
    {
        base.Start();
        // fleeSpeed 계산: fleeDistance / fleeDuration
        fleeSpeed = fleeDistance / fleeDuration;
        dieClip = AudioClipName.Rat_Die;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 감지 범위 안에 들어오면 도망
            if (!isFleeing)
            {
                // 플레이어와 반대 방향으로 도망가기
                fleeDirection = (transform.position - other.transform.position).normalized;
                SetDirnimaiton(fleeDirection.x, fleeDirection.y);
                isFleeing = true;
                fleeTime = fleeDuration; // 도망가는 시간 설정
                //fleeSpeed = fleeDistance / fleeDuration;
                detectCollider.enabled = false;

                SoundManager.Instance.Play(AudioType.Effect, AudioClipName.Rat_Flee);
            }
        }
    }

    protected override void Update()
    {
        if (monsterStateMachine.CurrentState == monsterStateMachine.dieMonsterState) return;

        if (isFleeing)
        {
            // 도망가는 시간동안 반대 방향으로 이동
            if (fleeTime > 0f)
            {
                transform.Translate(fleeDirection * fleeSpeed * Time.deltaTime); // 도망가는 이동
                fleeTime -= Time.deltaTime;
            }
            else
            {
                // 도망친 후 잠시 쉬기
                isFleeing = false;
                restTimer = restTime; // 쉬는 시간 설정
            }
        }
        else if (restTimer > 0f)
        {
            // 쉬는 시간
            restTimer -= Time.deltaTime;
            if (restTimer <= 0f)
            {
                // 쉬는 시간이 끝나면 다시 플레이어 감지 상태로 돌아가기
                detectCollider.enabled = true;
            }
        }
        else
        {
            // 플레이어가 감지 범위에 다시 들어오면 다시 도망가기 시작
            detectCollider.enabled = true;
        }
    }

    protected override void HandleMonsterHit(Transform attacker)
    {
        //OnIdle();
        //throw new System.NotImplementedException();
    }

    public override void AfterFleeState()
    {
        OnIdle();
    }

    // OnDie() 메소드를 오버라이드하여 추가 로직 구현
    public override void OnDie()
    {
        // 기본 사망 처리
        base.OnDie();

        // Target이 공격자로 등록되어 있다면(즉, 플레이어가 공격했을 경우)
        if (Target != null && Target.CompareTag("Player"))
        {
            // 20% 확률로 감염
            if (Random.Range(0f, 1f) <= 0.2f)
            {

                // 흑사병에 의한 사망 원인을 설정
                PlayerStatus playerStatus = Target.GetComponent<PlayerStatus>();
                playerStatus.SetLastDamageCause(DeathCause.Plague); // 사망 사유 셋팅
                playerStatus.Die();

                //Debug.Log("플레이어가 쥐를 죽였으나, 흑사병에 감염되어 사망했습니다.");
            }
        }
    }
}


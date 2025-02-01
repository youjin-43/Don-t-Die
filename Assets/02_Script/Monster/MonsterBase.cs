using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 디버그용 enum MonsterState
/// </summary>
public enum MonsterState
{
    Idle,
    Flee,
    Chase,
    Attack,
    Die
}

// 몬스터들의 공통 속성 및 동작 정의
public abstract class MonsterBase : MonoBehaviour, IDamageable, IItemDroppable
{
    protected MonsterStateMachine monsterStateMachine;

    // 스테이트 머신 디버그용
    [SerializeField] public MonsterState CurrentState;
    void ChangeState(MonsterState newState) => CurrentState = newState;

    [Header("Monster Data")]
    public MonsterData monsterData;
    [HideInInspector] public Animator MonsterAnimator;

    [SerializeField] protected float moveRange = 3f;
    [SerializeField] protected float CurrentHp;
    [SerializeField] protected int atkDamage;
    [SerializeField] protected float chaseOrFleeSpeed;

    [SerializeField] float knockbackForce;
    [SerializeField] float knockbackDuration;
    Rigidbody2D monsterRigidbody;



    #region OnHitEvent

    // 옵저버 패턴을 이용해서 몬스터가 공격받는 순간 특정 State로 transition하도록 구현

    public delegate void OnHitEventHandler(Transform attacker);
    public event OnHitEventHandler OnHitEvent;
    public Transform Target; // 공격 or 도망 대상 -> 지금 기획으로는 target이 플레이어가 될 수 밖에 없는데 나중에 확장 가능성을 고려하여 타겟을 설정하도록 함 

    protected abstract void HandleMonsterHit(Transform attacker); //abstract로 선언해서 MonsterBase를 상속한 몬스터가 공격받았을때의 행동패턴을 각각 정의하도록 함. 

    // monster.OnHitEvent += HandleMonsterHit; 이런식으로 구독해놓고 OnHit()을 호출하여 HandleMonsterHit 함수 Invoke
    public void OnHit(Transform attacker, int damage)
    {
        Target = attacker;
        OnHitEvent?.Invoke(attacker); // 이벤트 발생
        TakeDamage(damage);
    }

    #endregion

    Transform go;
    protected void SetData()
    {
        MonsterAnimator = GetComponent<Animator>();
        monsterRigidbody = GetComponent<Rigidbody2D>();

        CurrentHp = monsterData.MaxHP;
        atkDamage = (int)monsterData.AttackDamage;
        chaseOrFleeSpeed = monsterData.ChaseOrFleeSpeed;

        knockbackForce = monsterData.KnockbackForce;
        knockbackDuration = monsterData.KnockbackDuration;

        if(transform.childCount >0) transform.GetChild(0)?.GetComponent<MonsterAtkCol>()?.SetAtkDamage(atkDamage);

        monsterStateMachine = new MonsterStateMachine(this);
    }

    protected virtual void Start()
    {
        SetData(); // 몬스터 기본 데이터 셋팅

        monsterStateMachine.Initialize(monsterStateMachine.idleMonsterState); // State 설정 
        CurrentState = MonsterState.Idle; // 디버그용
                                          
        OnHitEvent += HandleMonsterHit; // 몬스터의 OnHitEvent를 구독
    }

    protected virtual void Update()
    {
        monsterStateMachine.Execute();
    }

    /// <summary>
    /// Flee 상태 이후 어떤 State로 넘어갈것인지 
    /// </summary>
    public abstract void AfterFleeState();

    #region OnState

    public void OnIdle()
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.idleMonsterState)
        {
            monsterStateMachine.TransitionTo(monsterStateMachine.idleMonsterState);
            ChangeState(MonsterState.Idle); // 디버그용 셋팅 
        }   
    }

    public void OnFlee()
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.fleeMonsterState)
        {
            monsterStateMachine.TransitionTo(monsterStateMachine.fleeMonsterState);
            ChangeState(MonsterState.Flee); // 디버그용 셋팅 
        }
    }

    public void OnChase(){
        if (monsterStateMachine.CurrentState != monsterStateMachine.chaseMonsterState)
        {
            monsterStateMachine.TransitionTo(monsterStateMachine.chaseMonsterState);
            ChangeState(MonsterState.Chase); // 디버그용 셋팅 
        }   
    }

    public void OnAttack()
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.attackMonsterState)
        {
            monsterStateMachine.TransitionTo(monsterStateMachine.attackMonsterState);
            ChangeState(MonsterState.Attack); // 디버그용 셋팅 
        }
    }

    public void OnDie()
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState)
        {
            monsterStateMachine.TransitionTo(monsterStateMachine.dieMonsterState);
            ChangeState(MonsterState.Die); // 디버그용 셋팅 
        }
    }

    #endregion

    #region Attack

    /// <summary>
    /// 공격 타겟에게 넉백 적용
    /// </summary>
    public virtual void ApplyKnockback(Rigidbody2D playerRb, Vector2 playerPosition)
    {
        float knockbackForce = 5f; // 넉백 세기 // TODO : 이거 따로 변수로 빼는게 좋을것 같음

        // 몬스터 → 플레이어 방향 벡터
        Vector2 knockbackDirection = (playerPosition - (Vector2)transform.position).normalized;

        // 반대 방향으로 넉백 적용
        playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }


    #endregion

    #region Damaged(IDamageable)

    public virtual void TakeDamage(int damage) // TODO : 선택된 도구의 공격력을 받아오도록 
    {
        if(monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState)
        {   
            CurrentHp -= damage;
            TriggerDamagedAnimaiton(); // TODO : 피격 이미지 박쥐 참고해서 좀 수정하면 좋을것 같음
            //DebugController.Log($"{transform.name} took {damage} damage -> Current HP : {CurrentHp} | called in MonsterBase");

            if (CurrentHp <= 0)
            {
                OnDie();
            }
            else ApplyKnockback(); // 넉백 적용
        }    
    }

    // 아직 사용되는곳 없음! 
    public bool IsDead()
    {
        return monsterStateMachine.CurrentState == monsterStateMachine.dieMonsterState;
    }

    private void ApplyKnockback()
    {
        if (monsterRigidbody == null)
        {
            Debug.Log($"{transform.name}에 Rigidbody2D가 컴포넌트가 없습니다 ");
            return; // Rigidbody2D가 없으면 넉백을 적용하지 않음
        }

        //Debug.Log("ApplyKnockback 호출됨 ");
        Vector2 knockbackDirection = (transform.position - Target.position).normalized; // 공격 방향 계산 (몬스터의 위치에서 공격자의 위치를 뺀 방향 벡터)

        // 힘을 추가 (Impulse는 즉각적인 힘)
        monsterRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(ResetVelocity());  // 일정 시간 후에 제어 복원
    }

    private IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(knockbackDuration); // 넉백 지속 시간
        monsterRigidbody.linearVelocity = Vector2.zero; // Rigidbody의 속도를 초기화
    }

    #endregion

    #region IItemDroppable

    public void DropItems()
    {
        foreach (var item in monsterData.DropItems)
        {
            int count = UnityEngine.Random.Range(item.minAmount, item.maxAmount + 1);

            while (count > 0)
            {
                Item go = PoolManager.Instance.InstantiateItem(item.data);

                // 플레이어 반대 방향으로 뿌리도록 
                Vector3 dir = transform.position +
                    (transform.position - GameManager.Instance.GetPlayerPos()
                    + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));

                go.Spread(transform.position, dir, UnityEngine.Random.Range(2.5f, 3f));
                count--;
            }
        }
    }

    #endregion

    #region Animation

    // 4방향인 몬스터가 있고 2방향인 몬스터가 있어서 애니메이션 컨트롤 로직이 달라 몬스터별로 함수를 override 해서 사옹하도록 함
 
    string isMoving = "IsMoving";
    string attack = "Attack";
    string takeDamage = "TakeDamage";
    string die = "Die";


    public void SetIsMovingAnimation(bool value)
    {
        MonsterAnimator.SetBool(isMoving, value);
    }

    // 4방향인건 override 해서 사
    public virtual void SetDirnimaiton(float dir_x, float dir_y)
    {
        if (dir_x < 0) transform.localScale = new Vector3(-1, 1, 1);
        if (dir_x > 0) transform.localScale = new Vector3(1, 1, 1);
    }

    public void TriggerAttackAnimaiton()
    {
        MonsterAnimator.SetTrigger(attack);
    }

    public void TriggerDamagedAnimaiton()
    {
        MonsterAnimator.SetTrigger(takeDamage);
    }

    public void SetDieAnimation() // DieMonsterState에서 호출 
    {
        Debug.Log("SetDieAnimation 실행 ");
        MonsterAnimator.SetTrigger(die);
    }
    #endregion

    #region Utility

    /// <summary>
    /// pos의 바이옴 정보를 리턴 
    /// </summary>
    public BiomeType GetBiomeInfo(Vector3 pos)
    {
        return EnvironmentManager.Instance.biomeMap.GetTileBiome(new Vector2Int((int)pos.x, (int)pos.y));
    }

    /// <summary>
    /// 현재 위치 기준으로 무작위 위치 계산
    /// </summary>
    public Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(-moveRange, moveRange);
        float randomY = Random.Range(-moveRange, moveRange);

        Vector3 currentPos = transform.position;
        return new Vector3(currentPos.x + randomX, currentPos.y + randomY, currentPos.z);
    }

    #endregion
}
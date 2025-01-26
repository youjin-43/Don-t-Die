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
    [HideInInspector] public Animator MonsterAnimator; //StateMachine에서 호출되는 속성들 
    public BiomeType BiomeType; 
    public float MoveSpeed;
    public float MoveInterval;
    public float ChaseSpeed;

    [SerializeField] protected MonsterData monsterData; 
    [SerializeField] protected float moveRange = 3f;
    [SerializeField] protected float CurrentHp;

    [SerializeField] float knockbackForce;
    [SerializeField] float knockbackDuration;
    Rigidbody2D monsterRigidbody;


    // 옵저버 패턴을 이용해서 몬스터가 공격받는 순간 특정 State로 transition하도록 구현
    #region OnHitEvent
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

    protected void SetData()
    {
        MonsterAnimator = GetComponent<Animator>();
        monsterRigidbody = GetComponent<Rigidbody2D>();
        BiomeType = monsterData.MyBiomeType;
        CurrentHp = monsterData.MaxHP;
        MoveSpeed = monsterData.MoveSpeed;
        MoveInterval = monsterData.MoveInterval;
        ChaseSpeed = monsterData.ChaseSpeed;
        knockbackForce = monsterData.KnockbackForce;
        knockbackDuration = monsterData.KnockbackDuration;

        monsterStateMachine = new MonsterStateMachine(this);
    }

    private void Start()
    {
        SetData(); // 몬스터 기본 데이터 셋팅
        monsterStateMachine.Initialize(monsterStateMachine.idleMonsterState);
        CurrentState = MonsterState.Idle; // 디버그용
                                          
        OnHitEvent += HandleMonsterHit; // 몬스터의 OnHitEvent를 구독
    }

    private void Update()
    {
        monsterStateMachine.Execute();
    }

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

    #region IDamageable
    public virtual void TakeDamage(int damage) // TODO : 선택된 도구의 공격력을 받아오도록 
    {
        if(monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState)
        {
            
            MonsterAnimator.SetTrigger("TakeDamage"); // TODO : 피격 이미지 박쥐 참고해서 좀 수정하면 좋을것 같음
                                      
            ApplyKnockback(); // 넉백 적용

            CurrentHp -= damage;
            Debug.Log($"{transform.name} took {damage} damage -> Current HP : {CurrentHp}");
            if (CurrentHp <= 0) OnDie();
        }    
    }

    private void ApplyKnockback()
    {
        if (monsterRigidbody == null) return; // Rigidbody2D가 없으면 넉백을 적용하지 않음

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
        Debug.Log("DropItems 호출됨 "); // TODO : 아 왜 드랍아이템 안돼!!
        foreach (var item in monsterData.DropItems)
        {
            int count = UnityEngine.Random.Range(item.minAmount, item.maxAmount + 1);

            while (count > 0)
            {
                Item go = PoolManager.Instance.InstantiateItem(item.data);

                //플레이어 방향 반대쪽으로 흩뿌려지도록 
                Vector3 dir = transform.position * 2 + GameManager.Instance.GetPlayerPos() + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                go.Spread(transform.position, dir, UnityEngine.Random.Range(2f, 2.5f));
                count--;
            }
        }
    }
    #endregion

    //#region Flee State

    //protected virtual void FleeBehavior(Transform playerTransform)
    //{
    //    Vector3 direction = (transform.position - playerTransform.position).normalized;

    //    // 애니메이션 설정
    //    monsterAnimator.SetBool("IsMoving", true);
    //    monsterAnimator.SetFloat("dirX", direction.x);
    //    monsterAnimator.SetFloat("dirY", direction.y);

    //    // 플레이어 반대 방향으로 이동
    //    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, monsterData.MoveSpeed * Time.deltaTime);

    //    if (Vector3.Distance(transform.position, playerTransform.position) > moveRange * 2)
    //    {
    //        ChangeState(MonsterState.Idle);
    //    }
    //}

    //#endregion

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
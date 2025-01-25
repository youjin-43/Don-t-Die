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

    #region OnHitEvent
    public delegate void OnHitEventHandler(Transform attacker);
    public event OnHitEventHandler OnHitEvent;
    public Transform Target; // 공격 or 도망 대상

    public void OnHit(Transform attacker)
    {
        Target = attacker;
        OnHitEvent?.Invoke(attacker); // 공격 이벤트 발생
    }

    protected abstract void HandleMonsterHit(Transform attacker);
    #endregion

    protected void SetData()
    {
        MonsterAnimator = GetComponent<Animator>();
        BiomeType = monsterData.MyBiomeType;
        CurrentHp = monsterData.MaxHP;
        MoveSpeed = monsterData.MoveSpeed;
        MoveInterval = monsterData.MoveInterval;
        ChaseSpeed = monsterData.ChaseSpeed;

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
        monsterStateMachine.TransitionTo(monsterStateMachine.idleMonsterState);
        ChangeState(MonsterState.Idle);
    }
    public void OnFlee()
    {
        monsterStateMachine.TransitionTo(monsterStateMachine.fleeMonsterState);
        ChangeState(MonsterState.Flee);
    }
    public void OnChase(){
        monsterStateMachine.TransitionTo(monsterStateMachine.chaseMonsterState);
        ChangeState(MonsterState.Chase);
    }

    public void OnAttack()
    {
        monsterStateMachine.TransitionTo(monsterStateMachine.attackMonsterState);
        ChangeState(MonsterState.Attack);
    }
    public void OnDie()
    {
        monsterStateMachine.TransitionTo(monsterStateMachine.dieMonsterState);
        ChangeState(MonsterState.Die);
    }
    #endregion

    #region IDamageable
    public virtual void TakeDamage(int damage) //TODO : 선택된 도구의 공경력을 받아오도록 
    {
        DebugController.Log($"{transform.name} took {damage} damage -> Current HP : {CurrentHp}");
        OnHit(transform); // 이벤트 발생 
        MonsterAnimator.SetTrigger("TakeDamage"); //TODO : 피격 이미지 박쥐 참고해서 좀 수정하면 좋을것 같음 
        CurrentHp -= damage;

        if (CurrentHp <= 0) OnDie();
        
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
using UnityEngine;

// 몬스터들의 공통 속성 및 동작 정의
public abstract class MonsterBase : MonoBehaviour
{
    [Header("Monster Base Attributes")]
    public MonsterData monsterData;

    [Space(10f)]
    public string MonsterName;
    public float CurrentHp;
    public float MoveSpeed;
    public float AttackDamage;
    //TODO : dropItems 추가

    MonsterStateMachine monsterStateMachine;
    private void Awake()
    {
        //monsterStateMachine = new MonsterStateMachin;
    }

    private void Start()
    {
        // 초기 상태 설정 (Idle)
        //monsterStateMachine.ChangeState(monsterStateMachin, this);
    }

    private void Update()
    {
        // 현재 상태 업데이트
        monsterStateMachine.UpdateState(this);
    }

    public abstract void Move(Vector3 targetPosition);

    public abstract void Attack(GameObject target); // 공격 로직 (구체적 동작은 각 몬스터가 구현)

    public virtual void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0) Die();
    }

    protected virtual void Die()
    {
        Debug.Log($"{MonsterName} has died!");
        Destroy(gameObject);
    }

}

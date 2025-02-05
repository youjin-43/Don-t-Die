using UnityEngine;

public class MonsterAtkCol : MonoBehaviour
{
    [SerializeField] Transform attakTarget; // Attack 애니메이션 진입시 셋팅 후 끝날때 clear

    public int atkDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (attakTarget == null)
        {
            attakTarget = collision.transform;
            IDamageable damageable = attakTarget.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(atkDamage);
                attakTarget.GetComponent<PlayerStatus>().SetLastDamageCause(DeathCause.SkelAttack); // 사망 사유 셋팅 
                transform.parent.GetComponent<MonsterBase>().ApplyKnockbackToTarget(attakTarget.GetComponent<Rigidbody2D>(), attakTarget.position);

                Debug.Log($"몬스터가 {attakTarget.name}에게 {atkDamage} 데미지를 입혔습니다.");

                if (damageable.IsDead())
                {
                    Debug.Log($"{attakTarget.name}이 죽었으므로 공격을 멈춥니다.");

                    attakTarget = null; // 타겟 초기화 
                    transform.parent.GetComponent<MonsterBase>().OnIdle();  // 몬스터 공격을 멈추고 Idle 상태로 전환
                }
            }
        }
    }

    public void ClearTarget()
    {
        attakTarget = null;
    }

    public void SetAtkDamage(float damage)
    {
        atkDamage = (int)damage;
    }
}

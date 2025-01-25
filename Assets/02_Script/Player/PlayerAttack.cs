using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerAnimator playerAnimator;
    [SerializeField] Collider2D AttackCollider; // 인스펙터에서 할당 
    [SerializeField] MonsterBase targetMonster;


    private void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterBase monsterBase = collision.GetComponent<MonsterBase>();
        if (monsterBase != null)
        {
            monsterBase.TakeDamage(10); //TODO : 현재 착용중인 도구의 데이터를 가져오도록
            monsterBase.OnHit(transform); // 이벤트 발생 
        }
    }

    public void Attack()
    {
        playerAnimator.TriggerAttackAnimation();
    }
}

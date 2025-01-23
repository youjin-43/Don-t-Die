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
        if (collision.CompareTag("Monster"))
        {
            collision.GetComponent<MonsterBase>().TakeDamage(10);
        }
    }

    public void Attack()
    {
        playerAnimator.TriggerAttackAnimation();
        //if (targetMonster != null)
        //{
        //    targetMonster.TakeDamage(10);
        //    targetMonster = null;
        //}
    }
}

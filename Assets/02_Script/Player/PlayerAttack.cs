using UnityEngine;

/// <summary>
/// 플레이어 자식 오브젝트 중 Attack Collider에 붙어있음 
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    public PlayerAnimator playerAnimator; //PlayerMoveManager 에서 할당 받도록 함 

    //private void Start()
    //{
        //playerAnimator = transform.parent.GetComponent<PlayerAnimator>();
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterBase monsterBase = collision.GetComponent<MonsterBase>();
        if (monsterBase != null)
        {
            //monsterBase.TakeDamage(10); //TODO : 데미지는 현재 착용중인 도구의 데이터를 가져오도록
            monsterBase.OnHit(transform.parent,10); // OnHit 이벤트 발생 -> attacker로 플레이어의 transfrom 전달
        }

    }

    public void Attack()
    {
        playerAnimator.TriggerAttackAnimation();
    }
}

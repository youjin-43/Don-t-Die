using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator playerAnimator;
    SpriteRenderer spriteRenderer;
    [SerializeField] Transform AttackCollider; //인스펙터에서 할당 

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetIdleAnimaion()
    {
        playerAnimator.SetFloat("Walk", 0);
    }

    public void SetWalkAnimaion()
    {
        playerAnimator.SetFloat("Walk", 1);
    }

    public void TriggerAttackAnimation()
    {
        playerAnimator.SetTrigger("Attack");
    }

    public void LookRight()
    {
        //spriteRenderer.flipX = true;
        transform.localScale = new Vector3(-1, 1, 1);
    }

    public void LookLeft()
    {
        //spriteRenderer.flipX = false;
        transform.localScale = new Vector3(1, 1, 1);
    }


}

using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator playerAnimator;
    SpriteRenderer spriteRenderer;

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
        spriteRenderer.flipX = true;
    }

    public void LookLeft()
    {
        spriteRenderer.flipX = false;
    }
}

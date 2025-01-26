using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator playerAnimator;
    [SerializeField] Transform AttackCollider; //인스펙터에서 할당 

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
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

    public void LookLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }

    public void LookRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }


}

using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator playerAnimator;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    #region Move

    public void SetIdleAnimaion()
    {
        playerAnimator.SetFloat("Walk", 0);
    }

    public void SetWalkAnimaion()
    {
        playerAnimator.SetFloat("Walk", 1);
    }

    #endregion


    #region GetItem

    public void TriggerGetItemAnimation()
    {
        playerAnimator.SetTrigger("GetItem");
    }

    #endregion

    #region Craft

    public void TriggerDoingAnimation()
    {
        playerAnimator.SetTrigger("Doing");
    }
    #endregion

    #region Use Tool

    public void SetUseToolAnimation_True() => playerAnimator.SetBool("UseTool", true);
    public void SetUseToolAnimation_False() => playerAnimator.SetBool("UseTool", false);

    // Sword
    public void SetSwordAnimation_True() => playerAnimator.SetBool("Tool_Sword", true);
    public void SetSwordAnimation_False() => playerAnimator.SetBool("Tool_Sword", false);

    // Axe
    public void SetAxeAnimation_True() => playerAnimator.SetBool("Tool_Axe", true);
    public void SetAxeAnimation_False() => playerAnimator.SetBool("Tool_Axe", false);

    // PickAxe
    public void SetPickAxeAnimation_True() => playerAnimator.SetBool("Tool_PickAxe", true);
    public void SetPickAxeAnimation_False() => playerAnimator.SetBool("Tool_PickAxe", false);

    #endregion

    #region 좌우 방향 전환

    public void LookLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }

    public void LookRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    #endregion

    #region Die

    public void TriggerTakeDamageAnimation()
    {
        playerAnimator.SetTrigger("TakeDamage");
    }

    public void TriggerDieAnimation()
    {
        playerAnimator.SetTrigger("Die");
    }

    #endregion
}

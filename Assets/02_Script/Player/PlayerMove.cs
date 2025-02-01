using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [HideInInspector] public PlayerAnimator playerAnimator; //PlayerMoveManager 에서 할당 받도록 함 
    public float moveSpeed = 4f; //이동 속도 

    Vector2 dir = new Vector2(0f, 0f);

    public void HandleMovement()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");
        //DebugController.Log(dir.x + "," + dir.y);

        transform.position += new Vector3(dir.x, dir.y, 0).normalized * moveSpeed * Time.deltaTime;
        HandleMoveAnimation();
    }

    void HandleMoveAnimation()
    {
        if (dir.x == 0 && dir.y == 0) playerAnimator.SetIdleAnimaion();
        else playerAnimator.SetWalkAnimaion();

        //좌우 바라보게 하기 
        if (dir.x < 0) playerAnimator.LookLeft();
        if (dir.x > 0) playerAnimator.LookRight();
    }
}

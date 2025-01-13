using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Transform plyerTransform;
    Animator animator;
    public float Speed = 0.015f;
    Vector2 dir = new Vector2(0f,0f);


    private void Start()
    {
        plyerTransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    void HandleMovement()
    {
        dir.x = Input.GetAxisRaw("Horizontal") ;
        dir.y = Input.GetAxisRaw("Vertical");
        //Debug.Log(dir.x + "," + dir.y);

        plyerTransform.position += new Vector3(dir.x, dir.y, 0).normalized*Speed;
    }

    void HandleAnimation()
    {
        if (dir.x == 0 && dir.y == 0)
        {
            animator.SetBool("IsMove", false);
        }
        else
        {
            animator.SetBool("IsMove", true);
            // 이동했던 방향 기억 
            animator.SetFloat("PreDx", dir.x);
            animator.SetFloat("PreDy", dir.y);
        }

        animator.SetFloat("Dx", dir.x);
        animator.SetFloat("Dy", dir.y);
        
    }
}

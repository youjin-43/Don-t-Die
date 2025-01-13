using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Transform plyerTransform;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public float Speed = 0.01f;
    float vx = 0f;
    float vy = 0f;


    private void Start()
    {
        plyerTransform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    void HandleMovement()
    {
        vx = Input.GetAxisRaw("Horizontal") * Speed;
        vy = Input.GetAxisRaw("Vertical") * Speed;

        //좌우 바라보게 하기 
        if (vx < 0) spriteRenderer.flipX = true;
        if (vx > 0) spriteRenderer.flipX = false;

        plyerTransform.position += new Vector3(vx, vy, 0);
    }

    void HandleAnimation()
    {
        if (vx == 0 && vy == 0)
        {
            //animator.SetTrigger("Idle");
            animator.SetBool("Walk",false);
        }
        else animator.SetBool("Walk",true); 
    }


}

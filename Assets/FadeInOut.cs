using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        animator.SetTrigger("In");
    }

    public void FadeOut()
    {
        animator.SetTrigger("Out");
    }
}

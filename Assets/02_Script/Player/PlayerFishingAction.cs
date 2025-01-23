using UnityEngine;

public class PlayerFishingAction : MonoBehaviour
{
    bool isFishing;
    Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        // if (도구 낀 상태로)
        if (Input.GetKeyDown(KeyCode.Equals))
        {

        }
    }
}

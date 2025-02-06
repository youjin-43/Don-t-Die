using UnityEngine;

public class LookCamera : MonoBehaviour
{
    public RectTransform canvas;

    void Update()
    {
        if(transform.localScale.x < 0)
        {
            canvas.localScale = new Vector3(-2f, 0.4f, 1);
        }
        else
        {
            canvas.localScale = new Vector3(2f, 0.4f, 1);
        }
    }
}

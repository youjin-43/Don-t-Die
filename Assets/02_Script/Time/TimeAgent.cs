using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    protected int timer;

    public int Timer
    {
        get { return timer; } 
        set { timer = Mathf.Max(0, value); }
    }

    public virtual void UpdateTimer()
    {

    }
}

using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    protected int timer;

    public int Timer
    {
        get { return timer; } 
        set { timer = Mathf.Max(0, value); }
    }

    protected virtual void OnEnable()
    {
        if (EnvironmentManager.Instance.TryGetComponent(out TimeController timeController))
        {
            timeController.Subscribe(this);
        }
    }

    private void OnDisable()
    {
        if (isQuitting) { return; }
        //if (EnvironmentManager.Instance.TryGetComponent(out TimeController timeController))
        //{
        //    timeController.Unsubscribe(this);
        //}
    }

    // 게임뷰에서 씬뷰로 넘어올 때 에러 방지
    bool isQuitting = false;
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    public virtual void UpdateTimer()
    {

    }
}

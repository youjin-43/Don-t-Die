using System;
using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    public Action onTimeTick;
    TimeController timeController;

    protected void Init()
    {
        timeController = transform.parent.parent.GetComponent<TimeController>();
        timeController.Subscribe(this);
    }

    private void OnDestroy()
    {
        if (timeController != null)
            timeController.Unsubscribe(this);
    }

    public void InvokeOnTimeTick()
    {
        DebugController.Log("InvokeOnTimeTick");
        onTimeTick?.Invoke();
    }

    void Start()
    {
        Init();
    }

    
    void Update()
    {
        
    }
}

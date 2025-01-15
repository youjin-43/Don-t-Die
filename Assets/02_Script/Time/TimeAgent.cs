using System;
using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    public Action onTimeTick;
    TimeController timeController;

    void Init()
    {
        timeController.Subscribe(this);
    }

    private void OnDestroy()
    {
        if (timeController != null)
            timeController.Unsubscribe(this);
    }

    public void InvokeOnTimeTick()
    {
        onTimeTick?.Invoke();
    }

    void Start()
    {
        timeController = transform.parent.parent.GetComponent<TimeController>();
        Init();
    }

    
    void Update()
    {
        
    }
}

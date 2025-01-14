using System;
using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    public Action onTimeTick;

    void Init()
    {
        // timeController.Subscribe();
    }

    private void OnDestroy()
    {
        // timeController.Unsubscribe();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}

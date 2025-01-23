using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region SINGLETON
    private static UIManager instance;
    public  static UIManager Instance
    {
        get
        {
            
            return instance;
        }
    }

    void SingletonInitialize()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    [SerializeField] GameObject _craftUI;
    [SerializeField] GameObject _statusUI;

    private void Awake()
    {
        SingletonInitialize();
        
        _craftUI.SetActive(true);
        _statusUI.SetActive(true);
    }

}

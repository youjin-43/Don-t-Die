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

    // 에디터 상에서 조합 UI가 켜있는게 보기 싫어서...
    [SerializeField] GameObject _craftUI;

    private void Awake()
    {
        SingletonInitialize();
        
        
        _craftUI.SetActive(true);
    }

    public void Update()
    {
        //Vector3 point = Camera.main.ScreenToWorldPoint
        //    (
        //        new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Input.mousePosition.z)
        //    );

        //Debug.Log(point.ToString());
    }
}

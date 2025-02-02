using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] GameObject _boxUI;

    private void Awake()
    {
        SingletonInitialize();
        
        _craftUI.SetActive(true);
        _statusUI.SetActive(true);
        _boxUI.SetActive(true);
    }

    public bool IsUIClick()
    {
        return EventSystem.current.IsPointerOverGameObject();

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}

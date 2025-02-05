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
            //DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    [SerializeField] GameObject _craftUI;
    [SerializeField] GameObject _statusUI;
    [SerializeField] GameObject _boxUI;

    [SerializeField] DeathUI _deathUI;

    private void Awake()
    {
        SingletonInitialize();
        
        _craftUI.SetActive(true);
        _statusUI.SetActive(true);
        _boxUI.SetActive(true);
        _deathUI.gameObject.SetActive(false);
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

    public void Death(DeathCause cause)
    {
        string code = ((int)(cause)).ToString("D3");

        string Content = DataManager.Instance.AchievementData[code].Content;

        _deathUI.gameObject.SetActive(true);
        _deathUI.SetContentText(Content);

        AchievementManager.Instance.SetAchievement(code);
    }
}

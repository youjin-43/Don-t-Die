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
    [SerializeField] EscUI   _escUI;
    [SerializeField] public SettingUI _settingUI;

    private void Awake()
    {
        SingletonInitialize();
        
        _craftUI.SetActive(true);
        _statusUI.SetActive(true);
        _boxUI.SetActive(true);
        _deathUI.gameObject.SetActive(true);
        _escUI.gameObject.SetActive(true);
        _settingUI.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // esc창이 켜져있고, 설정창 까지 켜져있으면 설정창만 닫아야 함
            if (_settingUI.IsOpened() == true)
            {
                _settingUI.ToggleSettingUI();
            }
            else
            {
                _escUI.ToggleEscUI();
            }
        }
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

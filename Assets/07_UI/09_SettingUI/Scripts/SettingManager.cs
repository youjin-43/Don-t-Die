using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    #region SINGLETON
    private static SettingManager instance;
    public static  SettingManager Instance
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

    [SerializeField] Slider _volumeSlider;

    void Awake()
    {
        SingletonInitialize();

        _volumeSlider = transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Slider>();
    }

    private void Start()
    {
        ToggleSettingUI();
    }

    public void ToggleSettingUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SetVolume()
    {
        Debug.Log("볼륨 조절중");
    }
}

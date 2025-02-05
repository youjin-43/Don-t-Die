using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] Slider _volumeSlider;

    public static float _value = 0.1f;

    void Awake()
    {
        _volumeSlider = transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Slider>();
    }

    private void Start()
    {
        ToggleSettingUI();
    }

    private void Update()
    {
        _volumeSlider.value = _value;    
    }

    public void ToggleSettingUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SetVolume()
    {
        SoundManager.Instance.SetAllVolume(_volumeSlider.value);

        _value = _volumeSlider.value;
    }

    public bool IsOpened()
    {
        return gameObject.activeSelf;
    }
}

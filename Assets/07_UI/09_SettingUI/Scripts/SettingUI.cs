using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] Slider _volumeSlider;

    private float _maxVolume;

    void Awake()
    {
        _volumeSlider = transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Slider>();
    }

    private void Start()
    {
        ToggleSettingUI();

        _volumeSlider.maxValue = SoundManager.Instance.GetVolume();
        _volumeSlider.value = _volumeSlider.maxValue / 2f;
    }

    public void ToggleSettingUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SetVolume()
    {
        SoundManager.Instance.SetAllVolume(_volumeSlider.value * 1.2f);
    }

    public bool IsOpened()
    {
        return gameObject.activeSelf;
    }
}

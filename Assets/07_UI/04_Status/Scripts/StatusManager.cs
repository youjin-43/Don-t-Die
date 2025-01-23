using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    #region SINGLETON
    private static StatusManager instance;
    public  static StatusManager Instance
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

    private Image _healthGauge;
    private Image _hungryGauge;
    private Image _thirstyGauge;
    private Image _temperatureGauge;

    private float _maxHealthPoint;
    private float _maxHungryPoint;
    private float _maxThirstyPoint;
    private float _maxTemperture;

    void Awake()
    {
        SingletonInitialize();

        _healthGauge      = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _hungryGauge      = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        _thirstyGauge     = transform.GetChild(2).GetChild(0).GetComponent<Image>();
        _temperatureGauge = transform.GetChild(3).GetChild(0).GetComponent<Image>();
    }

    public void SetHealth(float currentHealth)
    {
        _healthGauge.fillAmount = currentHealth / _maxHealthPoint;
    }
    
    public void SetHungry(float currentHungry)
    {
        _hungryGauge.fillAmount = currentHungry / _maxHungryPoint;
    }

    public void SetThirsty(float currentThirsty)
    {
        _thirstyGauge.fillAmount = currentThirsty / _maxThirstyPoint;
    }

    [Range(0f, 1f)]
    public float a = 1f;

    [Range(-10f, 10f)]
    public float b = 1f;
    Color red  = Color.red;
    Color blue = Color.blue;

    public Gradient grad;

    public void SetTemperature(float currentTemperature)
    {
        //_temperatureGauge.fillAmount = Mathf.Clamp(currentTemperature / _maxTemperture, 0.3f, 0.8f);
        _temperatureGauge.fillAmount = Mathf.Clamp(currentTemperature, 0.3f, 0.9f);
        _temperatureGauge.color = grad.Evaluate(_temperatureGauge.fillAmount);
    }

    private void Update()
    {
        SetTemperature(a);
    }
}

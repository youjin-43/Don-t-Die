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

    private float _maxHealthPoint  = 150f;
    private float _maxHungryPoint  = 150f;
    private float _maxThirstyPoint = 150f;
    private float _maxTemperture;

    private float _currentHealthPoint  = 75f;
    private float _currentHungryPoint  = 75f;
    private float _currentThirstyPoint = 75f;
    private float _currentTemperture;

    public float CurrentHungryPoint { get { return _currentHungryPoint; } }
    public float CurrentThirstyPoint { get { return _currentThirstyPoint; } }

    [Range(0f, 1f)]
    public float Temperature = 1f;

    public Gradient Gradient;

    void Awake()
    {
        SingletonInitialize();

        _healthGauge      = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _hungryGauge      = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        _thirstyGauge     = transform.GetChild(2).GetChild(0).GetComponent<Image>();
        _temperatureGauge = transform.GetChild(3).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        // 디버그용
        SetTemperature(Temperature);
    }

    public void AddHealth(float healthPoint)
    {
        _currentHealthPoint = Mathf.Clamp(_currentHealthPoint + healthPoint, 0f, _maxHealthPoint);

        _healthGauge.fillAmount = _currentHealthPoint / _maxHealthPoint;
    }
    
    public void AddHungry(float hungryPoint)
    {
        _currentHungryPoint = Mathf.Clamp(_currentHungryPoint + hungryPoint, 0f, _maxHungryPoint);
        
        _hungryGauge.fillAmount = _currentHungryPoint / _maxHungryPoint;
    }

    public void AddThirsty(float thirstyPoint)
    {
        _currentThirstyPoint = Mathf.Clamp(_currentThirstyPoint + thirstyPoint, 0f, _maxThirstyPoint);
        
        _thirstyGauge.fillAmount = _currentThirstyPoint / _maxThirstyPoint;
    }

    public void SetTemperature(float currentTemperature)
    {
        _temperatureGauge.fillAmount = Mathf.Clamp(currentTemperature, 0.3f, 0.9f);
        _temperatureGauge.color = Gradient.Evaluate(_temperatureGauge.fillAmount);
    }
}

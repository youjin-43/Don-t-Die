using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private float _maxHealthPoint = 150f;
    private float _maxHungryPoint = 150f;
    private float _maxThirstyPoint = 150f;
    private float _maxTemperture;

    [SerializeField] private float _currentHealthPoint;
    //[SerializeField] private float _currentHungryPoint;
    //[SerializeField] private float _currentThirstyPoint;
    //[SerializeField] private float _currentTemperture;

    private void Start()
    {
        _currentHealthPoint = _maxHealthPoint;
    }

    public void LoseHP(int amount) {
        _currentHealthPoint -= amount;
        if (_currentHealthPoint <= 0) GetComponent<PlayerMoveManager>().Die();
    }

    public void GainHP(int amount)
    {
        _currentHealthPoint += amount;
    }
}

using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamageable
{
    [Header("Equipment")]
    public HeadItemData equippedHeadItem;
    public ChestItemData equippedChestItem;

    [Header("SurvivalStats")]
    private float _maxHealthPoint = 150f;
    private float _maxHungryPoint = 150f;
    private float _maxThirstyPoint = 150f;
    private float _maxTemperture;

    [SerializeField] private float _currentHealthPoint;
    private bool isDead = false; // Player의 사망 여부를 명확하게 PlayerStatus에서 관리

    private void Start()
    {
        _currentHealthPoint = _maxHealthPoint;
    }

    #region IDamageable

    public void TakeDamage(int damage)
    {
        // TODO : 현재 방어구 기반으로 수치 조정 후 전달 
        LoseHP(damage);
    }

    public bool IsDead()
    {
        return isDead;
    }

    #endregion

    public void LoseHP(int amount)
    {
        _currentHealthPoint -= amount;
        if (_currentHealthPoint <= 0 && !isDead)
        {
            isDead = true;
            GetComponent<PlayerMoveManager>().Die(); // PlayerMoveManager의 Die() 호출
        }
    }

    public void GainHP(int amount)
    {
        if (!isDead)  // 죽은 상태에서는 회복 불가
        {
            _currentHealthPoint = Mathf.Min(_currentHealthPoint + amount, _maxHealthPoint);
        }
    }
}

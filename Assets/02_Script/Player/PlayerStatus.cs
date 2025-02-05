using System;
using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamageable
{
    [Header("Equipment")]
    [SerializeField] HeadItemData equippedHeadItem;
    [SerializeField] ChestItemData equippedChestItem;

    [Space(10f)]
    [Header("SurvivalStats")]
    public float _maxHealthPoint = 150f;
    public float _maxHungryPoint = 150f;
    public float _maxThirstyPoint = 150f;
    public float _maxTemperture;

    public float _currentHealthPoint = 75f;
    public float _currentHungryPoint = 75f;
    public float _currentThirstyPoint = 75f;
    public float _currentTemperture;

    float timeScale = 180f;
    float statusDebuffTimer = 0f;

    Coroutine darknessDamageRoutine;

    public float CurrentHungryPoint { get { return _currentHungryPoint; } }
    public float CurrentThirstyPoint { get { return _currentThirstyPoint; } }

    private bool isDead = false; // Player의 사망 여부를 명확하게 PlayerStatus에서 관리
    public DeathCause lastDamageCause = DeathCause.None; // 죽음 사유 

    [HideInInspector] public PlayerAnimator playerAnimator; //PlayerMoveManager 에서 할당 받도록 함 
    Rigidbody2D playerRigidbody;

    #region 장비 변경 이벤트 

    private void OnDestroy()
    {
        // 이벤트 해제 (메모리 누수 방지)
        EquipmentManager.Instance.OnEquipChanged -= HandleEquipChanged;
        EnvironmentManager.Instance.Time.OnNightStart -= StartDarknessDamage;
        EnvironmentManager.Instance.Time.OnNightEnd -= StopDarknessDamage;
    }

    /// <summary>
    /// 장비가 변경될 때 호출되는 함수
    /// </summary>
    private void HandleEquipChanged(ItemData itemData, EquipmentSlot slot)
    {
        if (slot == EquipmentSlot.Head) equippedHeadItem = EquipmentManager.Instance.GetCurrentHead();
        if (slot == EquipmentSlot.Chest) equippedChestItem = EquipmentManager.Instance.GetCurrentChest();

    }

    #endregion

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        // 장비 변경 이벤트 구독
        EquipmentManager.Instance.OnEquipChanged += HandleEquipChanged;
        EnvironmentManager.Instance.Time.OnNightStart += StartDarknessDamage;
        EnvironmentManager.Instance.Time.OnNightEnd += StopDarknessDamage;
        _currentHealthPoint = _maxHealthPoint;
        StatusUIManager.Instance.UpdateHealthUI();
        StatusUIManager.Instance.playerStatus = this;
    }

    private void Update()
    {
        statusDebuffTimer += Time.deltaTime * timeScale;

        if (statusDebuffTimer > 60f)
        {
            statusDebuffTimer = 0f;

            if (CurrentHungryPoint <= 0)
            {
                lastDamageCause = DeathCause.Starvation;
                LoseHP(0.4f);
            }

            if (CurrentThirstyPoint <= 0)
            {
                lastDamageCause = DeathCause.Dehydration;
                LoseHP(0.4f);
            }

            LoseHungry(0.05f);
            LoseThirsty(0.05f);
        }
    }


    public void SetLastDamageCause(DeathCause cause)
    {
        lastDamageCause = cause;
    }

    public void Die()
    {
        Debug.Log($"플레이어가 {lastDamageCause} 원인으로 사망!");
        playerAnimator.TriggerDieAnimation();
        // TODO :  UI 사망 메시지 표시


        UIManager.Instance.Death(lastDamageCause.ToString());

    }
    #region IDamageable

    public void TakeDamage(int damage)
    {
        // 피격 애니메이션 재생 
        playerAnimator.TriggerTakeDamageAnimation();

        StartCoroutine(ResetVelocity());  // 넉백 후 일정 시간 후에 제어 복원

        LoseHP(damage); // TODO : 현재 방어구 기반으로 수치 조정 후 전달
    }

    [SerializeField] float knockbackDuration = 0.2f; // 넉백 지속 시간
    private IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(knockbackDuration); // 넉백 지속 시간
        playerRigidbody.linearVelocity = Vector2.zero; // Rigidbody의 속도를 초기화
    }

    public bool IsDead()
    {
        return isDead;
    }

    #endregion

    #region HP

    // TODO : UI랑 연동!!

    // TODO : clamp로 변경!! 
    //_currentThirstyPoint = Mathf.Clamp(_currentThirstyPoint + thirstyPoint, 0f, _maxThirstyPoint);

    public void LoseHP(float amount)
    {
        _currentHealthPoint = Mathf.Clamp(_currentHealthPoint - amount, 0, _maxHealthPoint);
        if (_currentHealthPoint <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
        StatusUIManager.Instance.UpdateHealthUI();
    }

    public void GainHP(float amount)
    {
        if (!isDead)  // 죽은 상태에서는 회복 불가
        {
            _currentHealthPoint = Mathf.Clamp(_currentHealthPoint + amount, 0, _maxHealthPoint);
            StatusUIManager.Instance.UpdateHealthUI();
        }
    }

    #endregion

    #region Thirsty

    public void LoseThirsty(float amount)
    {
        _currentThirstyPoint = Mathf.Clamp(_currentThirstyPoint - amount, 0, _maxThirstyPoint);
        StatusUIManager.Instance.UpdateThirstyUI();
    }

    public void GainThirsty(float amount)
    {
        if (!isDead)  // 죽은 상태에서는 회복 불가
        {
            _currentThirstyPoint = Mathf.Clamp(_currentThirstyPoint + amount, 0, _maxThirstyPoint);
            StatusUIManager.Instance.UpdateThirstyUI();
        }
    }


    #endregion

    #region Hungry 

    public void LoseHungry(float amount)
    {
        _currentHungryPoint = Mathf.Clamp(_currentHungryPoint - amount, 0, _maxHungryPoint);
        StatusUIManager.Instance.UpdateHungryUI();
    }

    public void GainHungry(float amount)
    {
        if (!isDead)  // 죽은 상태에서는 회복 불가
        {
            _currentHungryPoint = Mathf.Clamp(_currentHungryPoint + amount, 0, _maxHungryPoint);
            StatusUIManager.Instance.UpdateHungryUI();
        }
    }

    #endregion

    #region Night Debuff

    bool isNight;

    void StartDarknessDamage()
    {
        isNight = true;
        if (darknessDamageRoutine == null)
        {
            darknessDamageRoutine = StartCoroutine(DamageOverTime());
        }
    }

    void StopDarknessDamage()
    {
        isNight = false;
        if (darknessDamageRoutine != null)
        {
            StopCoroutine(darknessDamageRoutine);
            darknessDamageRoutine = null;
        }
    }

    IEnumerator DamageOverTime()
    {
        while (isNight)
        {
            if (!Physics2D.OverlapCircle(transform.position, 5f, LayerMask.GetMask("Light")))
            {
                LoseHP(37.5f);
            }
            yield return new WaitForSeconds(3f);
        }
    }

    #endregion

    public void EatItem(EdibleItemData edibleItemData)
    {
        GainHP(edibleItemData.healthValue);
        GainHungry(edibleItemData.hungerValue);
        GainThirsty(edibleItemData.thirstValue);
    }

    public void DrinkWater(float chargePerUse)
    {
        GainThirsty(chargePerUse);
    }
}

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
    private float _maxHealthPoint = 150f;
    private float _maxHungryPoint = 150f;
    private float _maxThirstyPoint = 150f;
    private float _maxTemperture;

    [SerializeField] private float _currentHealthPoint;
    private bool isDead = false; // Player의 사망 여부를 명확하게 PlayerStatus에서 관리

    [HideInInspector] public PlayerAnimator playerAnimator; //PlayerMoveManager 에서 할당 받도록 함 
    Rigidbody2D playerRigidbody;

    #region 장비 변경 이벤트 

    private void OnDestroy()
    {
        // 이벤트 해제 (메모리 누수 방지)
        EquipmentManager.Instance.OnEquipChanged -= HandleEquipChanged;
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
        _currentHealthPoint = _maxHealthPoint;
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

    #endregion
}

using System;
using UnityEngine;

public class DamageableResourceNode : ResourceNode,IDamageable
{
    public static event Action<Transform> OnResourceAttacked; // 자원 위치 + 공격자(플레이어) 위치

    [SerializeField] GameObject hitEffect;
    float maxHealth;
    float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    protected override void Init()
    {
        maxHealth = natureResourceData.MaxHealth;
        if (currentHealth == 0)
        {
            currentHealth = maxHealth;
        }
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        Init();
    }

    public void TakeDamage(int damage)
    {
        if (hitEffect != null)
        {
            hitEffect.GetComponent<Animator>().Play("Hit", -1, 0f);
        }
        currentHealth = Mathf.Max(0, currentHealth - damage);
        DebugController.Log($"Hit {gameObject.name}. Damage : {damage} CurrentHealth : {currentHealth}");

        // 공격 받았을 때 이벤트 호출
        OnResourceAttacked?.Invoke(transform);

        if (currentHealth <= 0) Harvest();

    }

    public bool IsDead()
    {
        throw new System.NotImplementedException();
    }
}

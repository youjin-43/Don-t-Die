using UnityEngine;

public class DamageableResourceNode : ResourceNode,IDamageable
{
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

    public void Hit(int damage)     // 도구로 자원을 캐는 과정
    {
        if (hitEffect != null)
        {
            hitEffect.GetComponent<Animator>().Play("Hit", -1, 0f);
        }
        currentHealth = Mathf.Max(0, currentHealth - damage);
        DebugController.Log($"Hit {gameObject.name}. Damage : {damage} CurrentHealth : {currentHealth}");

        if (currentHealth < float.Epsilon)
        {
            Harvest();
        }
    }

    public void TakeDamage(int damage)
    {
        if (hitEffect != null)
        {
            hitEffect.GetComponent<Animator>().Play("Hit", -1, 0f);
        }
        currentHealth = Mathf.Max(0, currentHealth - damage);
        DebugController.Log($"Hit {gameObject.name}. Damage : {damage} CurrentHealth : {currentHealth}");

        if (currentHealth <= 0) Harvest();

    }

    public bool IsDead()
    {
        throw new System.NotImplementedException();
    }
}

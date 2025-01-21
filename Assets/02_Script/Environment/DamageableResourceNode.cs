using UnityEngine;

public class DamageableResourceNode : ResourceNode
{
    float maxHealth;
    float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    protected override void Init()
    {
        base.Init();
        maxHealth = natureResourceData.MaxHealth;
        if (currentHealth == 0)
        {
            currentHealth = maxHealth;
        }
    }

    private void Start()
    {
        Init();
    }

    public void Hit(int damage)     // 도구로 자원을 캐는 과정
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        DebugController.Log($"Hit {gameObject.name}. Damage : {damage} CurrentHealth : {currentHealth}");

        if (currentHealth < float.Epsilon)
        {
            Harvest();
        }
    }
}

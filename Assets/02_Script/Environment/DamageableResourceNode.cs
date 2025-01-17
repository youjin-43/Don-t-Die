using UnityEngine;

public class DamageableResourceNode : ResourceNode
{
    int maxHealth;
    int currentHealth;
    public int CurrentHealth
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
        DebugController.Log($"Hit {gameObject.name}. Damage : {damage} CurrentHealth : {currentHealth}");
        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (currentHealth == 0)
        {
            Harvest();
        }
    }
}

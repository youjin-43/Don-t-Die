using UnityEngine;

public class DamageableResourceNode : ResourceNode
{
    [SerializeField] int maxHealth;
    int currentHealth;

    protected override void Init()
    {
        base.Init();
        currentHealth = maxHealth;
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

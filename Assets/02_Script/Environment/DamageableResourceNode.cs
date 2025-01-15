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

    public void Hit(int damage)
    {
        DebugController.Log($"Hit {gameObject.name}. Damage : {damage} CurrentHealth : {currentHealth}");
        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (currentHealth == 0)
        {
            Harvest();
        }
    }
}

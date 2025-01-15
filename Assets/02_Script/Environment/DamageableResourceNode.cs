using UnityEngine;

public class DamageableResourceNode : ResourceNode
{
    [SerializeField] int maxHealth;
    int currentHealth;

    private void Init()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        Init();
    }

    public void Hit(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (currentHealth == 0)
        {
            Harvest();
        }
    }
}

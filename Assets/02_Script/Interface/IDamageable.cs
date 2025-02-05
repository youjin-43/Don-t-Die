using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage);
    bool IsDead();  // 죽었는지 확인
}

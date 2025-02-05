using System.Collections;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 10;

    private Rigidbody2D rb;
    private GameObject thrower; // 사과를 던진 객체 (곰)

    /* 곰(Bear)도 IDamageable을 구현하고 있기 때문에, 사과가 생성되자마자 곰의 Collider2D에 반응해서 바로 사라지는 문제를 해결하기 위해
    사과를 던진 오브젝트(곰)를 저장하고, 충돌 시 던진 오브젝트라면 무시 */
    public void Initialize(Vector2 direction, GameObject thrower)
    {
        this.thrower = thrower; // 던진 객체 저장
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;
        StartCoroutine(DestroyAfterTime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 던진 오브젝트(곰)와 충돌하면 무시
        if (collision.gameObject == thrower) return;

        Transform target = collision.transform;
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Debug.Log(collision.name);

            PlayerStatus playerStatus = target.GetComponent<PlayerStatus>();
            if(playerStatus != null) playerStatus.SetLastDamageCause(DeathCause.BearAttack); // 사망 사유 셋팅 
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}

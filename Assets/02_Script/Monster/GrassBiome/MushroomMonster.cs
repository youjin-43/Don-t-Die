using System;
using System.Collections;
using UnityEngine;

public class MushroomMonster : MonoBehaviour, IItemDroppable, IDamageable
{
    public event Action<Vector3> OnMonsterStateEnd; // 도망 후 다시 버섯으로 변신하는 이벤트

    [SerializeField] private MonsterData monsterData; // 인스펙터에서 할당

    Animator monsterAnimator;
    float fleeSpeed;
    float fleeDistance = 10f; // 일정 거리 도망 기준
    private int maxRetryCount = 5; // 최대 5번 방향 재설정

    void Start()
    {
        monsterAnimator = GetComponent<Animator>();
        fleeSpeed = monsterData.ChaseOrFleeSpeed;
        FleeFromPlayer();
    }

    public void FleeFromPlayer()
    {
        Vector3 fleeTarget = GetValidFleePosition();

        if (fleeTarget != Vector3.zero)
        {
            StartCoroutine(MoveToPosition(fleeTarget));
        }
        else
        {
            ReturnToMushroom(); // 안전한 위치를 찾지 못하면 변신
        }
    }

    private Vector3 GetValidFleePosition()
    {
        Vector3 fleeDirection = (transform.position - GameManager.Instance.GetPlayerPos()).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

        int retryCount = 0;
        while (retryCount < maxRetryCount)
        {
            if (IsPathValid(transform.position, fleeTarget)) return fleeTarget; // 바이옴이 GrassBiome이면 반환

            // 새로운 랜덤 방향 선택 
            float randomAngle = UnityEngine.Random.Range(-90f, 90f);
            fleeDirection = (Quaternion.Euler(0, 0, randomAngle) * fleeDirection).normalized; // z 축 기준 회전 + 정규화하여 크기 유지
            fleeTarget = transform.position + fleeDirection * fleeDistance;

            retryCount++;
        }

        return Vector3.zero; // 실패한 경우
    }

    private bool IsPathValid(Vector3 start, Vector3 end)
    {
        int steps = 3; // 경로를 나눌 샘플 개수
        for (int i = 0; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 point = Vector3.Lerp(start, end, t);
            if (!IsGrassBiome(point)) return false; // 하나라도 GrassBiome이 아니면 false 반환
        }
        return true;
    }

    private bool IsGrassBiome(Vector3 position)
    {
        BiomeType biome = EnvironmentManager.Instance.biomeMap.GetTileBiome(new Vector2Int((int)position.x, (int)position.y));
        return biome == BiomeType.GrassBiome;
    }

    private IEnumerator MoveToPosition(Vector3 targetPos)
    {
        SetIsMovingAnimation(true); // 애니메이션 설정 
        SetDirnimaiton(targetPos - transform.position);

        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, fleeSpeed * Time.deltaTime);
            yield return null;
        }

        SetIsMovingAnimation(false);// 애니메이션 설정 

        ReturnToMushroom(); // 도망 후 다시 버섯으로 변신하도록
    }

    private void ReturnToMushroom()
    {
        PoolManager.Instance.Push(gameObject); // 몬스터 제거
        OnMonsterStateEnd?.Invoke(transform.position); // 이벤트 호출하여 버섯으로 변환
    }

    #region 피격당했을 때 

    public void TakeDamage(int damage)
    {
        DropItems();
        Destroy(this); // TODO : 풀매니저로 나중에 변경.. 
    }

    public bool IsDead()
    {
        throw new NotImplementedException();
    }

    public void DropItems()
    {
        foreach (var item in monsterData.DropItems)
        {
            int count = UnityEngine.Random.Range(item.minAmount, item.maxAmount + 1);

            while (count > 0)
            {
                Item go = PoolManager.Instance.InstantiateItem(item.data);

                // 플레이어 반대 방향으로 뿌리도록 
                Vector3 dir = transform.position +
                    (transform.position - GameManager.Instance.GetPlayerPos()
                    + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));

                go.Spread(transform.position, dir, UnityEngine.Random.Range(2.5f, 3f));
                count--;
            }
        }
    }

    #endregion

    #region 애니메이션

    void SetIsMovingAnimation(bool value)
    {
        monsterAnimator.SetBool("isMoving", value);
    }

    void SetDirnimaiton(Vector3 vector)
    {
        monsterAnimator.SetFloat("dirX", vector.x);
        monsterAnimator.SetFloat("dirY", vector.y);
    }
    #endregion
}

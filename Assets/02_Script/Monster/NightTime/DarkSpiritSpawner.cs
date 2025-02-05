using System.Collections;
using UnityEngine;

/// <summary>
/// 모닥불(campFire)가 컴포넌트로 가지고 있음 
/// </summary>
public class DarkSpiritSpawner : MonoBehaviour
{
    public GameObject darkSpiritPrefab;

    public int minSpawnCount = 2; 
    public int maxSpawnCount = 4;

    public float spawnRadius = 10f;

    public float minSpawnDelay = 1f; // 최소 스폰 딜레이
    public float maxSpawnDelay = 5f; // 최대 스폰 딜레이

    bool isNight;
    Coroutine spawnDarkSpiritRoutine;

    void Start()
    {
        StartCoroutine(SpawnDarkSpirits());
    }

    private void OnEnable()
    {
        EnvironmentManager.Instance.Time.OnNightStart += StartSpawnDarkSpirits;
        EnvironmentManager.Instance.Time.OnNightEnd += StopSpawnDarkSpirits;
    }

    private void OnDisable()
    {
        EnvironmentManager.Instance.Time.OnNightStart -= StartSpawnDarkSpirits;
        EnvironmentManager.Instance.Time.OnNightStart -= StopSpawnDarkSpirits;
    }

    void StartSpawnDarkSpirits()
    {
        isNight = true;

        if (spawnDarkSpiritRoutine != null)
        {
            spawnDarkSpiritRoutine = StartCoroutine(SpawnDarkSpirits());
        }
    }

    void StopSpawnDarkSpirits()
    {
        isNight = false;

        StopCoroutine(spawnDarkSpiritRoutine);
    }

    IEnumerator SpawnDarkSpirits()
    {
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            if (!isNight) break;
            float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay); // 랜덤한 시간 기다림

            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            // 어둠 정령 생성 후 타겟을 나(모닥불)로 설정 
            Instantiate(darkSpiritPrefab, spawnPosition, Quaternion.identity).GetComponent<DarkSprit>().targetCampfire = GetComponent<Campfire>(); 
        }

        spawnDarkSpiritRoutine = null;
    }
}

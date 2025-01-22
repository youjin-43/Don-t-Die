using System.Collections.Generic;
using UnityEngine;
using VInspector;


//EnvironmentManager 가 생성된 이후 실행돼야함! 
public class MonsterSpawnManager : MonoBehaviour
{
    // 에디터에서 값이 변경될 때마다 호출
    private void OnValidate()
    {
        // 변경된 값 확인 
        DebugController.Log("OnValidate called, spawnRadius: " + spawnRadius);
        DebugController.Log("OnValidate called, MinSpawnCnt: " + MinSpawnCnt);
        DebugController.Log("OnValidate called, MaxSpawnCnt: " + MaxSpawnCnt);
    }

    private static MonsterSpawnManager instance;
    public static MonsterSpawnManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    void Awake()
    {
        Init();
    }

    [SerializeField] float spawnRadius = 10f;
    [SerializeField] int MinSpawnCnt = 3;
    [SerializeField] int MaxSpawnCnt = 20;

    public SerializedDictionary<BiomeType, List<Vector3>> seedPoints; // 디버그용 
    public SerializedDictionary<BiomeType, List<GameObject>> MonsterPrefab;
    public SerializedDictionary<BiomeType, List<GameObject>> SpawnedMonster; // 디버그용 

    private void OnDrawGizmos()
    {
        foreach (var kvp in seedPoints)
        {
            BiomeType biome = kvp.Key;        // 현재 키 (BiomeType)
            List<Vector3> points = kvp.Value; // 현재 값 (List<Vector3>)

            foreach (var pos in points)
            {
                Gizmos.DrawSphere(pos, 5f);
            }
        }
    }

    public void InitializeBiomeMonsters()
    {
        DebugController.Log("InitializeBiomeMonsters 실행됨");

        seedPoints = EnvironmentManager.Instance.seedPoints;

        // 각 바이옴 별로 몬스터 무리 생성
        foreach (var kvp in seedPoints)
        {
            BiomeType biome = kvp.Key;        // 현재 키 (BiomeType)
            List<Vector3> points = kvp.Value; // 현재 값 (List<Vector3>)
            DebugController.Log($"Biome: {biome}, Seed Points Count: {points.Count}");

            List<GameObject> monsters = MonsterPrefab[biome]; //선택된 바이옴에서 스폰될 수 있는 몬스터 리스트 

            for (int i = 0; i < points.Count; i++)
            {
                //현재 포인트가 현재 키 바이옴과 일치한다면(가끔 포인트가 바다 위에 있는 경우가 있음)
                if (EnvironmentManager.Instance.biomeMap.GetTileBiome(new Vector2Int((int)points[i].x, (int)points[i].y)) == biome)
                {
                    DebugController.Log($"Biome: {biome}, Seed Points: {points[i]} 에 몬스터 무리 생성");

                    // 각 seed point를 중심으로 무작위 위치에 여러 몬스터 스폰
                    int monsterCount = Random.Range(MinSpawnCnt, MaxSpawnCnt); // 스폰할 몬스터 수 (조정 가능)
                    int monsterPrefabIdx = 0; // 스폰 될 수 있는 몬스터 중 랜덤으로 생성

                    for (int m = 0; m < monsterCount; m++)
                    {
                        // 스폰 반경 내에서 랜덤 위치 계산
                        Vector3 randomOffset = new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0);
                        Vector3 spawnPosition = points[i] + randomOffset;

                        // 스폰 위치가 맵 내부인지 확인 
                        Vector2Int tilePosition = new Vector2Int((int)spawnPosition.x, (int)spawnPosition.y);
                        if (EnvironmentManager.Instance.biomeMap.GetTileBiome(tilePosition) == biome)
                        {
                            GameObject go = Instantiate(monsters[monsterPrefabIdx++], spawnPosition, Quaternion.identity);
                            if (monsterPrefabIdx == monsters.Count) monsterPrefabIdx = 0; //하나의 몬스터만 생성되는것을 방지하기 위해 
                            go.GetComponent<SpriteRenderer>().sortingOrder = 100;
                            SpawnedMonster[biome].Add(go);
                        }
                    }
                }

            }
        }
    }

    public void ClearSpawnedMonsters()
    {
        foreach (var mosterList in SpawnedMonster)
        {
            List<GameObject> SpawnedMonsters = mosterList.Value;
            foreach (var monster in SpawnedMonsters)
            {
                DestroyObject(monster);
            }
            mosterList.Value.Clear();
        }
    }
}

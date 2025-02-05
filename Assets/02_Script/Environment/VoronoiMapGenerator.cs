using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Voronoi Diagram의 구성 요소. 랜덤한 위치에 찍히는 점.
/// </summary>
public struct SeedPoint
{
    public Vector2 position;
    public Biome biome;
}

public class VoronoiMapGenerator : MonoBehaviour
{
    [SerializeField] Tilemap landTilemap;
    [SerializeField] Tilemap waterTilemap;

    [SerializeField] List<Biome> landBiomes;
    [SerializeField] Biome waterBiome;

    public int mapWidth = 100;
    public int mapHeight = 100;

    /// <summary>
    /// Voronoi Digram에 찍힐 점 개수. 클수록 더 잘게 쪼개진다.
    /// </summary>
    public int seedPointCount = 20;
    public float noiseScale = 0.1f;

    [Range(0f, 1f)]
    public float waterEdgeSize = 0.4f;
    public float edgeNoiseScale = 0.1f;
    public float edgeNoiseStrength = 0.2f;

    public int riverCount = 3;
    public int lakeCount = 10;

    public GameObject objectParent;

    private void Update()
    {
        //#region Debug용
        // --- 타일이 바이옴 정보와 오브젝트 정보를 잘 가지고 있는지 디버깅 하는 부분!! -- 나중에 지우셈
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3Int tilemapPos = landTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        //    TileBase tile = landTilemap.GetTile(tilemapPos);

        //    //GenerateTreeTmp(tilemapPos);

        //    DebugController.Log($"map[{tilemapPos.y}, {tilemapPos.x}] {EnvironmentManager.Instance.biomeMap.GetTileBiome(new Vector2Int(tilemapPos.x, tilemapPos.y))}");
        //    DebugController.Log($"{EnvironmentManager.Instance.objectMap.Map[tilemapPos.y, tilemapPos.x]}");
        //}
        //#endregion
    }

    /// <summary>
    /// 맵의 모든 구성 요소를 지운다.
    /// </summary>
    public void Clear()
    {
        landTilemap.ClearAllTiles();
        waterTilemap.ClearAllTiles();

        if (objectParent != null)   // 자원들은 풀 오브젝트이므로 풀에 반환.
        {
            Transform[] children = objectParent.GetComponentsInChildren<Transform>();

            foreach(Transform child in children)
            {
                if (child.name == objectParent.name) { continue; }  // 자기 자신은 무시
                if (!PoolManager.Instance.HasPool(child.name)) { continue; }
                PoolManager.Instance.Push(child.gameObject);
            }
        }

        EnvironmentManager.Instance.seedPoints.Clear();
        EnvironmentManager.Instance.natureResources.Clear();
    }

    /// <summary>
    /// 데이터를 바탕으로 맵을 생성하는 함수
    /// </summary>
    /// <param name="biomeMap"></param>
    /// <param name="objectMap"></param>
    /// <param name="objects"></param>
    public void GenerateFromData(BiomeMap biomeMap, List<ResourceObject> objects)
    {
        Clear();
        GenerateVoronoiMap(biomeMap);
        GenerateObjects(objects);
    }

    public void Generate()
    {
        // 맵을 생성하기 전 모든 타일을 삭제한다.
        Clear();
        GenerateVoronoiMap();
        GenerateLakes();
        GenerateObjects();
    }

    /// <summary>
    /// 바이옴에 맞는 Tree, Plant, Mineral을 생성한다.
    /// </summary>
    void GenerateObjects()
    {
        if (objectParent == null)
        {
            GameObject parent = new GameObject("ObjectParent");
            parent.transform.parent = PoolManager.Instance.transform;
            objectParent = parent;
        }

        ObjectGenerator objectGenerator = new ObjectGenerator(EnvironmentManager.Instance.biomeMap, mapWidth, mapHeight); // biome 정보에 맞춰서 오브젝트를 생성하기 때문에 파라미터로 건네준다.
        List<ResourceObject> resourceObjects = objectGenerator.GetResourceObjects();
        List<InstallableObject> installableObjects = objectGenerator.GetInstallableObjects();
        EnvironmentManager.Instance.objectMap = objectGenerator.objectMap;

        foreach (ResourceObject obj in resourceObjects)
        {
            GameObject go = InstantiateObject(obj);
            if (go == null) continue;
            obj.gameObject = go;
            if (go.GetComponent<Growable>() != null)
            {
                obj.isGrowable = true;
            }
            if (go.GetComponent<DamageableResourceNode>() != null)
            {
                obj.isDamageable = true;
            }
            EnvironmentManager.Instance.natureResources.Add(obj.gameObject.transform.position, obj);
        }

        foreach(InstallableObject obj in installableObjects)
        {
            Vector3 position = landTilemap.GetCellCenterWorld(new Vector3Int(obj.position.x, obj.position.y));
            GameObject go = PoolManager.Instance.InstantiatePoolObject(DataManager.Instance.ItemData[obj.dataName].Prefab, rootParent: objectParent.transform);
            go.transform.position = position;
            obj.gameObject = go;
        }
    }

    void GenerateObjects(List<ResourceObject> objects)
    {
        if (objectParent == null)
        {
            GameObject parent = new GameObject("ObjectParent");
            parent.transform.parent = transform;
            objectParent = parent;
        }

        foreach (ResourceObject obj in objects)
        {
            GameObject go = InstantiateObject(obj);

            if (obj.isGrowable)
            {
                go.GetComponent<Growable>().GrowStage = obj.growthStage;
                go.GetComponent<Growable>().Timer = obj.timer;
            }

            if (obj.isDamageable)
            {
                go.GetComponent<DamageableResourceNode>().CurrentHealth = obj.currentHealth;
            }
        }
    }

    /// <summary>
    /// 좌표에 맞춰 오브젝트를 생성한다.
    /// </summary>
    GameObject InstantiateObject(ResourceObject obj)
    {
        Vector3 cellCenterPosition = landTilemap.GetCellCenterWorld(new Vector3Int(obj.position.x, obj.position.y));
        Vector3 cellPosition = landTilemap.CellToWorld(new Vector3Int(obj.position.x, obj.position.y));

        Vector3 position = cellPosition;

        if (DataManager.Instance.NatureResources[obj.dataName].Width % 2 != 0)
        {
            position.x = cellCenterPosition.x;
        }
        if (DataManager.Instance.NatureResources[obj.dataName].Height % 2 != 0)
        {
            position.y = cellCenterPosition.y;
        }

        if (position == EnvironmentManager.Instance.playerSpawnPosition) return null;

        GameObject go = PoolManager.Instance.InstantiatePoolObject(DataManager.Instance.NatureResources[obj.dataName].Prefab, rootParent:objectParent.transform);
        go.transform.position = position;
        obj.gameObject = go;

        return go;
    }

    /// <summary>
    /// Voronoi Diagram을 사용하여 랜덤하게 맵을 생성한다.
    /// </summary>
    void GenerateVoronoiMap()
    {
        List<SeedPoint> seedPoints = GenerateSeedPoints(seedPointCount);

        EnvironmentManager.Instance.biomeMap = new BiomeMap(mapWidth, mapHeight);

        // 맵의 센터는 중앙으로 둔다
        Vector2 mapCenter = new Vector2(mapWidth / 2, mapHeight / 2);

        // 맵의 중앙에서 가장 멀리 떨어진 거리. 외곽을 바다로 설정하기 위해 둔 변수이다. 
        float maxDistance = Mathf.Sqrt(Mathf.Pow(mapWidth / 2, 2) + Mathf.Pow(mapHeight / 2, 2));

        float playerSpawnPointDistance = Vector3.Distance(mapCenter, EnvironmentManager.Instance.playerSpawnPosition);

        // 맵 전체 순회
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector2 currentPos = new Vector2(x, y);
                float minDistance = float.MaxValue;
                Biome selectedBiome = landBiomes[0];

                // 지금 위치한 타일에서 가장 가까운 seed point의 biome type을 따라간다.
                foreach (var seed in seedPoints)
                {
                    // 직선으로 나누어지면 부자연스러우므로 Perlin Noise를 이용해 seed point들의 위치를 약간씩 흔든다.
                    float noise = Mathf.PerlinNoise((x + seed.position.x) * noiseScale, (y + seed.position.y) * noiseScale);
                    float jitter = Mathf.Lerp(-1f, 1f, noise) * 3f;

                    float distance = Vector2.Distance(currentPos, seed.position + new Vector2(jitter, jitter));

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        selectedBiome = seed.biome;
                    }
                }

                // 중심에서부터 어느 정도 떨어져 있는지 0~1 사이의 값으로 변환한다.
                float distanceFromCenter = Vector2.Distance(currentPos, mapCenter) / maxDistance;

                // 자연스러움을 위해 해안선도 똑같이 Perlin Noise를 적용한다.
                float edgeNoise = Mathf.PerlinNoise(x * edgeNoiseScale, y * edgeNoiseScale) * edgeNoiseStrength;
                float adjustedThreshold = (1 - waterEdgeSize) + edgeNoise;

                int seasonIdx = (int)EnvironmentManager.Instance.Time.CurrentSeason;
                if (distanceFromCenter > adjustedThreshold)
                {
                    waterTilemap.SetTile(new Vector3Int(x, y, 0), waterBiome.Tiles[seasonIdx]);  // 바다
                    EnvironmentManager.Instance.biomeMap.MarkTile(x, y, waterBiome.BiomeType);
                }
                else
                {
                    landTilemap.SetTile(new Vector3Int(x, y, 0), selectedBiome.Tiles[seasonIdx]);  // 육지
                    EnvironmentManager.Instance.biomeMap.MarkTile(x, y, selectedBiome.BiomeType);
                    if (selectedBiome.BiomeType == BiomeType.GrassBiome)
                    {
                        Vector3 playerSpawnPositionOnTilemap = landTilemap.GetCellCenterWorld(new Vector3Int(x, y));
                        if (Vector3.Distance(mapCenter, playerSpawnPositionOnTilemap) < playerSpawnPointDistance)
                        {
                            playerSpawnPointDistance = Vector3.Distance(mapCenter, playerSpawnPositionOnTilemap);
                            EnvironmentManager.Instance.playerSpawnPosition = playerSpawnPositionOnTilemap;
                        }
                    }
                }
            }
        }
    }

    void GenerateVoronoiMap(BiomeMap biomeMap)
    {
        int seasonIdx = (int)EnvironmentManager.Instance.Time.CurrentSeason;
        for (int x = 0; x < biomeMap.width; x++)
        {
            for (int y = 0; y < biomeMap.height; y++)
            {
                TileBase tile = DataManager.Instance.BiomeDatas[biomeMap.map[y][x].ToString()].Tiles[seasonIdx];
                if (biomeMap.map[y][x] == BiomeType.WaterBiome)
                {
                    waterTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
                else
                {
                    landTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }

    bool isValidPosition(Vector2 pos)
    {
        return pos.x >= 0 && pos.x < mapWidth && pos.y >= 0 && pos.y < mapHeight;
    }

    void GenerateLakes()
    {
        // 점을 생성한 후 그 점을 중심으로 하는 원형 연못을 생성한다.
        // 노이즈를 추가해서 가장자리에 변칙성을 준다.
        int seasonIdx = (int)EnvironmentManager.Instance.Time.CurrentSeason;
        List<SeedPoint> points = GenerateSeedPoints(lakeCount);
        foreach (SeedPoint center in points)
        {
            if (Vector3.Distance(center.position, EnvironmentManager.Instance.playerSpawnPosition) < float.Epsilon) { continue; }
            int maxRadius = UnityEngine.Random.Range(3, 8);
            for (int x = -maxRadius; x <= maxRadius; x++)
            {
                for (int y = -maxRadius; y <= maxRadius; y++)
                {
                    Vector2Int pos = new Vector2Int((int)center.position.x + x, (int)center.position.y + y);
                    Vector3 posVec3 = new Vector3(pos.x, pos.y);

                    float distance = Vector2.Distance(center.position, pos);

                    float noise = Mathf.PerlinNoise(pos.x * noiseScale, pos.y * noiseScale);
                    float adjustedRadius = maxRadius * (0.8f + noise * 0.4f);

                    if (distance <= adjustedRadius && isValidPosition(pos) && Vector3.Distance(posVec3, EnvironmentManager.Instance.playerSpawnPosition) > float.Epsilon)
                    {
                        landTilemap.SetTile(new Vector3Int(pos.y, pos.x, 0), null);
                        waterTilemap.SetTile(new Vector3Int(pos.y, pos.x, 0), waterBiome.Tiles[seasonIdx]);
                        EnvironmentManager.Instance.biomeMap.MarkTile(pos.y, pos.x, BiomeType.WaterBiome);
                    }
                }
            }
        }
    }

    public void UpdateTilesBySeason(Season season)
    {
        DebugController.Log("UpdateTilesBySeason");
        int seasonIdx = (int)EnvironmentManager.Instance.Time.CurrentSeason;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                string currentBiome = EnvironmentManager.Instance.biomeMap.GetTileBiome(new Vector2Int(x, y)).ToString();
                TileBase tile = DataManager.Instance.BiomeDatas[currentBiome].Tiles[seasonIdx];
                if (tile != null)
                {
                    landTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }

    public bool InstallObject(Vector3 position, InstallableItemData data)
    {
        Vector3 cellCenterPosition = landTilemap.GetCellCenterWorld(new Vector3Int((int)(position.x), (int)(position.y)));
        Vector3 cellPosition = landTilemap.CellToWorld(new Vector3Int((int)(position.x), (int)(position.y)));

        Vector3 tilePosition = cellPosition;

        if (data.Width % 2 != 0)
        {
            tilePosition.x = cellCenterPosition.x;
        }
        if (data.Height % 2 != 0)
        {
            tilePosition.y = cellCenterPosition.y;
        }

        //Item go = PoolManager.Instance.InstantiateItem(data);
        GameObject go = Instantiate(data.Prefab);
        go.transform.position = tilePosition;

        InstallableObject obj = new InstallableObject { 
            position = new Vector2Int((int)(position.x), (int)(position.y)),
            dataName = data.Name,
            gameObject = go.gameObject
        };

        EnvironmentManager.Instance.installableObjects.Add(tilePosition, obj);

        return true;
    }

/// <summary>
/// 전체 맵의 랜덤 포인트에 점을 찍고 biome을 랜덤하게 정한다.
/// </summary>
/// <returns></returns>
List<SeedPoint> GenerateSeedPoints(int count)
    {
        List<SeedPoint> seeds = new List<SeedPoint>();
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2(UnityEngine.Random.Range(0, mapWidth), UnityEngine.Random.Range(0, mapHeight));
            int randIdx = UnityEngine.Random.Range(0, landBiomes.Count);
            SeedPoint seed = new SeedPoint { position = pos, biome = landBiomes[randIdx] };
            seeds.Add(seed);

            if (EnvironmentManager.Instance.seedPoints.ContainsKey(seed.biome.BiomeType))
            {
                EnvironmentManager.Instance.seedPoints[seed.biome.BiomeType].Add(seed.position);
            }
            else
            {
                List<Vector3> v = new List<Vector3>();
                v.Add(seed.position);
                EnvironmentManager.Instance.seedPoints.Add(seed.biome.BiomeType, v);
            }
        }
        return seeds;
    }


}

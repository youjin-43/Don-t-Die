using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class MapData
{
    public BiomeMap biomeMap;
    public ObjectMap objectMap;
    public List<ResourceObject> resourceObjects;
}


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

    BiomeMap biomeMap;
    public ObjectMap objectMap;

    GameObject objectParent;

    private void Update()
    {
        // --- 타일이 바이옴 정보와 오브젝트 정보를 잘 가지고 있는지 디버깅 하는 부분!! -- 나중에 지우셈
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int tilemapPos = landTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            TileBase tile = landTilemap.GetTile(tilemapPos);

            //GenerateTreeTmp(tilemapPos);

            DebugController.Log($"map[{tilemapPos.y}, {tilemapPos.x}] {biomeMap.GetTileBiome(new Vector2Int(tilemapPos.x, tilemapPos.y))}");
            DebugController.Log($"{objectMap.Map[tilemapPos.y, tilemapPos.x]}");
        }

        // --- Damageable Resource 잘 작동하는지 체크하는 부분. 지금은 Grass Tree 2 종류만 체크

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit && hit.transform.GetComponent<DamageableResourceNode>() != null)
            {
                hit.transform.GetComponent<DamageableResourceNode>().Hit(10);
            }
        }
    }

    /// <summary>
    /// 맵의 모든 구성 요소를 지운다.
    /// </summary>
    public void Clear()
    {
        landTilemap.ClearAllTiles();
        waterTilemap.ClearAllTiles();

        if (objectParent != null)
        {
            DestroyImmediate(objectParent.gameObject);
            objectParent = null;
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
        GenerateObjects();
    }

    /// <summary>
    /// 바이옴에 맞는 Tree, Plant, Mineral을 생성한다.
    /// </summary>
    void GenerateObjects()
    {
        if (objectParent != null) // Generate하기 전에 Clear 과정이 있지만 안전을 위해
        {
            DestroyImmediate(objectParent.gameObject);
        }

        ObjectGenerator objectGenerator = new ObjectGenerator(biomeMap, mapWidth, mapHeight); // biome 정보에 맞춰서 오브젝트를 생성하기 때문에 파라미터로 건네준다.
        List<ResourceObject> objects = objectGenerator.Generate();
        objectMap = objectGenerator.objectMap;

        GameObject parent = new GameObject("ObjectParent"); // 오브젝트들이 담길 부모 오브젝트를 만들고
        parent.transform.parent = transform; // Map Generator의 자식으로 만든다.  구조 : Map Generator - ObjectParent - Objects
        objectParent = parent;

        foreach (ResourceObject obj in objects)
        {
            GameObject go = InstantiateObject(obj, parent.transform);
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

        EnvironmentManager.Instance.objectMap = objectMap;
        //EnvironmentManager.Instance.resourceObjects = objects;
    }

    void GenerateObjects(List<ResourceObject> objects)
    {
        if (objectParent != null)
        {
            DestroyImmediate(objectParent.gameObject);
        }

        GameObject parent = new GameObject("ObjectParent");
        parent.transform.parent = transform;
        objectParent = parent;

        foreach (ResourceObject obj in objects)
        {
            GameObject go = InstantiateObject(obj, parent.transform);
            
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
    GameObject InstantiateObject(ResourceObject obj, Transform parent)
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

        GameObject go = Instantiate(DataManager.Instance.NatureResources[obj.dataName].Prefab, position, Quaternion.identity, parent);
        obj.gameObject = go;

        return go;
    }

    /// <summary>
    /// Voronoi Diagram을 사용하여 랜덤하게 맵을 생성한다.
    /// </summary>
    void GenerateVoronoiMap()
    {
        List<SeedPoint> seedPoints = GenerateSeedPoints();

        biomeMap = new BiomeMap(mapWidth, mapHeight);

        // 맵의 센터는 중앙으로 둔다
        Vector2 mapCenter = new Vector2(mapWidth / 2, mapHeight / 2);

        // 맵의 중앙에서 가장 멀리 떨어진 거리. 외곽을 바다로 설정하기 위해 둔 변수이다. 
        float maxDistance = Mathf.Sqrt(Mathf.Pow(mapWidth / 2, 2) + Mathf.Pow(mapHeight / 2, 2));

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

                if (distanceFromCenter > adjustedThreshold)
                {
                    waterTilemap.SetTile(new Vector3Int(x, y, 0), waterBiome.Tile);  // 바다
                    biomeMap.MarkTile(x, y, waterBiome.BiomeType);
                }
                else
                {
                    landTilemap.SetTile(new Vector3Int(x, y, 0), selectedBiome.Tile);  // 육지
                    biomeMap.MarkTile(x, y, selectedBiome.BiomeType);
                }
            }
        }

        EnvironmentManager.Instance.biomeMap = biomeMap;
    }

    void GenerateVoronoiMap(BiomeMap biomeMap)
    {
        for (int x = 0; x < biomeMap.width; x++)
        {
            for (int y = 0; y < biomeMap.height; y++)
            {
                TileBase tile = DataManager.Instance.BiomeDatas[biomeMap.map[y][x].ToString()].Tile;
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

    /// <summary>
    /// 전체 맵의 랜덤 포인트에 점을 찍고 biome을 랜덤하게 정한다.
    /// </summary>
    /// <returns></returns>
    List<SeedPoint> GenerateSeedPoints()
    {
        List<SeedPoint> seeds = new List<SeedPoint>();
        for (int i = 0; i < seedPointCount; i++)
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

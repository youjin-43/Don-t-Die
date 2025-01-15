using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    /// <summary>
    /// Voronoi Diagram의 구성 요소. 랜덤한 위치에 찍히는 점.
    /// </summary>
    private struct SeedPoint
    {
        public Vector2 position;
        public Biome biome;
    }

    void Start()
    {
        Generate();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int tilemapPos = landTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            TileBase tile = landTilemap.GetTile(tilemapPos);

            //GenerateTreeTmp(tilemapPos);

            DebugController.Log($"map[{tilemapPos.y}, {tilemapPos.x}] {biomeMap.GetTileBiomeByPosition(tilemapPos.x, tilemapPos.y)}");
            DebugController.Log($"{objectMap.Map[tilemapPos.y, tilemapPos.x]}");
        }

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

        if (transform.childCount > 1)
            DestroyImmediate(transform.GetChild(1).gameObject);
    }

    public void Generate()
    {
        // 맵을 생성하기 전 모든 타일을 삭제한다.
        Clear();
        GenerateVoronoiMap();
        GenerateObjects();
    }

    void GenerateObjects()
    {
        ObjectGenerator objectGenerator = new ObjectGenerator(biomeMap, mapWidth, mapHeight);
        var objects = objectGenerator.Generate();
        objectMap = objectGenerator.objectMap;

        GameObject go = new GameObject("ObjectParent");
        go.transform.parent = transform;

        foreach (var obj in objects)
        {
            InstantiateObject(obj, go.transform);
        }
    }

    void InstantiateObject(ResourceObject obj, Transform parent)
    {
        Vector3 cellCenterPosition = landTilemap.GetCellCenterWorld(new Vector3Int(obj.position.x, obj.position.y));
        Vector3 cellPosition = landTilemap.CellToWorld(new Vector3Int(obj.position.x, obj.position.y));

        Vector3 position = cellPosition;

        if (obj.data.Width % 2 != 0)
        {
            position.x = cellCenterPosition.x;
        }
        if (obj.data.Height % 2 != 0)
        {
            position.y = cellCenterPosition.y;
        }

        GameObject go = Instantiate(obj.data.Prefab, position, Quaternion.identity, parent);
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
                    biomeMap.MarkTile(x, y, waterBiome);
                }
                else
                {
                    landTilemap.SetTile(new Vector3Int(x, y, 0), selectedBiome.Tile);  // 육지
                    biomeMap.MarkTile(x, y, selectedBiome);
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
            Vector2 pos = new Vector2(Random.Range(0, mapWidth), Random.Range(0, mapHeight));
            int randIdx = Random.Range(0, landBiomes.Count);
            seeds.Add(new SeedPoint { position = pos, biome = landBiomes[randIdx] });
        }
        return seeds;
    }
}

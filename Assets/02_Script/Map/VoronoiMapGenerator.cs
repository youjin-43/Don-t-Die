using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VoronoiTilemapGenerator : MonoBehaviour
{
    [SerializeField] Tilemap landTilemap;
    [SerializeField] Tilemap waterTilemap;

    [SerializeField] List<Biome> landBiomes;
    [SerializeField] Biome waterBiome;

    public int mapWidth = 100;
    public int mapHeight = 100;
    public int seedPointCount = 20;
    public float noiseScale = 0.1f;
    [Range(0f, 1f)]
    public float waterEdgeSize = 0.4f;
    public float edgeNoiseScale = 0.1f;
    public float edgeNoiseStrength = 0.2f;

    private struct SeedPoint
    {
        public Vector2 position;
        public Biome biome;
    }

    void Start()
    {
        GenerateVoronoiMap();
    }

    public void Clear()
    {
        landTilemap.ClearAllTiles();
        waterTilemap.ClearAllTiles();
    }

    public void GenerateVoronoiMap()
    {
        Clear();
        List<SeedPoint> seedPoints = GenerateSeedPoints();

        Vector2 mapCenter = new Vector2(0, 0);
        float maxDistance = Mathf.Sqrt(Mathf.Pow(mapWidth / 2, 2) + Mathf.Pow(mapHeight / 2, 2));

        for (int x = -mapWidth / 2; x < mapWidth / 2; x++)
        {
            for (int y = -mapHeight / 2; y < mapHeight / 2; y++)
            {
                Vector2 currentPos = new Vector2(x, y);
                float minDistance = float.MaxValue;
                Biome selectedBiome = landBiomes[0];

                foreach (var seed in seedPoints)
                {
                    float noise = Mathf.PerlinNoise((x + seed.position.x) * noiseScale, (y + seed.position.y) * noiseScale);
                    float jitter = Mathf.Lerp(-1f, 1f, noise) * 3f;

                    float distance = Vector2.Distance(currentPos, seed.position + new Vector2(jitter, jitter));

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        selectedBiome = seed.biome;
                    }
                }

                float distanceFromCenter = Vector2.Distance(currentPos, mapCenter) / maxDistance;

                float edgeNoise = Mathf.PerlinNoise(x * edgeNoiseScale, y * edgeNoiseScale) * edgeNoiseStrength;
                float adjustedThreshold = (1 - waterEdgeSize) + edgeNoise;

                if (distanceFromCenter > adjustedThreshold)
                {
                    waterTilemap.SetTile(new Vector3Int(x, y, 0), waterBiome.Tile);  // ¹Ù´Ù
                }
                else
                {
                    landTilemap.SetTile(new Vector3Int(x, y, 0), selectedBiome.Tile);  // À°Áö
                }
            }
        }
    }

    List<SeedPoint> GenerateSeedPoints()
    {
        List<SeedPoint> seeds = new List<SeedPoint>();
        for (int i = 0; i < seedPointCount; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-mapWidth / 2, mapWidth / 2), Random.Range(-mapHeight / 2, mapHeight / 2));
            int randIdx = Random.Range(0, landBiomes.Count);
            seeds.Add(new SeedPoint { position = pos, biome = landBiomes[randIdx] });
        }
        return seeds;
    }
}

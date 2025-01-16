using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct ResourceObject
{
    public Vector2Int position;
    public NatureResourceData data;
}

public class ObjectGenerator
{
    BiomeMap biomeMap;
    public ObjectMap objectMap;

    List<ResourceObject> resourceObjects;

    public ObjectGenerator(BiomeMap biomeMap, int width, int height)
    {
        this.biomeMap = biomeMap;
        objectMap = new ObjectMap(width, height);
        resourceObjects = new List<ResourceObject>();
    }

    public List<ResourceObject> Generate()
    {
        resourceObjects.Clear();
        GenerateTrees();
        GeneratePlants();
        GenerateMinerals();
        return resourceObjects;
    }

    void GenerateTrees()
    {
        for (int i = 0; i < objectMap.height; i++)
        {
            for (int j = 0; j < objectMap.width; j++)
            {
                EnvironmentManager.Instance.biomeDatas.TryGetValue(biomeMap.map[i][j], out Biome currentBiome);

                if (currentBiome == null || currentBiome.Trees.Count == 0)
                {
                    continue;
                }

                if (UnityEngine.Random.Range(0, 10000) / 100f < currentBiome.TreesIntensity)
                {
                    int randIdx = UnityEngine.Random.Range(0, currentBiome.Trees.Count);
                    NatureResourceData tree = currentBiome.Trees[randIdx];

                    if (biomeMap.IsValidPosition(new Vector2Int(j, i), tree.Width, tree.Height, tree.BiomeType)
                        && objectMap.AreTilesEmpty(new Vector2Int(j, i), tree.Width, tree.Height))
                    {
                        objectMap.MarkTiles(new Vector2Int(j, i), tree.Width, tree.Height, ObjectType.Tree);
                        resourceObjects.Add(new ResourceObject
                        {
                            position = new Vector2Int(j, i),
                            data = tree
                        });
                    }
                }
            }
        }
    }

    void GeneratePlants()
    {
        for (int i = 0; i < objectMap.height; i++)
        {
            for (int j = 0; j < objectMap.width; j++)
            {
                EnvironmentManager.Instance.biomeDatas.TryGetValue(biomeMap.map[i][j], out Biome currentBiome);

                if (currentBiome == null || currentBiome.Plants.Count == 0)
                {
                    continue;
                }

                if (UnityEngine.Random.Range(0, 10000) / 100f < currentBiome.PlantsIntensity)
                {
                    int randIdx = UnityEngine.Random.Range(0, currentBiome.Plants.Count);
                    NatureResourceData plant = currentBiome.Plants[randIdx];

                    if (biomeMap.IsValidPosition(new Vector2Int(j, i), plant.Width, plant.Height, plant.BiomeType)
                        && objectMap.AreTilesEmpty(new Vector2Int(j, i), plant.Width, plant.Height))
                    {
                        objectMap.MarkTiles(new Vector2Int(j, i), plant.Width, plant.Height, ObjectType.Plant);
                        resourceObjects.Add(new ResourceObject
                        {
                            position = new Vector2Int(j, i),
                            data = plant
                        });
                    }
                }
            }
        }
    }

    void GenerateMinerals()
    {
        for (int i = 0; i < objectMap.height; i++)
        {
            for (int j = 0; j < objectMap.width; j++)
            {
                EnvironmentManager.Instance.biomeDatas.TryGetValue(biomeMap.map[i][j], out Biome currentBiome);

                if (currentBiome == null || currentBiome.Minerals.Count == 0)
                {
                    continue;
                }

                if (UnityEngine.Random.Range(0, 10000) / 100f < currentBiome.MineralsIntensity)
                {
                    int randIdx = UnityEngine.Random.Range(0, currentBiome.Minerals.Count);
                    NatureResourceData mineral = currentBiome.Minerals[randIdx];

                    if (biomeMap.IsValidPosition(new Vector2Int(j, i), mineral.Width, mineral.Height, mineral.BiomeType)
                        && objectMap.AreTilesEmpty(new Vector2Int(j, i), mineral.Width, mineral.Height))
                    {
                        objectMap.MarkTiles(new Vector2Int(j, i), mineral.Width, mineral.Height, ObjectType.Mineral);
                        resourceObjects.Add(new ResourceObject
                        {
                            position = new Vector2Int(j, i),
                            data = mineral
                        });
                    }
                }
            }
        }
    }
}

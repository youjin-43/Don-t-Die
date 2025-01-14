using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator
{
    BiomeMap biomeMap;
    public ObjectMap objectMap;

    List<ResourceObject> resourceObjects;

    public struct ResourceObject
    {
        public Vector2Int position;
        public RenewableResourceData data;
    }

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
        return resourceObjects;
    }

    void GenerateTrees()
    {
        for (int i = 0; i < objectMap.height; i++)
        {
            for (int j = 0; j < objectMap.width; j++)
            {
                Biome currentBiome = biomeMap.Map[i, j];
                if (Random.Range(0, 10000) / 100f < currentBiome.TreesIntensity)
                {
                    int randIdx = Random.Range(0, currentBiome.Trees.Count);
                    RenewableResourceData tree = currentBiome.Trees[randIdx];

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
                Biome currentBiome = biomeMap.Map[i, j];
                if (Random.Range(0, 10000) / 100f < currentBiome.PlantsIntensity)
                {
                    int randIdx = Random.Range(0, currentBiome.Plants.Count);
                    RenewableResourceData plant = currentBiome.Plants[randIdx];

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
}

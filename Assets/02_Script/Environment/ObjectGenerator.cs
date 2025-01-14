using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator
{
    BiomeMap biomeMap;
    public ObjectMap objectMap;

    List<TreeObject> treeObjects;

    public struct TreeObject
    {
        public Vector2Int position;
        public GameObject prefab;
    }

    public ObjectGenerator(BiomeMap biomeMap, int width, int height)
    {
        this.biomeMap = biomeMap;
        objectMap = new ObjectMap(width, height);
        treeObjects = new List<TreeObject>();
    }

    public List<TreeObject> Generate()
    {
        GenerateTrees();
        return treeObjects;
    }

    void GenerateTrees()
    {
        treeObjects.Clear();
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
                        treeObjects.Add(new TreeObject
                        {
                            position = new Vector2Int(j, i),
                            prefab = tree.Prefab
                        });
                    }
                }
            }
        }
    }
}

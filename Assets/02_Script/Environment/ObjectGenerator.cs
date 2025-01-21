using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ResourceObject
{
    public Vector2Int position;
    public string dataName;
    public bool isDamageable;
    public bool isGrowable;
    public float currentHealth;
    public int growthStage;
    public int timer;

    [System.NonSerialized] public GameObject gameObject;    // GameObject는 직렬화 못 함. 런타임에서 어떤 GameObject와 연결되어 있는지 캐싱
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
        GenerateObjects(ObjectType.Tree);
        GenerateObjects(ObjectType.Plant);
        GenerateObjects(ObjectType.Mineral);
        return resourceObjects;
    }

    void GenerateObjects(ObjectType type)
    {
        for (int i = 0; i < objectMap.height; i++)
        {
            for (int j = 0; j < objectMap.width; j++)
            {
                DataManager.Instance.BiomeDatas.TryGetValue(biomeMap.map[i][j].ToString(), out Biome currentBiome);

                if (currentBiome == null || currentBiome.NatureResources[type].Count == 0)
                {
                    continue;
                }

                if (UnityEngine.Random.Range(0, 10000) / 100f < currentBiome.Intensities[type])
                {
                    int randIdx = UnityEngine.Random.Range(0, currentBiome.NatureResources[type].Count);
                    NatureResourceData obj = currentBiome.NatureResources[type][randIdx];

                    if (biomeMap.IsValidPosition(new Vector2Int(j, i), obj.Width, obj.Height, obj.BiomeType)
                        && objectMap.AreTilesEmpty(new Vector2Int(j, i), obj.Width, obj.Height))
                    {
                        objectMap.MarkTiles(new Vector2Int(j, i), obj.Width, obj.Height, type);
                        resourceObjects.Add(new ResourceObject
                        {
                            position = new Vector2Int(j, i),
                            dataName = obj.name,
                        });
                    }
                }
            }
        }
    }
}

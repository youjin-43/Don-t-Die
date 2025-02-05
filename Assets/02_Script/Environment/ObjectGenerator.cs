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

[Serializable]
public class InstallableObject
{
    public Vector2Int position;
    public string dataName;

    [System.NonSerialized] public GameObject gameObject;
}

public class ObjectGenerator
{
    BiomeMap biomeMap;
    public ObjectMap objectMap;

    List<ResourceObject> resourceObjects;
    List<InstallableObject> installableObjects;

    public ObjectGenerator(BiomeMap biomeMap, int width, int height)
    {
        this.biomeMap = biomeMap;
        objectMap = new ObjectMap(width, height);
        resourceObjects = new List<ResourceObject>();
        installableObjects = new List<InstallableObject>();
    }

    /// <summary>
    /// 수확 가능한 자원들을 생성한다.
    /// </summary>
    /// <returns></returns>
    public List<ResourceObject> GetResourceObjects()
    {
        resourceObjects.Clear();
        GenerateObjectsByType(ObjectType.Tree);
        GenerateObjectsByType(ObjectType.Plant);
        GenerateObjectsByType(ObjectType.Mineral);
        GenerateItems();
        return resourceObjects;
    }

    /// <summary>
    /// 설치 가능한 오브젝트들을 생성한다. (현재는 보물상자만 생성됨)
    /// </summary>
    /// <returns></returns>
    public List<InstallableObject> GetInstallableObjects()
    {
        GenerateInstallableObjects();

        return installableObjects;
    }

    void GenerateObjectsByType(ObjectType type)
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

    void GenerateInstallableObjects()
    {
        for (int i = 0; i < objectMap.height; i++)
        {
            for (int j = 0; j < objectMap.width; j++)
            {
                DataManager.Instance.BiomeDatas.TryGetValue(biomeMap.map[i][j].ToString(), out Biome currentBiome);

                if (currentBiome == null || currentBiome.InstallableItemData == null)
                {
                    continue;
                }

                if (UnityEngine.Random.Range(0, 10000) / 100f < currentBiome.InstallableObjectsIntensity)
                {
                    InstallableItemData obj = currentBiome.InstallableItemData;

                    if (biomeMap.IsValidPosition(new Vector2Int(j, i), 1, 1, currentBiome.BiomeType)
                        && objectMap.AreTilesEmpty(new Vector2Int(j, i), 1, 1))
                    {
                        objectMap.MarkTiles(new Vector2Int(j, i), 1, 1, ObjectType.Installable);
                        installableObjects.Add(new InstallableObject
                        {
                            position = new Vector2Int(j, i),
                            dataName = obj.name,
                        });
                    }
                }
            }
        }
    }

    void GenerateItems()
    {
        for (int i = 0; i < biomeMap.height; i++)
        {
            for (int j = 0; j < biomeMap.width; j++)
            {
                DataManager.Instance.BiomeDatas.TryGetValue(biomeMap.map[i][j].ToString(), out Biome currentBiome);
                if (currentBiome == null || currentBiome.DropItems.Count == 0)
                {
                    continue;
                }

                if (UnityEngine.Random.Range(0, 1000) / 100f < currentBiome.DropItemsIntensity)
                {
                    ItemData itemData = currentBiome.DropItems[UnityEngine.Random.Range(0, currentBiome.DropItems.Count)].data;
                    Item item = PoolManager.Instance.InstantiateItem(itemData);
                    Vector3 position = new Vector3(j, i);
                    do
                    {
                        position = new Vector3(j, i) + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
                    } while (!biomeMap.IsOnMap(position));
                    
                    item.transform.position = position;
                }
            }
        }
    }
}

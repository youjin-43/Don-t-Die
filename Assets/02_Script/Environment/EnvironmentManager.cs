using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using VInspector;

[Serializable]
public class MapData
{
    public BiomeMap biomeMap;
    public ObjectMap objectMap;
    public List<ResourceObject> resourceObjects;
    public List<InstallableObject> installableObjects;
}


public class EnvironmentManager : MonoBehaviour
{
    public BiomeMap biomeMap;
    public ObjectMap objectMap;
    VoronoiMapGenerator voronoiMapGenerator;
    TimeController timeController;
    public SerializedDictionary<Vector3, ResourceObject> natureResources = new SerializedDictionary<Vector3, ResourceObject>();
    public SerializedDictionary<Vector3, InstallableObject> installableObjects = new SerializedDictionary<Vector3, InstallableObject>();
    public SerializedDictionary<BiomeType, List<Vector3>> seedPoints = new SerializedDictionary<BiomeType, List<Vector3>>();

    public VoronoiMapGenerator VoronoiMapGenerator
    {
        get
        {
            if (voronoiMapGenerator == null)
            {
                voronoiMapGenerator = (VoronoiMapGenerator)FindAnyObjectByType(typeof(VoronoiMapGenerator));
            }
            return voronoiMapGenerator;
        }
    }
    public TimeController Time 
    {
        get {  return timeController; } 
    }  

    private static EnvironmentManager instance;
    public static EnvironmentManager Instance
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
            timeController = GetComponent<TimeController>();
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

    void Start()
    {
        VoronoiMapGenerator.Generate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (LoadData())
            {
                VoronoiMapGenerator.GenerateFromData(biomeMap, natureResources.Values.ToList());
            } 
        }
    }

    public void SaveData()
    {
        foreach (ResourceObject obj in natureResources.Values)
        {
            if (obj.isGrowable)
            {
                obj.growthStage = obj.gameObject.GetComponent<Growable>().GrowStage;
                obj.timer = obj.gameObject.GetComponent<Growable>().Timer;
            }
            if (obj.isDamageable)
            {
                obj.currentHealth = obj.gameObject.GetComponent<DamageableResourceNode>().CurrentHealth;
            }
        }

        MapData mapData = new MapData
        {
            biomeMap = biomeMap,
            objectMap = objectMap,
            resourceObjects = natureResources.Values.ToList()
        };

        string json = JsonConvert.SerializeObject(mapData);

        File.WriteAllText(Application.persistentDataPath + "/mapData.json", json);
        DebugController.Log($"Save Map Data at {Application.persistentDataPath}");
    }

    bool LoadData()
    {
        string filePath = Application.persistentDataPath + "/mapData.json";

        if (File.Exists(filePath))
        {
            voronoiMapGenerator.Clear();
            string json = File.ReadAllText(filePath);

            MapData mapData = JsonConvert.DeserializeObject<MapData>(json);

            List<ResourceObject> resourceObjects = new List<ResourceObject>();

            if (mapData != null)
            {
                biomeMap = mapData.biomeMap;
                objectMap = mapData.objectMap;
                resourceObjects = mapData.resourceObjects;

                foreach(ResourceObject resourceObject in resourceObjects)
                {
                    Vector3 position = new Vector3(resourceObject.position.x, resourceObject.position.y, 0);
                    natureResources.Add(position, resourceObject);
                }

                return true;
            }
        }

        DebugController.Log($"Load Map Failed.");
        return false;
    }

    public void UpdateTilesBySeason()
    {
        voronoiMapGenerator.UpdateTilesBySeason(timeController.CurrentSeason);
    }

    public bool InstallObject(Vector3 position, InstallableItemData data)
    {
        Vector2Int cellPosition = new Vector2Int((int)(position.x), (int)(position.y));
        if (!objectMap.AreTilesEmpty(cellPosition, data.Width, data.Height) || !biomeMap.IsOnMap(cellPosition, data.Width, data.Height)) return false;

        objectMap.MarkTiles(cellPosition, data.Width, data.Height, ObjectType.Installable);
        return voronoiMapGenerator.InstallObject(position, data);
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using VInspector;

public class EnvironmentManager : MonoBehaviour
{
    public BiomeMap biomeMap;
    public ObjectMap objectMap;
    VoronoiMapGenerator voronoiMapGenerator;
    public SerializedDictionary<Vector3, ResourceObject> natureResources = new SerializedDictionary<Vector3, ResourceObject>();
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
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public BiomeMap biomeMap;
    public ObjectMap objectMap;
    public List<ResourceObject> resourceObjects;
    VoronoiMapGenerator voronoiMapGenerator;

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
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (LoadData())
            {
                VoronoiMapGenerator.GenerateFromData(biomeMap, null, null);
            } 
        }
    }

    public void SaveData()
    {
        MapData mapData = new MapData
        {
            biomeMap = biomeMap,
            //objectMap = objectMap,
            //resourceObjects = resourceObjects
        };

        string json = JsonConvert.SerializeObject(mapData);

        File.WriteAllText(Application.persistentDataPath + "/mapData.json", json);
    }

    bool LoadData()
    {
        string filePath = Application.persistentDataPath + "/mapData.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            MapData mapData = JsonConvert.DeserializeObject<MapData>(json);

            if (mapData != null)
            {
                biomeMap = mapData.biomeMap;
                return true;
            }
        }
        return false;
    }
}

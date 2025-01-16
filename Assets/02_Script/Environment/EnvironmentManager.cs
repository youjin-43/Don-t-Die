using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public BiomeMap biomeMap;
    public ObjectMap objectMap;
    public List<ResourceObject> resourceObjects;
    VoronoiMapGenerator voronoiMapGenerator;
    public Dictionary<BiomeType, Biome> biomeDatas;

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
            biomeDatas = new Dictionary<BiomeType, Biome>();
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        VoronoiMapGenerator.Generate();
    }

    
    void Update()
    {
        
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    private static GameManager instance;
    public static GameManager Instance
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


    public bool Test = false;


    private void Start()
    {
        if(Test == false)
        {
            GenMonsters();
            SpawnPlayer();
        }
    }

    public Transform PlayerTransform; //인스펙터에서 할당해야함
    public Vector3 GetPlayerPos()
    {
        return PlayerTransform.position;
    }

    public Vector2 PlayerDir { get; set; } // PlayerMove.cs에서 캐싱받고있음

    void GenMonsters()
    {
        Debug.Log("GenMonsters 호출됨 ");
        MonsterSpawnManager.Instance.InitializeBiomeMonsters();
    }

    void SpawnPlayer()
    {
        GameObject go = Instantiate(playerPrefab);
        PlayerTransform = go.transform;

        int count = EnvironmentManager.Instance.seedPoints[BiomeType.GrassBiome].Count;

        BiomeMap biomeMap = EnvironmentManager.Instance.biomeMap;
        Vector3 position = new Vector3(100, 100);
        float timer = 0f;

        do
        {
            timer += Time.deltaTime;
            int randIdx = Random.Range(0, count);
            position = EnvironmentManager.Instance.seedPoints[BiomeType.GrassBiome][randIdx];
        }
        while (timer < 5f && !biomeMap.IsOnMap(position));

        go.transform.position = position;
    }
}

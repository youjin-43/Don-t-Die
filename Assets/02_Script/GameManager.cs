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

    private void Start()
    {
        GenMonsters();
        SpawnPlayer();
    }

    public Transform PlayerTransform; //인스펙터에서 할당해야함
    public Vector3 GetPlayerPos()
    {
        return PlayerTransform.position;
    }

    void GenMonsters()
    {
        Debug.Log("GenMonsters 호출됨 ");
        MonsterSpawnManager.Instance.InitializeBiomeMonsters();
    }

    void SpawnPlayer()
    {
        GameObject go = Instantiate(playerPrefab);
        PlayerTransform = go.transform;
        if (EnvironmentManager.Instance.playerSpawnPosition == Vector3.zero)    // 만에 하나 GrassBiome 타일이 하나도 없는 경우
        {
            go.transform.position = new Vector3(100, 100);
        }
        else 
        {
            go.transform.position = EnvironmentManager.Instance.playerSpawnPosition; 
        }
    }
}

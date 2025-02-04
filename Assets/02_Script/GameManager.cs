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
            //InitStatusUI();
        }
    }

    void InitStatusUI()
    {
        StatusUIManager.Instance.UpdateHealthUI();
        StatusUIManager.Instance.UpdateHungryUI();
        StatusUIManager.Instance.UpdateThirstyUI();
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
        StatusUIManager.Instance.playerStatus = PlayerTransform.GetComponent<PlayerStatus>();

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

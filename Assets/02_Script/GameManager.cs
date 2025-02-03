using UnityEngine;

public class GameManager : MonoBehaviour
{

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
}

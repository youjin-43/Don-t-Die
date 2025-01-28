using UnityEngine;
using UnityEngine.UI;
using VInspector;

// 조합 UI
public class CraftManager : MonoBehaviour
{
    #region SINGLETON
    private static CraftManager instance;
    public  static CraftManager Instance
    {
        get
        {
            return instance;
        }
    }

    void SingletonInitialize()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    // 조합 UI 상단 탭의 카테고리들 담아 둘 컨테이너
    public SerializedDictionary<string, CraftCategory> Categories = new SerializedDictionary<string, CraftCategory>();

    // 조합 UI 중앙부 스크롤뷰에 넣을 조합식 프리팹
    [SerializeField] GameObject CraftListPrefab;

    // 조합식에 필요한 아이템 슬롯 프리팹
    [SerializeField] GameObject CraftListItemSlotPrefab;

    void Awake()
    {
        SingletonInitialize();
    }

    void Start()
    {
        // 로드 해 온 데이터들을 순회하면서
        foreach (var data in DataManager.Instance.CraftingData)
        {
            // 카테고리에 맞는 데이터들을
            if(Categories.TryGetValue(data.Value.Category, out CraftCategory category))
            {
                // 카테고리에 추가해줌
                category.AddData(data.Value);
            }
        }
        // 카테고리들을 순회하면서
        foreach (var category in Categories)
        {
            // 카테고리 초기화
            category.Value.Start();

            // 카테고리에 버튼 이벤트 바인딩
            category.Value.transform.GetComponent<Button>().onClick.AddListener(() => TabClicked(category.Value.name));

            // 카테고리가 들고 있어야 할 조합식들을 생성
            category.Value.CreateCraftList(CraftListPrefab, CraftListItemSlotPrefab);
        }

        // 처음 시작할 땐 닫아놓자
        ToggleCraftingUI();
    }

    // 카테고리 클릭 이벤트
    public void TabClicked(string tabName)
    {
        foreach(var tab in Categories)
        {
            tab.Value.ToggleCraftCategory(tabName);
        }
    }

    // 조합 UI 스위치
    public void ToggleCraftingUI()
    {
        foreach(var category in Categories)
        {
            category.Value.ResouceCounting(InventoryManager.Instance.GetInventoryDict());
        }

        gameObject.SetActive(!gameObject.activeSelf);
    }
}

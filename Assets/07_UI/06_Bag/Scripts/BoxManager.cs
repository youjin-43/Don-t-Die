using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 상자 UI
public class BoxManager : MonoBehaviour
{
    #region SINGLETON
    private static BoxManager instance;
    public  static BoxManager Instance
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

    // 인벤토리가 들고 있을 박스 슬롯 프리팹
    [SerializeField] GameObject BoxItemSlotPrefab;

    // 아이템 슬롯 최대 갯수
    private int _maxBoxSize = 9;

    // 아이템 슬롯을 담아놓을 컨테이너
    private List<BoxItemSlot> _boxSlot = new List<BoxItemSlot>();

    // 슬롯에 뭐가 들어가 있는지 확인하기 위한 컨테이너
    private Dictionary<string, int> _boxDict = new Dictionary<string, int>();


    // 슬롯 카운터 이미지
    [SerializeField]
    public List<Sprite> _itemCountImages = new List<Sprite>();


    // 드래깅용 변수
    [SerializeField] RectTransform _dragUI;
    private BoxItemSlot            _startSlot;
    private ItemData               _startSlotItemData;
    private int                    _startSlotItemCount;
    private int                    _startSlotDurability;
    private bool                   _isDragging = false;


    void Awake()
    {
        SingletonInitialize();
    }

    void Start()
    {
        Transform parent = transform.GetChild(1).GetChild(0).transform;

        // 자식으로 아이템 슬롯을 생성
        for (int i = 1; i <= 9; ++i)
        {
            BoxItemSlot go = Instantiate(BoxItemSlotPrefab, parent).GetComponent<BoxItemSlot>();

            _boxSlot.Add(go);
        }

        _dragUI.gameObject.SetActive(false);
        _dragUI.GetComponent<Image>().raycastTarget = false;

        // 처음 시작할 땐 닫아놓자
        ToggleBoxUI();
    }

    void Update()
    {
        DragAndDropItem();
    }

    // 조합 UI 스위치
    public void ToggleBoxUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public bool IsBoxOpened()
    {
        return gameObject.activeSelf;
    }

    public Dictionary<string, int> GetInventoryDict()
    {
        return _boxDict;
    }

    private void DragAndDropItem()
    {
        // 드래그 중 이라면
        if (_isDragging == true)
        {
            // 데이터를 옮겨 실은 UI는 마우스 위치를 따라감
            _dragUI.position = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                if (UIManager.Instance.IsUIClick() == false)
                {
                    EdibleItemData edibleItemData = _startSlotItemData as EdibleItemData;

                    // 구울 수 있니?
                    if(edibleItemData != null && edibleItemData.PossibleGrilling == true)
                    {
                        DropItemToCampfire(edibleItemData);
                    }
                    // 나무니?
                    else if(_startSlotItemData.Name == "Wood")
                    {
                        DropItemToCampfire(_startSlotItemData as ResourceItemData);    
                    }
                    else
                    { 
                        DropItemToField();
                    }
                }
            }
        }
    }

    public void RemoveItemFromDict(string itemName, int itemCount)
    {
        _boxDict[itemName] -= itemCount;
    }


    public bool AddItem(ItemData itemData, int durability = 0)
    {
        for (int i = 0; i < _maxBoxSize; ++i)
        {
            // 슬롯이 비어있다면
            if (_boxSlot[i].IsEmpty() == true)
            {
                // 다른 슬롯에 같은 아이템이 있는가
                if (_boxDict.TryGetValue(itemData.Name, out int count) && count != 0)
                {
                    // 있으면 그 슬롯까지 계속 감
                    continue;
                }
                else
                {
                    // 아이템 추가
                    AddItemToSlot(itemData, i, durability);

                    return true;
                }
            }
            // 슬롯이 비어있지 않다면
            else
            {
                // 같은 아이템인가
                if (_boxSlot[i].GetItemName() == itemData.Name)
                {
                    // 같은 아이템이지만 꽉 차있는가?
                    if (_boxSlot[i].IsFull() == true)
                    {
                        // 처음부터 다시 돌아서 빈 칸에 추가함
                        if (SearchFromFirstSlot(itemData, out int slot) == true && slot >= 0)
                        {
                            AddItemToSlot(itemData, slot, durability);

                            return true;
                        }
                        // 다 돌았는데도 슬롯이 없음 = 꽉참
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        // 아이템 추가
                        AddItemToSlot(itemData, i, durability);

                        return true;
                    }
                }
                // 다른 아이템인가
                else
                {
                    // 다음 슬롯으로
                    continue;
                }
            }
        }

        // 꽉 참
        return false;
        //}
    }

    private bool SearchFromFirstSlot(ItemData itemData, out int value)
    {
        int emptySlot = int.MaxValue;
        int sameItemSlot = int.MaxValue;

        for (int i = 0; i < _maxBoxSize; ++i)
        {
            // 슬롯이 비어있다면
            if (_boxSlot[i].IsEmpty() == true)
            {
                if (emptySlot > i)
                {
                    emptySlot = i;
                }
            }
            // 비어있지는 않은데 같은 아이템이 들어있고 가득 채워지지 않았다면
            else if (_boxSlot[i].GetItemData(out int count) == itemData
                && _boxSlot[i].IsFull() == false)
            {
                if (sameItemSlot > i)
                {
                    sameItemSlot = i;
                }
            }
        }

        if (sameItemSlot != int.MaxValue)
        {
            value = sameItemSlot;
            return true;
        }
        else if (sameItemSlot == int.MaxValue)
        {
            value = emptySlot;
            return true;
        }
        else
        {
            value = -1;
            return false;
        }
    }

    private void AddItemToSlot(ItemData itemData, int slot, int durability)
    {
        _boxSlot[slot]._currentDurability = durability;
        _boxSlot[slot].AddItemData(itemData);

        if (_boxDict.ContainsKey(itemData.Name))
        {
            _boxDict[itemData.Name]++;
        }
        else
        {
            _boxDict[itemData.Name] = 1;
        }
    }

    public bool IsDragging()
    {
        return _isDragging;
    }

    public void BeginDragData(BoxItemSlot startSlot)
    {
        // 빈 슬롯을 클릭했다면
        if (startSlot.IsEmpty() == true)
        {
            return;
        }
        // 드래깅 플래그
        {
            _isDragging = true;
        }
        // 출발지점, 데이터 캐싱
        {
            _startSlot = startSlot;
            _startSlotItemData = startSlot.GetItemData(out int itemCount);
            _startSlotItemCount = itemCount;
            _startSlotDurability = startSlot._currentDurability;
        }
        // 드래그 UI 활성화
        {
            _dragUI.gameObject.SetActive(true);
            _dragUI.GetComponent<Image>().sprite = _startSlotItemData.Image;
        }
        // 슬롯 초기화
        {
            _startSlot.ClearItemSlot();
        }
    }

    public void EndDragData(BoxItemSlot endSlot)
    {
        // 비어 있어야
        if (endSlot.IsEmpty() == true)
        {
            endSlot._currentDurability = _startSlotDurability;
            endSlot.AddItemData(_startSlotItemData, _startSlotItemCount);
        }
        // 비어있지 않다면
        else
        {
            // 1. 비어있지 않았는데 양 슬롯에 있던 아이템이 같고 재료 아이템이였다면
            if (_startSlotItemData.Name == endSlot.GetItemName()
                && (_startSlotItemData.ItemType == ItemType.Resource
                || _startSlotItemData.ItemType == ItemType.Edible))
            {
                ItemData endSlotItemData = endSlot.GetItemData(out int itemCount);
                int endSlotItemCount = itemCount;

                // 합치려면 양쪽 아이템 갯수를 더했을 때
                // 인벤토리 최대 적재량 ( 9개 ) 과 같다면
                if (_startSlotItemCount + endSlotItemCount <= 9)
                {
                    endSlot.AddItemData(_startSlotItemData, _startSlotItemCount);
                    _startSlot.ClearItemSlot();
                }
                // 인벤토리 최대 적재량 보다 크다면
                else
                {
                    endSlot.AddItemData(_startSlotItemData, 9 - endSlotItemCount);
                    _startSlot.AddItemData(_startSlotItemData, _startSlotItemCount - (9 - endSlotItemCount));
                }
            }
            // 2. 비어있지 않았는데 서로 아이템이 다른 경우 (재료든 장비든 뭐든)
            else
            {
                // 데이터 스왑
                ItemData endSlotItemData = endSlot.GetItemData(out int itemCount);
                int endSlotDurability = endSlot._currentDurability;
                endSlot.ClearItemSlot();

                endSlot._currentDurability = _startSlotDurability;
                endSlot.AddItemData(_startSlotItemData, _startSlotItemCount);

                _startSlot._currentDurability = endSlotDurability;
                _startSlot.AddItemData(endSlotItemData, itemCount);
            }
        }
        // 드래그 UI 비활성화
        {
            _dragUI.gameObject.SetActive(false);
        }
        // 드래그 플래그
        {
            _isDragging = false;
        }
    }

    private void ClearDragUI()
    {
        // 드래그 UI 비활성화
        {
            _dragUI.gameObject.SetActive(false);
        }
        // 캐시데이터, 출발지점, 데이터  초기화
        {
            _startSlot.ClearItemSlot();
            _startSlot = null;
            _startSlotItemData = null;
            _startSlotItemCount = 0;
        }
        // 드래그 플래그
        {
            _isDragging = false;
        }
    }

    private void DropItemToCampfire(ResourceItemData wood)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            string[] split = hit.collider.name.Split('(');

            if (split[0] == "Campfire" &&
            Vector3.Distance(GameManager.Instance.GetPlayerPos(), hit.collider.transform.position) < 2f)
            {
                hit.collider.gameObject.GetComponent<Campfire>().AddDurability(_startSlotItemCount * 10);

                RemoveItemFromDict(wood.Name, _startSlotItemCount);

                ClearDragUI();
            }
        }
        else
        {
            DropItemToField();
        }
    }

    private void DropItemToCampfire(EdibleItemData edibleItemData)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            string[] split = hit.collider.name.Split('(');

            if (split[0] == "Campfire" &&
            Vector3.Distance(GameManager.Instance.GetPlayerPos(), hit.collider.transform.position) < 2f)
            {
                for (int i = 0; i < _startSlotItemCount; ++i)
                {
                    Item item = PoolManager.Instance.InstantiateItem(edibleItemData.ItemDataAfterGrilled);

                    Vector3 dir = hit.collider.transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));

                    item.Spread(hit.collider.transform.position, dir, Random.Range(2.5f, 3f));
                }
            }

            RemoveItemFromDict(edibleItemData.Name, _startSlotItemCount);

            ClearDragUI();
        }
        else
        {
            DropItemToField();
        }
    }

    private void DropItemToField()
    {
        // 아이템 생성, 데이터 이전
        {
            for (int i = 0; i < _startSlotItemCount; ++i)
            {
                Item item = PoolManager.Instance.InstantiateItem(_startSlotItemData);
                item.currentDurability = _startSlotDurability;

                Vector3 dir = GameManager.Instance.GetPlayerPos() + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));

                item.Spread(GameManager.Instance.GetPlayerPos(), dir, Random.Range(2.5f, 3f));
            }
        }

        _boxDict[_startSlotItemData.Name] -= _startSlotItemCount;
        ClearDragUI();
    }

    public bool SendItemDataToInventory(ItemData itemData, int durability)
    {
        _boxDict[itemData.Name] -= 1;

        return InventoryManager.Instance.AddItem(itemData, durability);
    }
}
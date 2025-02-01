using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InventoryItemSlot;

// 인벤토리 UI
public class InventoryManager : MonoBehaviour
{
    #region SINGLETON
    private static InventoryManager instance;
    public  static InventoryManager Instance
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

    // 인벤토리가 들고 있을 아이템 슬롯 프리팹
    [SerializeField] GameObject ItemSlotPrefab;

    // 아이템 슬롯 최대 갯수
    private int _maxInventorySize = 15;

    // 아이템 슬롯을 담아놓을 컨테이너
    private List<InventoryItemSlot> _inventorySlot = new List<InventoryItemSlot>();

    // 슬롯에 뭐가 들어가 있는지 확인하기 위한 컨테이너
    private Dictionary<string, int> _inventoryDict = new Dictionary<string, int>();

    // 설치아이템을 겹쳐서 설치하지 못하게 하기 위해 위치를 캐싱해놓음
    private List<Vector3> _installedItemPosition = new List<Vector3>();


    // 슬롯 카운터 이미지
    [SerializeField]
    public List<Sprite> _itemCountImages = new List<Sprite>();







    // 드래깅용 변수
    [SerializeField] RectTransform _dragUI;
    private InventoryItemSlot      _startSlot;
    private ItemData               _startSlotItemData;
    private int                    _startSlotItemCount;
    private bool                   _isDragging = false;


    // 스크롤용 변수
    private int  _focussedIndex = 0;
    private bool _scrollable    = true;


    // 플레이어 트랜스폼 캐싱
    private Transform _playerTransform;


    void Awake()
    {
        SingletonInitialize();
    }

    void Start()
    {
        // 자식으로 아이템 슬롯을 생성
        for(int i = 0; i < _maxInventorySize; ++i)
        {
            GameObject go = Instantiate(ItemSlotPrefab, transform);

            _inventorySlot.Add(go.GetComponent<InventoryItemSlot>());

            //if(i == 0)
            //{
            //    go.GetComponent<InventoryItemSlot>().Select();
            //}
        }

        _dragUI.gameObject.SetActive(false);
        _dragUI.GetComponent<Image>().raycastTarget = false;
    }

    void Update()
    {
        DragAndDropItem();
        //InventoryScroll();
    }

    public Dictionary<string, int> GetInventoryDict()
    {
        return _inventoryDict;
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
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    DropItemToField();
                }
            }
        }
    }

    private void InventoryScroll()
    {
        if(_scrollable == true)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0)
            {
                _focussedIndex -= (int)Mathf.Clamp(scroll * 10, -1f, 1f);

                if (_focussedIndex < 0)
                {
                    _focussedIndex = _maxInventorySize - 1;
                }
                else if (_focussedIndex >= _maxInventorySize)
                {
                    _focussedIndex = 0;
                }

                for (int i = 0; i < _maxInventorySize; ++i)
                {
                    if (i == _focussedIndex)
                    {
                        _inventorySlot[i].Select();
                    }
                    else
                    {
                        _inventorySlot[i].Unselect();
                    }
                }
            }
        }
    }

    public void UseSelectedItem()
    {
        _inventorySlot[_focussedIndex].UseItem();
    }

    public void DisableScrollToggle()
    {
        _scrollable = !_scrollable;
    }

    public void CachingPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    public void RemoveItem(string itemName, int itemCount = 1)
    {
        int count = itemCount;

        _inventoryDict[itemName] -= itemCount;

        foreach(var item in _inventorySlot)
        {
            if(item.GetItemName() == itemName)
            {
                count = item.RemoveItemData(count);

                if(count == 0)
                {
                    break;
                }
            }
        }
    }

    public bool AddItem(ItemData itemData)
    {
        //if(itemData as ToolItemData)
        //{
        //    for (int i = 0; i < _maxInventorySize; ++i)
        //    {
        //        // 슬롯이 비어있다면
        //        if (_inventorySlot[i].IsEmpty() == true)
        //        {
        //            AddItemToSlot(itemData, i);
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        //else
        //{
            for (int i = 0; i < _maxInventorySize; ++i)
            {
                // 슬롯이 비어있다면
                if (_inventorySlot[i].IsEmpty() == true)
                {
                    // 다른 슬롯에 같은 아이템이 있는가
                    if (_inventoryDict.TryGetValue(itemData.Name, out int count) && count != 0)
                    {
                        // 있으면 그 슬롯까지 계속 감
                        continue;
                    }
                    else
                    {
                        // 아이템 추가
                        AddItemToSlot(itemData, i);

                        return true;
                    }
                }
                // 슬롯이 비어있지 않다면
                else
                {
                    // 같은 아이템인가
                    if (_inventorySlot[i].GetItemName() == itemData.Name)
                    {
                        // 같은 아이템이지만 꽉 차있는가?
                        if (_inventorySlot[i].IsFull() == true)
                        {
                            // 처음부터 다시 돌아서 빈 칸에 추가함
                            if (SearchFromFirstSlot(itemData, out int slot) == true && slot >= 0)
                            {
                                AddItemToSlot(itemData, slot);

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
                            AddItemToSlot(itemData, i);

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
        for(int i = 0; i < _maxInventorySize; ++i)
        {
            // 슬롯이 비어있다면
            if (_inventorySlot[i].IsEmpty() == true)
            {
                value = i;
                return true;
            }
            // 비어있지는 않은데
            else
            {
                if (_inventorySlot[i].IsFull() == false && _inventorySlot[i].GetItemData(out int itemCount) == itemData)
                {
                    value = i;
                    return true;
                }
            }
        }

        value = -1;
        return false;
    }

    private void AddItemToSlot(ItemData itemData, int slot)
    {
        _inventorySlot[slot].AddItemData(itemData);

        if (_inventoryDict.ContainsKey(itemData.Name))
        {
            _inventoryDict[itemData.Name]++;
        }
        else
        {
            _inventoryDict[itemData.Name] = 1;
        }
    }

    public bool IsDragging()
    {
        return _isDragging;
    }

    public void BeginDragData(InventoryItemSlot startSlot)
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
            _startSlot          = startSlot;
            _startSlotItemData  = startSlot.GetItemData(out int itemCount);
            _startSlotItemCount = itemCount;
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

    public void EndDragData(InventoryItemSlot endSlot)
    {
        // 비어 있어야
        if (endSlot.IsEmpty() == true)
        {
            endSlot.AddItemData(_startSlotItemData, _startSlotItemCount);
        }
        // 비어있지 않다면
        else
        {
            // 1. 비어있지 않았는데 양 슬롯에 있던 아이템이 같고 재료 아이템이였다면
            if(_startSlotItemData.Name == endSlot.GetItemName() && _startSlotItemData.ItemType == ItemType.Resource)
            {
                ItemData endSlotItemData  = endSlot.GetItemData(out int itemCount);
                int      endSlotItemCount = itemCount;

                // 합치려면 양쪽 아이템 갯수를 더했을 때
                // 인벤토리 최대 적재량 ( 9개 ) 과 같다면
                if(_startSlotItemCount + endSlotItemCount <= 9)
                {
                    endSlot.AddItemData(_startSlotItemData, _startSlotItemCount);
                    _startSlot.ClearItemSlot();
                }
                // 인벤토리 최대 적재량 보다 크다면
                else
                {
                    endSlot.AddItemData(_startSlotItemData, Mathf.Abs(_startSlotItemCount - endSlotItemCount));
                    _startSlot.AddItemData(_startSlotItemData, Mathf.Abs(9 - (_startSlotItemCount - endSlotItemCount)));
                }
            }
            // 2. 비어있지 않았는데 서로 아이템이 다른 경우 (재료든 장비든 뭐든)
            else
            {
                // 데이터 스왑
                ItemData endSlotItemData = endSlot.GetItemData(out int itemCount);
                endSlot.ClearItemSlot();

                endSlot.AddItemData(_startSlotItemData, _startSlotItemCount);

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

    private void DropItemToField()
    {
        // 아이템 생성, 데이터 이전
        {
            for(int i = 0; i < _startSlotItemCount; ++i)
            {
                Vector3 position = adjustDropItemDistance();

                GameObject go = Instantiate(_startSlotItemData.Prefab, position, Quaternion.identity);

                go.GetComponent<Item>().SetItemData(_startSlotItemData);

                //Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                //
                //go.GetComponent<Item>().Spread(GameManager.Instance.GetPlayerPos(), dir, Random.Range(2.5f, 3f));
            }
        }
        // 드래그 UI 비활성화
        {
            _dragUI.gameObject.SetActive(false);
        }
        // 캐시데이터, 출발지점, 데이터  초기화
        {
            _inventoryDict[_startSlotItemData.Name] -= _startSlotItemCount;

            _startSlot.ClearItemSlot();
            _startSlot          = null;
            _startSlotItemData  = null;
            _startSlotItemCount = 0;
        }
        // 드래그 플래그
        {
            _isDragging = false;
        }
    }

    private Vector3 adjustDropItemDistance()
    {
        Vector3 randomPos = new Vector3(GameManager.Instance.GetPlayerPos().x - Random.Range(-2f, 2f), GameManager.Instance.GetPlayerPos().y - Random.Range(-2f, 2f), GameManager.Instance.GetPlayerPos().z - Random.Range(-2f, 2f));

        while(Vector3.Distance(GameManager.Instance.GetPlayerPos(), randomPos) > 2)
        {
            randomPos = new Vector3(GameManager.Instance.GetPlayerPos().x - Random.Range(-2f, 2f), GameManager.Instance.GetPlayerPos().y - Random.Range(-2f, 2f), GameManager.Instance.GetPlayerPos().z - Random.Range(-2f, 2f));
        }

        return randomPos;
    }

    public ItemData ExchangeEquipItem(ItemData itemData, EquipmentSlot slot)
    {
        _inventoryDict[itemData.Name] -= 1;

        // 받아 온 장비를 장착하고 장착하고 있던 장비는 가져옴
        ItemData equipedItemData = EquipmentManager.Instance.EquipItem(itemData, slot);

        // 장착하고 있던 장비가 없었다면
        if (equipedItemData == null) 
        {
            return null;
        }
        // 장착하고 있던 장비가 있다면
        else
        {
            _inventoryDict[equipedItemData.Name] += 1;

            return equipedItemData;
        }
    }

    public void EatItem(EdibleItemData edibleItemData)
    {
        _inventoryDict[edibleItemData.Name] -= 1;

        edibleItemData.Execute();
    }

    public bool InstallItem(InstallableItemData installableItemData)
    {
        float installDistance = 1f;

        Vector3 right = new Vector3(GameManager.Instance.PlayerTransform.localScale.x, 0f, 0f);

        Vector3 position = GameManager.Instance.PlayerTransform.position + installDistance * right;

        if(avoidOverlap(position) == false)
        {
            // 설치하려는 위치에 이미 아이템이 설치되어 있음
            // TODO : 

            return false;
        }
        else
        {
            _inventoryDict[installableItemData.Name] -= 1;

            _installedItemPosition.Add(position);

            GameObject go = Instantiate(installableItemData.Prefab, position, Quaternion.identity);

            go.GetComponent<Item>().SetItemData(installableItemData);

            return true;
        }
    }

    private bool avoidOverlap(Vector3 position)
    {
        // 이 방법은
        // 설치가 되었다가 회수가 되었을 때
        // 해당 위치가 계속 컨테이너에 남아있기 때문에 오류가 발생함

        float delta = 1f;

        foreach(var installedItemPosition in _installedItemPosition)
        {
            if (installedItemPosition.x + delta >= position.x &&
                installedItemPosition.x - delta <= position.x &&
                installedItemPosition.y + delta >= position.y &&
                installedItemPosition.y - delta <= position.y    )
            {
                return false;
            }
        }

        return true;
    }
}
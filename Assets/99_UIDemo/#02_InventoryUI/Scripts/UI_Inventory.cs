using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UI_ItemSlot;

// 인벤토리 UI
public class UI_Inventory : MonoBehaviour
{
    #region SINGLETON
    private static UI_Inventory instance;
    public  static UI_Inventory Instance
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
    int _initialInventorySize = 15;

    // 아이템 슬롯을 담아놓을 컨테이너
    List<UI_ItemSlot> _inventorySlot = new List<UI_ItemSlot>();

    // 슬롯에 뭐가 들어가 있는지 확인하기 위한 컨테이너
    private Dictionary<string, int> _inventoryDict = new Dictionary<string, int>();



    // 드래깅용 변수
    [SerializeField] RectTransform _dragUI;

    private UI_ItemSlot  _startSlot;
    private ItemSlotData _startSlotItemData;

    private bool _isDragging = false;


    // 플레이어 트랜스폼 캐싱
    private Transform _playerTransform;


    private void Awake()
    {
        SingletonInitialize();
    }

    void Start()
    {
        // 자식으로 아이템 슬롯을 생성
        for(int i = 0; i < _initialInventorySize; ++i)
        {
            GameObject go = Instantiate(ItemSlotPrefab, transform);

            _inventorySlot.Add(go.GetComponent<UI_ItemSlot>());
        }

        _dragUI.gameObject.SetActive(false);
        _dragUI.GetComponent<Image>().raycastTarget = false;
    }

    void Update()
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

    public void CachingPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    public bool AddItem(ItemData itemData)
    {
        for(int i = 0; i < _initialInventorySize; ++i)
        {
            // 슬롯이 비어있다면
            if(_inventorySlot[i].IsEmpty() == true)
            {
                // 다른 슬롯에 같은 아이템이 있는가
                if(_inventoryDict.TryGetValue(itemData.Name, out int count) && count != 0)
                {
                    // 있으면 그 슬롯까지 계속 감
                    continue;
                }
                else
                {
                    // 아이템 추가
                    _inventorySlot[i].AddItemData(itemData);
                    //_inventoryDict.Add(itemData.Name);
                    
                    if(_inventoryDict.ContainsKey(itemData.Name))
                    {
                        _inventoryDict[itemData.Name]++;
                    }
                    else
                    {
                        _inventoryDict[itemData.Name] = 1;
                    }

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
                        addItem(itemData);

                        if (_inventoryDict.ContainsKey(itemData.Name))
                        {
                            _inventoryDict[itemData.Name]++;
                        }
                        else
                        {
                            _inventoryDict[itemData.Name] = 1;
                        }

                        return true;
                    }
                    else
                    {
                        // 아이템 추가
                        _inventorySlot[i].AddItemData(itemData);
                        //_inventoryDict.Add(itemData.Name);

                        if (_inventoryDict.ContainsKey(itemData.Name))
                        {
                            _inventoryDict[itemData.Name]++;
                        }
                        else
                        {
                            _inventoryDict[itemData.Name] = 1;
                        }

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
    }

    private void addItem(ItemData itemData)
    {
        foreach(var slot in _inventorySlot)
        {
            if(slot.IsEmpty() == true)
            {
                slot.AddItemData(itemData);
                break;
            }
        }
    }

    public bool IsDragging()
    {
        return _isDragging;
    }

    public void BeginDragData(UI_ItemSlot startSlot)
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
            _startSlotItemData = startSlot.GetItemSlotData();
        }
        // 드래그 UI 활성화
        {
            _dragUI.gameObject.SetActive(true);
            _dragUI.GetComponent<Image>().sprite = _startSlotItemData.ItemData.Image;
        }
        // 슬롯 초기화
        {
            startSlot.ClearItemSlotData();
        }
    }

    public void EndDragData(UI_ItemSlot endSlot)
    {
        // 비어 있어야
        if (endSlot.IsEmpty() == true)
        {
            endSlot.SetItemSlotData(_startSlotItemData);
        }
        // 비어있지 않다면
        else
        {
            // 데이터 스왑
            ItemSlotData endSlotItemData = endSlot.GetItemSlotData();

            endSlot.SetItemSlotData(_startSlotItemData);

            _startSlot.SetItemSlotData(endSlotItemData);
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
            for(int i = 0; i < _startSlotItemData.ItemCount; ++i)
            {
                Vector3 position = adjustDropItemDistance();

                GameObject go = Instantiate(_startSlotItemData.ItemData.Prefab, position, Quaternion.identity);

                go.GetComponent<Item>().SetItemData(_startSlotItemData.ItemData);
            }
        }
        // 드래그 UI 비활성화
        {
            _dragUI.gameObject.SetActive(false);
        }
        // 캐시데이터, 출발지점, 데이터  초기화
        {
            _inventoryDict[_startSlotItemData.ItemData.Name] -= _startSlotItemData.ItemCount;

            _startSlot = null;
            _startSlotItemData.ItemData  = null;
            _startSlotItemData.ItemCount = 0;
        }
        // 드래그 플래그
        {
            _isDragging = false;
        }
    }

    private Vector3 adjustDropItemDistance()
    {
        Vector3 randomPos = new Vector3(_playerTransform.position.x - Random.Range(-2f, 2f), _playerTransform.position.y - Random.Range(-2f, 2f), _playerTransform.position.z - Random.Range(-2f, 2f));

        while(Vector3.Distance(_playerTransform.position, randomPos) > 2)
        {
            randomPos = new Vector3(_playerTransform.position.x - Random.Range(-2f, 2f), _playerTransform.position.y - Random.Range(-2f, 2f), _playerTransform.position.z - Random.Range(-2f, 2f));
        }

        return randomPos;
    }
}
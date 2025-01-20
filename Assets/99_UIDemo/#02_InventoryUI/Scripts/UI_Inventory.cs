using System.Collections.Generic;
using UnityEngine;

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
    }

    // 아이템 캐싱용 컨테이너
    private HashSet<string> _inventoryDict = new HashSet<string>();

    public bool AddItem(ItemData itemData)
    {
        bool isFull = false;

        for(int i = 0; i < _initialInventorySize; ++i)
        {
            // 슬롯이 비어있다면
            if(_inventorySlot[i].IsEmpty() == true)
            {
                // 다른 슬롯에 같은 아이템이 있는가
                if(_inventoryDict.Contains(itemData.Name) == true)
                {
                    // 그 슬롯까지 갔는데 꽉 차 있어서
                    // 처음부터 다시 돌아서 빈 칸 찾아옴
                    if(isFull == true)
                    {
                        _inventorySlot[i].AddItemData(itemData);
                        _inventoryDict.Add(itemData.Name);

                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    _inventorySlot[i].AddItemData(itemData);
                    _inventoryDict.Add(itemData.Name);

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
                        isFull = true;
                        continue;
                    }
                    else
                    {
                        _inventorySlot[i].AddItemData(itemData);
                        _inventoryDict.Add(itemData.Name);

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

        return false;
    }

    public Item GetItem()
    {
        return null;
    }
}
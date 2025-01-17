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
    List<UI_ItemSlot> _inventory = new List<UI_ItemSlot>();

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

            go.name = "ItemSlot_" + i;

            _inventory.Add(go.GetComponent<UI_ItemSlot>());
        }
    }

    void Update()
    {

    }

    public bool AddItem(ItemData item)
    {
        // 아이템 슬롯을 순회하면서
        foreach (UI_ItemSlot slot in _inventory)
        {
            // 슬롯이 비어있다면
            if(slot.IsEmpty() == true)
            {
                // 슬롯에 아이템을 추가
                slot.AddItem(item);
                return true;
            }
            // 슬롯이 비어있지 않다면
            else
            {
                //// 주운 아이템과 같다면 && 그 아이템이 최대갯수보다 적다면
                //if (slot.GetItemName() == item.name && slot.IsFull() == false)
                //{
                //    slot.AddItem(item);
                //    return true;
                //}
            }
        }

        // 이미 들고 있는 것도 없고, 인벤토리도 가득 참
        return false;
    }

    public Item GetItem()
    {
        return null;
    }
}
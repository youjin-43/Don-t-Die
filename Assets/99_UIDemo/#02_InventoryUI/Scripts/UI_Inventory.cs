using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private static UI_Inventory instance;
    public  static UI_Inventory Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
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

    [SerializeField] Transform  QuickSlotArea;
    [SerializeField] GameObject ItemSlotPrefab;

    // 이거 바꿀거면 UI 크기 수동으로 설정해줘야 함
    int _initialInventorySize = 15;

    List<UI_ItemSlot> _inventory = new List<UI_ItemSlot>();

    int _focusIndex = 0;

    void Start()
    {
        for(int i = 0; i < _initialInventorySize; ++i)
        {
            GameObject go = Instantiate(ItemSlotPrefab, QuickSlotArea);

            _inventory.Add(go.GetComponent<UI_ItemSlot>());
        }

    }

    void Update()
    {
        ItemFocusing();
    }

    void ItemFocusing()
    {
        // 앞으로 굴리기 = +Y
        // 뒤로   굴리기 = -Y

        Vector2 mouseWheelDelta = Input.mouseScrollDelta;

        if(mouseWheelDelta.y != 0)
        {
            _focusIndex -= (int)mouseWheelDelta.y;
        }
        if(_focusIndex < 0)
        {
            _focusIndex += _initialInventorySize;
        }
        if(_focusIndex >= 15)
        {
            _focusIndex -= _initialInventorySize;
        }

        _inventory[_focusIndex].Activate();

        for(int i = 0; i < _initialInventorySize; ++i)
        {
            if(i == _focusIndex)
            {
                _inventory[_focusIndex].Activate();
            }
            else
            {
                _inventory[i].Deactivate();
            }
        }
    }

    public bool AddItem(Item item)
    {
        foreach (UI_ItemSlot slot in _inventory)
        {
            // 슬롯이 비어있지 않다면
            if(slot.IsEmpty() == false)
            {
                // 슬롯에 들어가 있는 아이템이 주운 아이템이랑 같고, 64개 미만이라면
                if(slot.GetHavingItemName() == item.name && slot.IsFull() == false)
                {
                    slot.AddItem(item);
                    return true;
                }
            }
            else
            {
                // 슬롯이 비어있다면
                slot.AddItem(item);
                return true;
            }
        }

        // 이미 들고 있는 것도 없고, 인벤토리도 가득 참
        return false;
    }
    public Item GetItem()
    {
        if (_inventory[_focusIndex].IsEmpty() == false)
        {
            return _inventory[_focusIndex].GetItem();
        }

        return null;
    }
}
using System;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region SINGLETON
    private static EquipmentManager instance;
    public  static EquipmentManager Instance
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

    // 장비창이 들고 있을 장비 슬롯 프리팹
    [SerializeField] GameObject EquipmentSlotPrefab;

    // 장비 슬롯 최대 갯수
    int _initialEquipmentSize = 3;

    // 장비 슬롯을 담아놓을 변수들
    EquipmentItemSlot _toolItemSlot;
    EquipmentItemSlot _headItemSlot;
    EquipmentItemSlot _chestItemSlot;

    private void Awake()
    {
        SingletonInitialize();
    }

    private void Start()
    {
        // 도구 슬롯
        {
            _toolItemSlot = Instantiate(EquipmentSlotPrefab, transform).GetComponent<EquipmentItemSlot>();
        }
        // 투구 슬롯
        {
            _headItemSlot = Instantiate(EquipmentSlotPrefab, transform).GetComponent<EquipmentItemSlot>();
        }
        // 갑옷 슬롯
        {
            _chestItemSlot = Instantiate(EquipmentSlotPrefab, transform).GetComponent<EquipmentItemSlot>();
        }
    }

    /// <summary>
    /// 장비창에 있는 장비를 가져오는 함수
    /// </summary>
    public ToolItemData GetCurrentTool()
    {
        return _toolItemSlot?.GetItemData() as ToolItemData;
    }

    /// <summary>
    /// 장비창에 있는 투구를 가져오는 함수
    /// </summary>
    public HeadItemData GetCurrentHead()
    {
        return _headItemSlot?.GetItemData() as HeadItemData;
    }

    /// <summary>
    /// 장비창에 있는 갑옷을 가져오는 함수
    /// </summary>
    public ChestItemData GetCurrentChest()
    {
        return _chestItemSlot?.GetItemData() as ChestItemData;
    }

    public void ReduceToolDurability()
    {
        _toolItemSlot._currentDurability--;

        if (_toolItemSlot._currentDurability <= 0)
        {
            _toolItemSlot.ClearEquipment(true);     // 착용한 툴 없애기
        }
        _toolItemSlot.UpdateDurabilityGaugeUI();
    }

    // 장비 변경 이벤트 정의
    public event Action<ItemData, EquipmentSlot> OnEquipChanged;

    public ItemData EquipItem(ItemData itemData, EquipmentSlot slot)
    {
        ItemData equippedItemData = null;

        switch (slot)
        {
            case EquipmentSlot.Hand:
                {
                    if(_toolItemSlot != null)
                    {
                        equippedItemData = _toolItemSlot.GetItemData();
                    }

                    _toolItemSlot.AddItemData(itemData);

                    break;
                }
            case EquipmentSlot.Head:
                {
                    if (_headItemSlot != null)
                    {
                        equippedItemData = _headItemSlot.GetItemData();
                    }

                    _headItemSlot.AddItemData(itemData);

                    break;
                }

            case EquipmentSlot.Chest:
                {
                    if (_chestItemSlot != null)
                    {
                        equippedItemData = _chestItemSlot.GetItemData();
                    }

                    _chestItemSlot.AddItemData(itemData);

                    break;
                }
        }
        OnEquipChanged?.Invoke(itemData, slot); // 장비가 변경되었음을 알리는 이벤트 호출
        return equippedItemData;
    }

    public void InvokeOnEquipChanged(ItemData itemData, EquipmentSlot slot)
    {
        OnEquipChanged?.Invoke(itemData, slot);
    }
}

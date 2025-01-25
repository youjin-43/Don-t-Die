using System.Collections.Generic;
using UnityEngine;
using static InventoryItemSlot;

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

    // 장비 슬롯을 담아놓을 컨테이너
    Dictionary<string, EquipmentItemSlot> _equipmentSlot = new Dictionary<string, EquipmentItemSlot>();

    private void Awake()
    {
        SingletonInitialize();
    }

    private void Start()
    {
        // 도구 슬롯
        {
            GameObject go = Instantiate(EquipmentSlotPrefab, transform);

            _equipmentSlot.Add("Hand", go.GetComponent<EquipmentItemSlot>());
        }
        // 투구 슬롯
        {
            GameObject go = Instantiate(EquipmentSlotPrefab, transform);

            _equipmentSlot.Add("Head", go.GetComponent<EquipmentItemSlot>());
        }
        // 갑옷 슬롯
        {
            GameObject go = Instantiate(EquipmentSlotPrefab, transform);

            _equipmentSlot.Add("Chest", go.GetComponent<EquipmentItemSlot>());
        }
    }

    /// <summary>
    /// 장비창에 있는 장비를 가져오는 함수
    /// </summary>
    /// <param name="parts">Head, Hand, Chest</param>
    public ToolItemData GetCurrentTool(string parts)
    {
        if(_equipmentSlot[parts] != null)
        {
            return _equipmentSlot[parts].GetItemData() as ToolItemData;
        }

        return null;
    }

    public ItemData EquipItem(ItemData itemData, EquipmentSlot slot)
    {
        string key = "";

        switch (slot)
        {
            case EquipmentSlot.Hand:
                key = "Hand";
                break;

            case EquipmentSlot.Head:
                key = "Head";
                break;

            case EquipmentSlot.Chest:
                key = "Chest";
                break;
        }

        if(_equipmentSlot[key].IsEmpty() == true)
        {
            _equipmentSlot[key].AddItemData(itemData);

            return null;
        }
        // 이미 장비하고 있다면
        else
        {
            ItemData equippedItemData = _equipmentSlot[key].GetItemData();

            _equipmentSlot[key].AddItemData(itemData);

            return equippedItemData;
        }
    }
}

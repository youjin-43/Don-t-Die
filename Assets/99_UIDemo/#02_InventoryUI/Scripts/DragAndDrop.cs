using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UI_ItemSlot;

public class DragAndDrop : MonoBehaviour
{
    #region SINGLETON
    private static DragAndDrop instance;
    public  static DragAndDrop Instance
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

    [SerializeField] RectTransform _dragUI;

    private UI_ItemSlot  _startSlot;
    private ItemSlotData _startSlotItemData;

    bool _isDragging = false;

    private void Awake()
    {
        SingletonInitialize();
    }

    void Start()
    {
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
        }
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
            _startSlot         = startSlot;
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
}

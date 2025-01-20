using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] RectTransform _dragItemUI;

    private UI_ItemSlot _originalSlot;
    private ItemData    _draggedItemData;
    private int         _draggedItemCount;

    bool _isDragging = false;

    private void Awake()
    {
        SingletonInitialize();
    }

    void Start()
    {
        _dragItemUI.gameObject.SetActive(false);
        _dragItemUI.GetComponent<Image>().raycastTarget = false;
    }

    void Update()
    {
        // 드래그 중 이라면
        if (_isDragging == true)
        {
            // 데이터를 옮겨 실은 UI는 마우스 위치를 따라감
            _dragItemUI.position = Input.mousePosition;
        }
    }

    public void BeginDrag(UI_ItemSlot slot)
    {
        // 빈 슬롯을 클릭했다면
        if(slot.IsEmpty() == true)
        {
            return;
        }

        // 드래그 플래그 ON
        _isDragging = true;

        // 출발 슬롯
        _originalSlot = slot;

        // 아이탬 수
        _draggedItemCount = slot.GetItemCount();

        // 옮길 데이터를 담을 변수
        _draggedItemData = slot.GetItemData();

        // 옮겨 실을 UI 활성화
        _dragItemUI.gameObject.SetActive(true);

        // 옮겨 실을 UI 이미지 설정
        _dragItemUI.GetComponent<Image>().sprite = _draggedItemData.Image;

        // 출발 슬롯 클리어
        slot.ClearSlot();
    }

    public void EndDrag(UI_ItemSlot dropSlot)
    {
        // 비어 있어야
        if(dropSlot.IsEmpty() == true)
        {
            // 도착 슬롯에 데이터 추가
            dropSlot.AddItemData(_draggedItemData);
            dropSlot.SetItemCount(_draggedItemCount);
        }
        // 비어있지 않다면
        else
        {
            // 도착 슬롯의 데이터를
            ItemData dropSlotItemData  = dropSlot.GetItemData();
            int      dropslotItemCount = dropSlot.GetItemCount();

            // 출발 슬롯의 데이터로 바꾸고
            dropSlot.AddItemData (_draggedItemData);
            dropSlot.SetItemCount(_draggedItemCount);

            // 출발 슬롯의 데이터를 도착슬롯의 데이터로 바꿈
            _originalSlot.AddItemData (dropSlotItemData);
            _originalSlot.SetItemCount(dropslotItemCount);
        }

        // 옮겨 실어 온 데이터를 담은 변수 초기화
        _draggedItemData = null;

        // 옮겨 실어 온 UI 비활성화
        _dragItemUI.gameObject.SetActive(false);

        // 드래그 플래그 OFF
        _isDragging = false;
    }
}

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
                                          
public class UI_ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // 아이템 데이터
    public struct ItemSlotData
    {
        public ItemData ItemData;
        public int      ItemCount;
    }
    private ItemSlotData _itemSlotData;

    // 슬롯 데이터
    private Image           _itemImage;
    private TextMeshProUGUI _itemCountText;
    private int             _maxItemCount  = 3;

    // 드래깅 데이터
    static bool             _isDragging    = false;

    private void Awake()
    {
        _itemImage     = transform.GetChild(0).GetComponent<Image>();
        _itemCountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        _itemCountText.gameObject.SetActive(false);
    }

    // IPointerClickHandler 인터페이스
    public void OnPointerClick(PointerEventData eventData)
    {
        // 좌클릭 이벤트 (아이템 이동)
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 오작동 방지
            if (IsEmpty() == true && _isDragging == false)
            {
                return;
            }

            if (_isDragging == false)
            {
                _isDragging = true;

                _itemCountText.gameObject.SetActive(false);

                DragAndDrop.Instance.BeginDragData(this);
            }
            else
            {
                _isDragging = false;

                DragAndDrop.Instance.EndDragData(this);
            }
        }
        // 우클릭 이벤트 (아이템 사용)
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 오작동 방지
            if (IsEmpty() == true)
            {
                return;
            }

            if (_itemSlotData.ItemData == null)
            {
                DebugController.Log("아이템이 없어요");
            }
            else
            {
                UseItem();
            }
        }
    }

    public bool AddItemData(ItemData itemData)
    {
        // 슬롯에 최초로 아이템이 들어 온 경우
        if(_itemSlotData.ItemData == null)
        {
            _itemImage.sprite = itemData.Image;
            _itemCountText.gameObject.SetActive(true);
        }
        // 아이템 추가
        {
            _itemSlotData.ItemData   = itemData;
            _itemSlotData.ItemCount += 1;
        }
        // 아이템 갯수 UI 업데이트
        {
            _itemCountText.text = _itemSlotData.ItemCount.ToString();
        }

        return true;
    }

    public ItemData GetItemData()
    {
        _itemSlotData.ItemCount -= 1;

        return _itemSlotData.ItemData;
    }

    public string GetItemName()
    {
        return _itemSlotData.ItemData.Name;
    }

    public void UseItem()
    {
        DebugController.Log("아이템 사용");
    }

    public bool IsEmpty()
    {
        return _itemSlotData.ItemData == null;
    }

    public bool IsFull()
    {
        return _itemSlotData.ItemCount == _maxItemCount;
    }

    /// <summary>
    /// 인벤토리 드래깅용
    /// </summary>
    public ItemSlotData GetItemSlotData()
    {
        return _itemSlotData;
    }

    /// <summary>
    /// 인벤토리 드래깅용
    /// </summary>
    public void SetItemSlotData(ItemSlotData itemSlotData)
    {
        // 데이터 초기화
        {
            _itemSlotData = itemSlotData;
        }
        // 이미지 초기화
        {
            _itemImage.sprite = itemSlotData.ItemData.Image;
        }
        // 카운트 초기화
        {
            _itemCountText.text = itemSlotData.ItemCount.ToString();
            _itemCountText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 인벤토리 드래깅용
    /// </summary>
    public void ClearItemSlotData()
    {
        // 데이터 초기화
        {
            _itemSlotData.ItemData = null;
            _itemSlotData.ItemCount = 0;
        }
        // 이미지 초기화
        {
            _itemImage.sprite = null;
        }
        // 카운트 초기화
        {
            _itemCountText.text = "0";
            _itemCountText.gameObject.SetActive(false);
        }
    }
}

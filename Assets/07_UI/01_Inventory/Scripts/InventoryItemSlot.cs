using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
                                          
public class InventoryItemSlot : MonoBehaviour, IPointerClickHandler
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
    private Image           _itemSelectImage;
    private TextMeshProUGUI _itemCountText;
    private int             _maxItemCount  = 9;

    // 드래깅 데이터
    static bool             _isDragging    = false;

    private void Awake()
    {
        _itemImage           = transform.GetChild(0).GetComponent<Image>();
        _itemSelectImage     = transform.GetChild(1).GetComponent<Image>();
        _itemCountText       = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        _itemImage.color = new Color(1, 1, 1, 0);
        _itemCountText.gameObject.SetActive(false);
        _itemSelectImage.gameObject.SetActive(false);
    }

    // IPointerClickHandler 인터페이스
    public void OnPointerClick(PointerEventData eventData)
    {
        // 좌클릭 이벤트 (아이템 이동)
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 오작동 방지
            if (IsEmpty() == true && InventoryManager.Instance.IsDragging() == false)
            {
                return;
            }

            // 아이템 인벤토리 상에서의 이동
            if (InventoryManager.Instance.IsDragging() == false)
            {
                InventoryManager.Instance.BeginDragData(this);
            }
            else
            {
                InventoryManager.Instance.EndDragData(this);
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

    public void Select()
    {
        _itemSelectImage.gameObject.SetActive(true);
    }

    public void Unselect()
    {
        _itemSelectImage.gameObject.SetActive(false);
    }

    public bool AddItemData(ItemData itemData)
    {
        if(itemData as ToolItemData != null)
        {
            // 들어온 아이템은 장비임
            _maxItemCount = 1;
        }
        else
        {
            // 들어온 아이템은 재료임
        }

        // 슬롯에 최초로 아이템이 들어 온 경우
        if(_itemSlotData.ItemData == null)
        {
            _itemImage.sprite = itemData.Image;
            _itemImage.color  = new Color(1, 1, 1, 1);
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
        // 데이터가 ToolItemData 일 경우
        {
            var itemData = _itemSlotData.ItemData as ToolItemData;

            if(itemData != null)
            {
                ItemData equippedItemData = InventoryManager.Instance.ExchangeEquipItem(itemData, itemData.EquipSlot);

                // 장비창이 비어있었다면
                if (equippedItemData == null)
                {
                    ClearItemSlotData();
                }
                // 장비창이 비어있지 않았다면
                else
                {
                    ItemSlotData itemSlotData;
                    {
                        itemSlotData.ItemData = equippedItemData;
                        itemSlotData.ItemCount = 1;
                    }
                    SetItemSlotData(itemSlotData);
                }
            }
        }
        // 데이터가 _____________ 일 경우
        {

        
        }
        // 데이터가 _____________ 일 경우
        {

        }
        // ......
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
            _itemImage.color  = new Color(1, 1, 1, 1);
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
        // 아이템 데이터 초기화
        {
            _itemSlotData.ItemData = null;
            _itemSlotData.ItemCount = 0;
        }
        // 슬롯 데이터 초기화
        {
            _itemImage.sprite = null;
            _itemImage.color  = new Color(1, 1, 1, 0);
            _maxItemCount = 9;
        }
        // 카운트 초기화
        {
            _itemCountText.text = "0";
            _itemCountText.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 인벤토리 드래깅용
    /// </summary>
    public void EndDrag()
    {
        _isDragging = false;
    }

    
}

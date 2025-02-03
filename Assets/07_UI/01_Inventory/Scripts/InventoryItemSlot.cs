using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
                                          
public class InventoryItemSlot : MonoBehaviour, IPointerClickHandler
{
    // 아이템 데이터
    private ItemData _itemData;
    private int      _maxItemCount;

    // 슬롯 데이터
    private Image        _itemImage;
    private Image        _itemCountImage;
    private Transform    _itemDurability;
    private Image        _itemDurabilityGauge;
    private Image        _itemSelectImage;
    private Image        _slotNumberImage;
    private int          _currentItemCount;
    public int          _currentDurability;
    private int          _slotNumber;

    // 드래깅 데이터
    static bool _isDragging = false;

    private void Awake()
    {
        _itemImage           = transform.GetChild(0).GetComponent<Image>();
        _itemCountImage      = transform.GetChild(1).GetComponent<Image>();
        _itemDurability      = transform.GetChild(2);
        _itemDurabilityGauge = _itemDurability.GetChild(1).GetComponent<Image>();
        _itemSelectImage     = transform.GetChild(3).GetComponent<Image>();
        _slotNumberImage     = transform.GetChild(4).GetComponent<Image>();

        _itemImage.color = new Color(1, 1, 1, 0);
        _itemCountImage.gameObject.SetActive(false);
        _itemDurability.gameObject.SetActive(false);
        _itemSelectImage.gameObject.SetActive(false);
        _slotNumberImage.gameObject.SetActive(false);
    }

    public void SetSlotNumber(int slotNumber)
    {
        _slotNumber = slotNumber;

        if(_slotNumber < 10)
        {
            _slotNumberImage.gameObject.SetActive(true);
            _slotNumberImage.sprite = InventoryManager.Instance._itemCountImages[_slotNumber];
        }
    }

    public ItemData GetItemData(out int itenCount)
    {
        itenCount = _currentItemCount;

        return _itemData;
    }

    public string GetItemName()
    {
        return _itemData?.Name;
    }

    public bool IsEmpty()
    {
        return _itemData == null;
    }

    public bool IsFull()
    {
        return _currentItemCount == _maxItemCount;
    }

    public void EndDrag()
    {
        _isDragging = false;
    }

    public void Select()
    {
        _itemSelectImage.gameObject.SetActive(true);
    }

    public void Unselect()
    {
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

            // UI 클릭 플래그

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
            if (IsEmpty() == false)
            {
                UseItem();
            }
        }
    }

    public int RemoveItemData(int itemCount)
    {
        int remainItemCount = _currentItemCount;

        // 다음 슬롯까지 갈 필요가 없이 여깄는거 빼면 끝
        if(_currentItemCount > itemCount)
        {
            _currentItemCount -= itemCount;
            UpdateItemCount();
            return 0;
        }
        // 다음 슬롯까지 갈 필요는 없지만 슬롯이 비어야 하는 경우
        else if (_currentItemCount == itemCount)
        {
            ClearItemSlot();
            return 0;
        }
        // 다음 슬롯까지 탐색해서 지워야 하는 경우
        else
        {
            ClearItemSlot();
            return Math.Abs(remainItemCount- itemCount);
        }
    }

    private void UpdateItemCount()
    {
        // 아이템 스택 표시
        {
            _itemCountImage.gameObject.SetActive(true);
            _itemCountImage.sprite = InventoryManager.Instance._itemCountImages[_currentItemCount];
        }
        // 내구도 표시는 비활성
        {
            _itemDurability.gameObject.SetActive(false);
        }
    }

    public bool AddItemData(ItemData itemData, int itemCount = 1)
    {
        // 종류와 상관없는 공동 작업
        {
            // 1. 아이템 데이터 추가
            {
                _itemData = itemData;
            }
            // 2. 현재 슬롯 스택 증가
            {
                _currentItemCount += itemCount;
            }
            // 3. 최대 스택 설정
            {
                _maxItemCount = itemData.MaxCountSize;
            }
            // 4. 아이템 이미지 교체
            {
                _itemImage.sprite = itemData.Image;
                _itemImage.color  = new Color(1, 1, 1, 1);
            }
        }
        // 아이템 종류에 따른 개별 작업
        {
            switch (itemData.ItemType)
            {
                case ItemType.Resource:
                    AddResourceItem(itemData as ResourceItemData);
                    break;

                case ItemType.Edible:
                    AddEdibleItem(itemData as EdibleItemData);
                    break;

                case ItemType.Equippable:
                    AddEquippableItem(itemData as EquippableItemData);
                    break;
            }
        }

        return true;
    }

    private void AddResourceItem(ResourceItemData toolItemData)
    {
        UpdateItemCount();
    }

    private void AddEdibleItem(EdibleItemData edibleItemData)
    {
        UpdateItemCount();
    }

    private void AddEquippableItem(EquippableItemData equippableItemData)
    {
        // 아이템 내구도 표시
        {
            _itemDurability.gameObject.SetActive(true);

            DebugController.Log($"{_currentDurability}");
            float fillAmount = _currentDurability / equippableItemData.maxDurability;

            _itemDurabilityGauge.fillAmount = fillAmount;
            _itemDurabilityGauge.color = Color.HSVToRGB(fillAmount / 3f, 1f, 1f);
        }
        // 아이템 스택 표시는 비활성
        {
            _itemCountImage.gameObject.SetActive(false);
        }
    }

    public void ClearItemSlot()
    {
        // 1. 아이템 데이터 초기화
        {
            _itemData = null;
        }
        // 2. 현재 슬롯 스택 초기화
        {
            _currentItemCount = 0;
        }
        // 3. 최대 스택 초기화
        {
            _maxItemCount = 9;
        }
        // 4. 아이템 이미지 초기화
        {
            _itemImage.sprite = null;
            _itemImage.color  = new Color(1, 1, 1, 0);
            _itemCountImage.gameObject.SetActive(false);
            _itemDurability.gameObject.SetActive(false);
        }
    }

    public void UseItem()
    {
        if(_itemData == null)
        {
            return;
        }

        if (BoxManager.Instance.IsBoxOpened() == true)
        {
            if (InventoryManager.Instance.SendItemDataToBox(_itemData) == true)
            {
                RemoveItemData(1);
            }
            return;
        }

        // 사용 할 아이템의 종류에 따라 분류
        switch (_itemData.ItemType)
        {
            case ItemType.Resource:
                {
                    break;
                }
            case ItemType.Edible:
                {
                    InventoryManager.Instance.EatItem(_itemData as EdibleItemData);

                    _currentItemCount -= 1;

                    _itemCountImage.sprite = InventoryManager.Instance._itemCountImages[_currentItemCount];

                    if (_currentItemCount == 0)
                    {
                        ClearItemSlot();
                    }

                    break;
                }
            case ItemType.Equippable:
                {
                    EquippableItemData equippableItemData = _itemData as EquippableItemData;

                    // 현재 슬롯의 장비를 장비창으로 넘기고
                    // 장비창에 이미 아이템이 있다면 그 아이템을 받아옴
                    ItemData itemData = InventoryManager.Instance.ExchangeEquipItem(equippableItemData, equippableItemData.EquipSlot);

                    // 장착하고 있던 장비가 없었다면
                    if (itemData == null)
                    {
                        ClearItemSlot();
                    }
                    // 있었다면
                    else
                    {
                        AddItemData(itemData);
                    }

                    break;
                }
            case ItemType.Installable:
                {
                    if (InventoryManager.Instance.InstallItem(_itemData as InstallableItemData) == true)
                    {
                        ClearItemSlot();
                    }
                }
                break;
        }
    }
}

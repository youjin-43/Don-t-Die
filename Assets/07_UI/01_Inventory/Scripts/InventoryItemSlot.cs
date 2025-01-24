using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
                                          
public class InventoryItemSlot : MonoBehaviour, IPointerClickHandler
{
    // 아이템 데이터
    public struct ItemSlotData
    {
        public ItemData ItemData;
        public int      ItemCount;
    }
    private ItemSlotData _itemSlotData;


    // 아이템 데이터
    private ItemData _itemData;

    // 슬롯 데이터
    [SerializeField] 
    private List<Sprite> _itemCountImages = new List<Sprite>();

    private Image _itemImage;
    private Image _itemCountImage;
    private Image _itemDurabilityGuage;
    private Image _itemSelectImage;

    private int _currentItemStack;
    private int _maxItemStack  = 9;


    // 드래깅 데이터
    static bool             _isDragging    = false;

    private void Awake()
    {
        _itemImage           = transform.GetChild(0).GetComponent<Image>();
        _itemCountImage      = transform.GetChild(1).GetComponent<Image>();
        _itemDurabilityGuage = transform.GetChild(2).GetComponent<Image>();
        _itemSelectImage     = transform.GetChild(3).GetComponent<Image>();
        
        _itemImage.color = new Color(1, 1, 1, 0);
        _itemCountImage.     gameObject.SetActive(false);
        _itemDurabilityGuage.gameObject.SetActive(false);
        _itemSelectImage.    gameObject.SetActive(false);
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
            if (IsEmpty() == false)
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
        // 슬롯에 최초로 아이템이 들어 온 경우
        if(_itemSlotData.ItemData == null)
        {
            _itemImage.sprite = itemData.Image;
            _itemImage.color  = new Color(1, 1, 1, 1); 
        }
        // 아이템 추가
        {
            _itemSlotData.ItemData   = itemData;
            _itemSlotData.ItemCount += 1;
        }
        // UI 업데이트
        // 도구 아이템이 들어왔다면

        ToolItemData tooliTemData = itemData as ToolItemData;

        //ToolItemData toolItem = WhatType<ToolItemData>(itemData);


        if (tooliTemData != null)
        {
            _itemDurabilityGuage.gameObject.SetActive(true);
            _maxItemStack = 1;

            //_itemDurabilityGuage.fillAmount = tooliTemData.currentDurability / tooliTemData.maxDurability;
        }
        // 그 이외의 아이템이 들어왔다면
        else
        {
            _itemCountImage.gameObject.SetActive(true);
            _itemCountImage.sprite = _itemCountImages[_itemSlotData.ItemCount];
        }


        return true;

        ///////////////////////////////
        // NEW ////////////////////////
        ///////////////////////////////
        
        // 종류와 상관없는 공동 작업
        {
            // 1. 최초로 들어왔을 경우 아이템 이미지 ON
            _itemImage.sprite = itemData.Image;
            _itemImage.color  = new Color(1, 1, 1, 1);
        }
        // 아이템 종류에 따른 개별 작업
        switch (itemData.ItemType)
        {
            case ItemType.Resource:
                AddResourceItem(itemData as ToolItemData);
                break;

            case ItemType.Equippable:
                AddEquippableItem(itemData as EquippableItemData);
                break;

            case ItemType.Edible:
                AddEdibleItem(itemData as EdibleItemData);
                break;
        }

        return true;
    }

    public void AddResourceItem(ToolItemData toolItemData)
    {
        _itemData     = toolItemData;
        _maxItemStack = toolItemData.MaxStackSize;

    }

    public void AddEquippableItem(EquippableItemData equippableItemData)
    {
        switch (equippableItemData.EquipSlot)
        {
            case EquipmentSlot.Hand:
                _itemData = equippableItemData as ToolItemData;
                break;

            case EquipmentSlot.Head:
                break;

            case EquipmentSlot.Chest:
                break;
        }
    }

    public void AddEdibleItem(EdibleItemData edibleItemData)
    {

    }

    public ItemData GetItemData()
    {
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
        return _itemSlotData.ItemCount == _maxItemStack;
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
        
        // 아이템 종류에 따른 처리


        if(itemSlotData.ItemData as ToolItemData != null)
        {
            _itemDurabilityGuage.gameObject.SetActive(true);
        }
        else
        {
            _itemCountImage.sprite = _itemCountImages[itemSlotData.ItemCount];
            _itemCountImage.gameObject.SetActive(true);
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
            _maxItemStack = 9;
        }
        // 카운트 초기화
        {
            //_itemCountText.text = "0";
            _itemDurabilityGuage.gameObject.SetActive(false);
            _itemCountImage.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 인벤토리 드래깅용
    /// </summary>
    public void EndDrag()
    {
        _isDragging = false;
    }

    public T WhatType<T>(ItemData itemData) where T : ItemData
    {
        if (itemData is T resourceItemData)
        {
            return resourceItemData;
        }
        else if(itemData is T toolItemData)
        {
            return toolItemData;
        }
        else if(itemData is T edibleItemData)
        {
            return edibleItemData; 
        }

        throw new InvalidCastException();
    }
}

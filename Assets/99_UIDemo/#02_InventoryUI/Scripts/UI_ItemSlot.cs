using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
                                          
public class UI_ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // 들고 있는 아이템 이미지
    private Image _itemImage;

    // 아이템 카운트 텍스트
    private TextMeshProUGUI _itemCountText;

    // 아이템 데이터
    private ItemData _itemData;

    // 몇 개 까지?
    private int _currItemCount = 0;
    private int _maxItemCount  = 64;

    // 드래깅 컨트롤용
    static bool _isDragging = false;

    private void Awake()
    {
        _itemImage    = transform.GetChild(0).GetComponent<Image>();
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

                DragAndDrop.Instance.BeginDrag(this);
            }
            else
            {
                _isDragging = false;

                DragAndDrop.Instance.EndDrag(this);
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

            if (_itemData == null)
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
        // 꽉 찼으면
        if(_currItemCount == _maxItemCount)
        {
            return false;
        }

        // 슬롯에 아이템이 처음 들어왔을 때
        if( _itemData == null)
        {
            _itemImage.sprite = itemData.Image;

            _itemCountText.gameObject.SetActive(true);
        }

        _itemData = itemData;

        ++_currItemCount;

        _itemCountText.text = _currItemCount.ToString();

        return true;
    }

    public ItemData GetItemData()
    {
        return _itemData;
    }

    public string GetItemName()
    {
        return _itemData.Name;
    }

    public void UseItem()
    {
        DebugController.Log("아이템 사용");
    }

    public int GetItemCount()
    {
        return _currItemCount;
    }

    public void SetItemCount(int count)
    {
        _currItemCount = count;
    }

    public bool IsEmpty()
    {
        return _itemData == null;
    }

    public bool IsFull()
    {
        return _currItemCount == _maxItemCount;
    }

    public void ClearSlot()
    {
        _itemData         = null;
        _itemImage.sprite = null;
        _currItemCount    = 0;
        _itemCountText.gameObject.SetActive(false);
    }
}

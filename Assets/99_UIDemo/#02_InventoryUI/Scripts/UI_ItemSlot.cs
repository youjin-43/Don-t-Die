using System.Collections.Generic;
using System.Drawing;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
                                          
public class UI_ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // 아이템 아래 그림자 효과
    private GameObject _shade;

    // 들고 있는 아이템 이미지
    private Image      _itemImage;

    // 아이템을 여러개 들고 있기 위한 컨테이너
    //private Queue<ItemData> _items = new Queue<ItemData>();
    private ItemData _item;

    private void Awake()
    {
        _shade     = transform.GetChild(0).gameObject;
        _itemImage = transform.GetChild(1).GetComponent<Image>();

        // 그림자 효과는 처음에는 꺼줌
        _shade.SetActive(false);
    }

    static bool _isDragging = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        // 좌클릭 이벤트 (아이템 이동)
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_isDragging == false)
            {
                _isDragging = true;
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
            if(_item == null)
            {
                DebugController.Log("아이템이 없어용");
            }
            else
            {
                DebugController.Log("아이템 사용");
            }
        }
    }

    public void AddItem(ItemData item)
    {
        // 슬롯이 비어있다면
        //if(IsEmpty() == true)
        //{
        //    // 이미지 바꿔줌
        //    _itemImage.sprite = item.Image;
        //    // 그림자 효과 활성화
        //    _shade.SetActive(true);
        //}

        //_items.Enqueue(item);

        _shade.SetActive (true);
        _itemImage.sprite = item.Image;
        _item = item;
    }

    public ItemData GetItemData()
    {
        //ItemData item = _items.Dequeue();

        //// 아이템이 없다면
        //if(_items.Count == 0)
        //{
        //    // 그림자 효과 비활성화
        //    _shade.SetActive(false);
        //}

        //return item;

        _shade.SetActive(false);

        return _item;
    }

    public string GetItemName()
    {
        //return _items.Peek().name;
        return _item.Name;
    }

    public bool IsEmpty()
    {
       // return _items.Count == 0;
        return _item == null;
    }

    public void ClearSlot()
    {
        _item = null;
        _itemImage.sprite = null;
        _shade.SetActive(false);
    }

    //public bool IsFull()
    //{
    //    return _items.Count == 64;
    //}
}

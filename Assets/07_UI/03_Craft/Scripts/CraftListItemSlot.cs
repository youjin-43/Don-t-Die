using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftListItemSlot : MonoBehaviour, IPointerClickHandler
{
    // 아이템을 그릴 슬롯인지, +, = 모양을 그릴 슬롯인지
    public enum Type
    {
        ItemSlot,
        ResultSlot,
        Image_Equal,
        Image_Plus
    }
    
    private TextMeshProUGUI _currentItemCountText;
    private TextMeshProUGUI _needItemCountText;

    private string _itemName;
    private int    _currentItemCount;
    private int    _needItemCount;
    private Type   _type;

    public string GetItemName()
    {
        return _itemName;
    }

    public void SetData(Type type, string itemName, int needItemCount = 0)
    {
        if(_currentItemCountText == null && _needItemCountText == null)
        {
            _currentItemCountText = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            _needItemCountText    = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        _itemName      = itemName;
        _needItemCount = needItemCount;
        _type          = type;

        // 아이템이 그려질 슬롯이라면
        if (_type == Type.ItemSlot || _type == Type.ResultSlot)
        {
            _needItemCountText.text = needItemCount.ToString();
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
        }

        if (DataManager.Instance.IconImageData.TryGetValue(_itemName, out Sprite sprite))
        {
            transform.GetChild(1).GetComponent<Image>().color  = new Color(1, 1, 1, 1);
            transform.GetChild(1).GetComponent<Image>().sprite = sprite;
        }
    }

    public void ResourceCounting(Dictionary<string, int> inventoryDict)
    {
        if(inventoryDict.TryGetValue(_itemName, out int itemCount) == true)
        {
            _currentItemCount          = itemCount;
            _currentItemCountText.text = itemCount.ToString();
        }
        else
        {
            _currentItemCount          = 0;
            _currentItemCountText.text = 0.ToString();
        }
    }

    public int ResourceCheck()
    {
        return _currentItemCount;
    }

    public void SetCount(int possibleItemCount)
    {
        if(_type == Type.ResultSlot)
        {
            _currentItemCount          = possibleItemCount;
            _currentItemCountText.text = possibleItemCount.ToString();
        }
    }

    public void CraftUnlock()
    {
        if (_type == Type.ResultSlot)
        {

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 좌클릭 이벤트 (아이템 이동)
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DebugController.Log("우클릭이에여");
        }
    }
}
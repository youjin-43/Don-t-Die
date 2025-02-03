using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftListItemSlot : MonoBehaviour, IPointerClickHandler
{
    // 아이템을 그릴 슬롯인지, +, = 모양을 그릴 슬롯인지
    public enum CraftListItemSlotType
    {
        ItemSlot,
        ResultSlot,
        Image_Equal,
        Image_Plus
    }
    
    private TextMeshProUGUI _currentItemCountText;
    private TextMeshProUGUI _needItemCountText;

    private Dictionary<string, int> _recipe = new Dictionary<string, int>();

    private string                _itemName;
    private int                   _currentItemCount;
    private int                   _needItemCount;
    private CraftListItemSlotType _type;
    private bool                  _possibleCraft = false;

    public string GetItemName()
    {
        return _itemName;
    }

    public void SetData(CraftListItemSlotType type, string itemName, int needItemCount = 0)
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
        if (_type == CraftListItemSlotType.ItemSlot || _type == CraftListItemSlotType.ResultSlot)
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

    public void SetRecipe(Dictionary<string, int> recipe)
    {
        _recipe = recipe;
    }

    public void ResourceCounting(Dictionary<string, int> inventoryDict)
    {
        if(inventoryDict.TryGetValue(_itemName, out int itemCount) == true
            && _type != CraftListItemSlotType.ResultSlot)
            // 결과슬롯의 갯수는 인벤토리의 아이템 갯수가 아니라 생성가능한 숫자
        {
            _currentItemCount          = itemCount;
            _currentItemCountText.text = itemCount.ToString();
        }
        else
        {
            _currentItemCount          = 0;
            _currentItemCountText.text = "0";
        }
    }

    public int ResourceCheck()
    {
        return _currentItemCount;
    }

    public void SetCount(int possibleItemCount)
    {
        if(_type == CraftListItemSlotType.ResultSlot)
        {
            _currentItemCount          = possibleItemCount;
            _currentItemCountText.text = possibleItemCount.ToString();
        }
    }

    public void CraftUnlock()
    {
        if (_type == CraftListItemSlotType.ResultSlot)
        {
            _possibleCraft = true;
        }
    }

    public void CraftLock()
    {
        if (_type == CraftListItemSlotType.ResultSlot)
        {
            _possibleCraft = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && _type == CraftListItemSlotType.ResultSlot && _possibleCraft == true)
        {
            ItemData currentItemData = DataManager.Instance.ItemData[_itemName];

            if(InventoryManager.Instance.AddItem(currentItemData) == true)
            {
                foreach(var ingredient in _recipe)
                {
                    InventoryManager.Instance.RemoveItem(ingredient.Key, ingredient.Value);
                }

                CraftManager.Instance.UpdateCraftingUI();
            }
            // 슬롯이 꽉 찼다면
            else
            {

            }
        }
    }
}
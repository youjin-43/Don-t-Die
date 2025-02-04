using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftListItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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
    private GameObject      _description;
    private TextMeshProUGUI _descriptionText;

    private Dictionary<string, int> _recipe = new Dictionary<string, int>();

    private string                _itemName;
    private string                _itemNameKr;
    private int                   _currentItemCount;
    private int                   _needItemCount;
    private CraftListItemSlotType _type;
    private bool                  _possibleCraft = false;

    private bool _isBag = false;

    Coroutine co_doingCraft;

    public string GetItemName()
    {
        return _itemName;
    }

    public void SetData(CraftListItemSlotType type, string itemName, int needItemCount = 0)
    {
        if(_currentItemCountText == null && _needItemCountText == null && _description == null)
        {
            _currentItemCountText = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            _needItemCountText    = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();

            _description     = transform.GetChild(3).GetChild(0).gameObject;
            _descriptionText = _description.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        
            _description.SetActive(false);
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
        if(DataManager.Instance.ItemData.TryGetValue(_itemName, out ItemData itemData))
        {
            _descriptionText.text = itemData.NameKR;
            _itemNameKr = itemData.NameKR;
        }
    }

    public bool IsBag()
    {
        return _isBag;
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
        if (co_doingCraft != null) return;
        if (eventData.button == PointerEventData.InputButton.Right && _type == CraftListItemSlotType.ResultSlot && _possibleCraft == true)
        {
            if (_itemName == "Bag")
            {
                _isBag = true;

                foreach (var ingredient in _recipe)
                {
                    InventoryManager.Instance.RemoveItem(ingredient.Key, ingredient.Value);
                }

                CraftManager.Instance.UpdateCraftingUI();
                GameManager.Instance.IsBagCreated = true;

                return;
            }

            ItemData currentItemData = DataManager.Instance.ItemData[_itemName];
            int maxDurability = 0;

            if (currentItemData is EquippableItemData)
            {
                string[] split = currentItemData.NameKR.Split(' ');

                if (split[1] == "물병")
                {
                    maxDurability = 0;
                }
                else
                {
                    maxDurability = (currentItemData as EquippableItemData).maxDurability;
                }
            }

            co_doingCraft = StartCoroutine(DoingCraftRoutine(currentItemData, maxDurability));
        }
    }

    IEnumerator DoingCraftRoutine(ItemData itemData, int maxDurability)
    {
        PlayerAnimator playerAnimator = GameManager.Instance.PlayerTransform.GetComponent<PlayerAnimator>();
        playerAnimator.TriggerDoingAnimation();

        yield return new WaitForSeconds(0.5f);

        if (InventoryManager.Instance.AddItem(itemData, maxDurability) == true)
        {
            foreach (var ingredient in _recipe)
            {
                InventoryManager.Instance.RemoveItem(ingredient.Key, ingredient.Value);
            }

            CraftManager.Instance.UpdateCraftingUI();
        }
        co_doingCraft = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_itemNameKr != null)
        {
            //_description.transform.SetParent(CraftManager.Instance.ToolTipCanvas);

            // UI를 최상단으로
            //_description.transform.SetAsLastSibling();

            _description.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_itemNameKr != null)
        {
            //_description.transform.SetParent(transform);
            _description.SetActive(false);
        }
    }
}

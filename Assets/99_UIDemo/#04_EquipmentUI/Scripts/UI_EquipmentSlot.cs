using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    // 아이템 데이터
    private ItemData _itemData;

    // 슬롯 데이터
    private Image _itemImage;

    private void Awake()
    {
        _itemImage = transform.GetChild(0).GetComponent<Image>();
    }

    // IPointerClickHandler 인터페이스
    public void OnPointerClick(PointerEventData eventData)
    {
        // 우클릭 이벤트 (아이템 사용)
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_itemData == null)
            {
                DebugController.Log("장비창이 비어있어요");
            }
            else
            {
                ClearEquipment();
            }
        }
    }

    public bool AddItemData(ItemData itemData)
    {
        _itemData         = itemData;
        _itemImage.sprite = itemData.Image;

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

    public void ClearEquipment()
    {
        UI_Inventory.Instance.AddItem(_itemData);

        _itemData         = null;
        _itemImage.sprite = null;
    }

    public bool IsEmpty()
    {
        return _itemData == null;
    }
}

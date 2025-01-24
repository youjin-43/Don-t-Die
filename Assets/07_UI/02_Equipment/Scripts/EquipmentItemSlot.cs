using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentItemSlot : MonoBehaviour, IPointerClickHandler
{
    // 아이템 데이터
    private ItemData _itemData;

    // 슬롯 데이터
    private Image _itemImage;
    private Image _itemDurabilityGauge;

    private void Awake()
    {
        _itemImage           = transform.GetChild(0).GetComponent<Image>();
        _itemDurabilityGauge = transform.GetChild(1).GetChild(1).GetComponent<Image>();

        _itemImage.color = new Color(1, 1, 1, 0);
        _itemDurabilityGauge.gameObject.SetActive(false);
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
        _itemImage.color  = new Color(1, 1, 1, 1);
        _itemDurabilityGauge.gameObject.SetActive(true);


        //ToolItemData toolItemData = itemData as ToolItemData;
        //
        //_itemDurabilityGauge.fillAmount = toolItemData.currentDurability / toolItemData.maxDurability;
        //
        //_itemDurabilityGauge.color = Color.HSVToRGB(_itemDurabilityGauge.fillAmount / 3, 1.0f, 1.0f);

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
        InventoryManager.Instance.AddItem(_itemData);

        _itemData         = null;
        _itemImage.sprite = null;
        _itemImage.color  = new Color(1, 1, 1, 0);
        _itemDurabilityGauge.gameObject.SetActive(false);
    }

    public bool IsEmpty()
    {
        return _itemData == null;
    }
}

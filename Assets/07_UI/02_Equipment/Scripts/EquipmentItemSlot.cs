using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentItemSlot : MonoBehaviour, IPointerClickHandler
{
    // 아이템 데이터
    private ItemData _itemData;

    // 슬롯 데이터
    private Image     _itemImage;
    private Transform _itemDurability;
    private Image     _itemDurabilityGauge;
    public int _currentDurability;

    private void Awake()
    {
        _itemImage           = transform.GetChild(0).GetComponent<Image>();
        _itemDurability      = transform.GetChild(1);
        _itemDurabilityGauge = _itemDurability.GetChild(1).GetComponent<Image>();

        _itemImage.color = new Color(1, 1, 1, 0);
        _itemDurability.gameObject.SetActive(false);
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

    public bool AddItemData(ItemData itemData, int durability)
    {
        _itemData         = itemData;
        _itemImage.sprite = itemData.Image;
        _itemImage.color  = new Color(1, 1, 1, 1);
        _itemDurability.gameObject.SetActive(true);
        _currentDurability = durability;

        EquippableItemData equippableItemData = itemData as EquippableItemData;

        float fillAmount = _currentDurability / (float)equippableItemData.maxDurability;
        DebugController.Log($"{_currentDurability}, {equippableItemData.maxDurability}, {fillAmount}");

        _itemDurabilityGauge.fillAmount = fillAmount;
        _itemDurabilityGauge.color = Color.HSVToRGB(fillAmount / 3, 1.0f, 1.0f);

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

    public void ClearEquipment(bool destroy = false)
    {
        if(destroy || InventoryManager.Instance.AddItem(_itemData) == true)
        {
            _itemData = null;
            _itemImage.sprite = null;
            _itemImage.color = new Color(1, 1, 1, 0);
            _itemDurability.gameObject.SetActive(false);

            EquipmentManager.Instance.InvokeOnEquipChanged(_itemData, EquipmentSlot.Unknown);
        }
    }

    public void UpdateDurabilityGaugeUI()
    {
        EquippableItemData equippableItemData = _itemData as EquippableItemData;
        float fillAmount = _currentDurability / (float) equippableItemData.maxDurability;

        _itemDurabilityGauge.fillAmount = fillAmount;
        _itemDurabilityGauge.color = Color.HSVToRGB(fillAmount / 3, 1.0f, 1.0f);
    }

    public bool IsEmpty()
    {
        return _itemData == null;
    }
}

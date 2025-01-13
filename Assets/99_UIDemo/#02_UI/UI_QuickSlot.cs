using System.Collections.Generic;
using UnityEngine;

public class UI_QuickSlot : MonoBehaviour
{
    [SerializeField] Transform  QuickSlotArea;
    [SerializeField] GameObject ItemSlotPrefab;

    // 이거 바꿀거면 UI 크기 수동으로 설정해줘야 함
    int _initialInventorySize = 15;

    List<ItemSlot> _inventory = new List<ItemSlot>();

    int _focusIndex = 0;

    void Start()
    {
        for(int i = 0; i < _initialInventorySize; ++i)
        {
            GameObject go = Instantiate(ItemSlotPrefab, QuickSlotArea);

            _inventory.Add(go.GetComponent<ItemSlot>());
        }
    }

    void Update()
    {
        ItemFocusing();
    }

    void ItemFocusing()
    {
        // 앞으로 굴리기 = +Y
        // 뒤로   굴리기 = -Y

        Vector2 mouseWheelDelta = Input.mouseScrollDelta;

        if(mouseWheelDelta.y != 0)
        {
            _focusIndex -= (int)mouseWheelDelta.y;
        }
        if(_focusIndex < 0)
        {
            _focusIndex += _initialInventorySize;
        }
        if(_focusIndex >= 15)
        {
            _focusIndex -= _initialInventorySize;
        }

        _inventory[_focusIndex].Activate();

        for(int i = 0; i < _initialInventorySize; ++i)
        {
            if(i == _focusIndex)
            {
                _inventory[_focusIndex].Activate();
            }
            else
            {
                _inventory[i].Deactivate();
            }
        }
    }
}

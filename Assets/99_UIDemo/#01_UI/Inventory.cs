using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Transform  ItemSlotArea;
    [SerializeField] GameObject ItemSlotPrefab;

    private List<List<ItemSlot>> inventoryList      = new List<List<ItemSlot>>();

    void Start()
    {
        for(int i = 0; i < 3; ++i)
        {
            List<ItemSlot> inventoryRow = new List<ItemSlot>();

            for(int j = 0; j < 3; ++j)
            {
                GameObject go = Instantiate(ItemSlotPrefab, ItemSlotArea);

                inventoryRow.Add(go.GetComponent<ItemSlot>());
            }

            inventoryList.Add(inventoryRow);
        }
    }

    void Update()
    {
        
    }
}

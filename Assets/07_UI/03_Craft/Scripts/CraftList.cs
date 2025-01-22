using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraftList : MonoBehaviour
{
    // 아이템 슬롯 담아놓을 컨테이너
    private List<CraftListItemSlot> _craftItemSlotList = new List<CraftListItemSlot>();

    private CraftingData _data;
    private Dictionary<string, int> _recipe = new Dictionary<string, int>();
    
    public void AddCraftItemSlot(Transform parent, GameObject craftItemSlotPrefab, CraftingData data)
    {
        Queue<string> queue = ParseRecipe(data);

        int count = 2 * data.NumOfMaterial + 1;

        for (int j = 0; j < count; ++j)
        {
            CraftListItemSlot slot = Instantiate(craftItemSlotPrefab, parent).GetComponent<CraftListItemSlot>();

            if(j == count - 1)
            {
                slot.SetType(CraftListItemSlot.Type.ItemSlot, _data.Name);
            }
            else if (j == count - 2)
            {
                slot.SetType(CraftListItemSlot.Type.Image_Equal, "Equal");
            }
            else if (j % 2 == 1)
            {
                slot.SetType(CraftListItemSlot.Type.Image_Plus, "Plus");
            }
            else
            {
                slot.SetType(CraftListItemSlot.Type.ItemSlot, queue.Dequeue());
            }

            _craftItemSlotList.Add(slot);
        }
    }

    private Queue<string> ParseRecipe(CraftingData data)
    {
        Queue<string> queue = new Queue<string>();

        _data = data;

        string[] recipes = _data.Recipe.Split('+');

        foreach(string item in recipes)
        {
            string[] recipe = item.Split('_');
            _recipe.Add(recipe[0], int.Parse(recipe[1]));

            queue.Enqueue(recipe[0]);
        }

        return queue;
    }
}

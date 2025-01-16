using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_CraftList : MonoBehaviour
{
    private List<UI_CraftListItemSlot> _craftItemSlotList = new List<UI_CraftListItemSlot>();

    private CraftingData _data;
    private Dictionary<string, int> _recipe = new Dictionary<string, int>();
    
    public void AddCraftItemSlot(Transform parent, GameObject craftItemSlotPrefab, CraftingData data)
    {
        Queue<string> queue = ParseRecipe(data);

        int count = 2 * data.NumOfMaterial + 1;

        for (int j = 0; j < count; ++j)
        {
            UI_CraftListItemSlot slot = Instantiate(craftItemSlotPrefab, parent).GetComponent<UI_CraftListItemSlot>();

            if(j == count - 1)
            {
                slot.SetType(UI_CraftListItemSlot.Type.ItemSlot, _data.Name);
            }
            else if (j == count - 2)
            {
                slot.SetType(UI_CraftListItemSlot.Type.Image_Equal, "Equal");
            }
            else if (j % 2 == 1)
            {
                slot.SetType(UI_CraftListItemSlot.Type.Image_Plus, "Plus");
            }
            else
            {
                slot.SetType(UI_CraftListItemSlot.Type.ItemSlot, queue.Dequeue());
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

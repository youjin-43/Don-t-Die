using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VInspector.Libs;

public class CraftList : MonoBehaviour
{
    // 이건 어떤 아이템을 만들 수 있는가
    private CraftingData _data;

    // 재료 데이터 파싱
    private Dictionary<string, int> _recipe = new Dictionary<string, int>();

    // 아이템 슬롯 담아놓을 컨테이너
    private List<CraftListItemSlot> _craftItemSlotList = new List<CraftListItemSlot>();


    private Dictionary<string , TextMeshProUGUI> _currentItemCountTextDict = new Dictionary<string , TextMeshProUGUI>();


    private Image _mask;

    void Awake()
    {
        _mask = transform.GetChild(1).GetComponent<Image>();
    }

    void Start()
    {
        if(_data.NeedCraftingTable == true)
        {
            _mask.color = new Color(0, 0, 0, 0.7f);
        }
    }

    public void AddCraftItemSlot(Transform parent, GameObject craftItemSlotPrefab, CraftingData data, out Queue<string> resourceQueue)
    {
        Queue<string> queue = ParseRecipe(data);

        // out 매개변수는 무조건 새로 할당해야 함
        // resourceQueue = queue (X)
        resourceQueue = new Queue<string>(queue);

        int count = 2 * data.NumOfMaterial + 1;

        for (int j = 0; j < count; ++j)
        {
            CraftListItemSlot slot = Instantiate(craftItemSlotPrefab, parent).GetComponent<CraftListItemSlot>();

            if(j == count - 1)
            {
                slot.SetData(CraftListItemSlot.Type.ItemSlot, _data.Name, 1);
            }
            else if (j == count - 2)
            {
                slot.SetData(CraftListItemSlot.Type.Image_Equal, "Equal");
            }
            else if (j % 2 == 1)
            {
                slot.SetData(CraftListItemSlot.Type.Image_Plus, "Plus");
            }
            else
            {
                string ingredient = queue.Dequeue();

                slot.SetData(CraftListItemSlot.Type.ItemSlot, ingredient, _recipe[ingredient]);
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

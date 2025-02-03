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

    // 조합 비활성화 마스크
    public Image _mask;

    // 조합 플래그
    private bool _isPossibleCrafting = false;
    private int  _possibleItemCount  = int.MaxValue;

    void Awake()
    {
        //_mask = transform.GetChild(1).GetComponent<Image>();
    }

    public void AddCraftItemSlot(Transform parent, GameObject craftItemSlotPrefab, CraftingData data)
    {
        _data = data;

        // _mask는 GetComponent하면 터져서 에디터에서 바인딩 했음
        if (_data.NeedCraftingTable == true)
        {
            _mask.color = new Color(0, 0, 0, 0.9f);
        }
        else
        {
            _mask.color = new Color(0, 0, 0, 0.7f);
        }

        Queue<string> queue = ParseRecipe(data);

        int count = 2 * data.NumOfMaterial + 1;

        for (int j = 0; j < count; ++j)
        {
            CraftListItemSlot slot = Instantiate(craftItemSlotPrefab, parent).GetComponent<CraftListItemSlot>();

            if(j == count - 1)
            {
                slot.SetData(CraftListItemSlot.CraftListItemSlotType.ResultSlot, _data.Name, 1);
                slot.SetRecipe(_recipe);
            }
            else if (j == count - 2)
            {
                slot.SetData(CraftListItemSlot.CraftListItemSlotType.Image_Equal, "Equal");
            }
            else if (j % 2 == 1)
            {
                slot.SetData(CraftListItemSlot.CraftListItemSlotType.Image_Plus, "Plus");
            }
            else
            {
                string ingredient = queue.Dequeue();

                slot.SetData(CraftListItemSlot.CraftListItemSlotType.ItemSlot, ingredient, _recipe[ingredient]);
            }

            _craftItemSlotList.Add(slot);
        }
    }

    private Queue<string> ParseRecipe(CraftingData data)
    {
        Queue<string> queue = new Queue<string>();

        string[] recipes = _data.Recipe.Split('+');

        foreach(string item in recipes)
        {
            string[] recipe = item.Split('_');
            _recipe.Add(recipe[0], int.Parse(recipe[1]));

            queue.Enqueue(recipe[0]);
        }

        return queue;
    }

    public void ResourceCounting(Dictionary<string, int> inventoryDict)
    {
        // Counting
        foreach (var craftItemSlot in _craftItemSlotList)
        {
            craftItemSlot.ResourceCounting(inventoryDict);
        }

        // Check
        for(int i = 0; i < _craftItemSlotList.Count - 2; i += 2)
        {
            // 조합 아이템 슬롯이 들고있는 아이템 수
            int itemCountInCraftItemSlot = _craftItemSlotList[i].ResourceCheck();

            // 조합에 필요한 아이템 수
            int needItemCount = _recipe[_craftItemSlotList[i].GetItemName()];

            // 들고 있는 아이템의 수가 더 많다면
            if (itemCountInCraftItemSlot >= needItemCount)
            {
                // 그렇다면 결과물을 몇 개 까지 만들 수 있을지 계산
                if(_possibleItemCount >= itemCountInCraftItemSlot / needItemCount)
                {
                    _possibleItemCount = itemCountInCraftItemSlot / needItemCount;
                }
                
                _isPossibleCrafting = true;
            }
            else
            {
                _isPossibleCrafting = false;
                break;
            }
        }


        // 모든 재료가 필요 재료보다 많은 경우
        if(_isPossibleCrafting == true)
        {
            SetCount();
        }
        else
        {
            _possibleItemCount = int.MaxValue;
            _isPossibleCrafting = false;
            _craftItemSlotList[_craftItemSlotList.Count - 1].CraftLock();
            if (_data.NeedCraftingTable == true)
            {
                _mask.color = new Color(0, 0, 0, 0.9f);
            }
            else
            {
                _mask.color = new Color(0, 0, 0, 0.7f);
            }
        }
    }

    private void SetCount()
    {
        _craftItemSlotList[_craftItemSlotList.Count - 1].SetCount(_possibleItemCount);

            _mask.color = new Color(0, 0, 0, 0f);

            _craftItemSlotList[_craftItemSlotList.Count - 1].CraftUnlock();
        if(_data.NeedCraftingTable == false)
        {
        }
        // 조합대가 필요한 조합들에 대해서는 아래서 처리
        else
        {

        }
    }
}

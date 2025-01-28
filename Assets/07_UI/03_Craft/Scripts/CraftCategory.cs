using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CraftCategory;

public class CraftCategory : MonoBehaviour
{
    public struct Recipe
    {
        public string name;

        public Dictionary<string, int> necessaryMaterials;
    }
    private Recipe _recipe;


    private GameObject _scrollView;           // 조합식들을 담는 Parent
    private GameObject _categorySelectImage;  // 클릭 됐는지 확인하기 위한 오브젝트
    
    private List<CraftList>    _craftLists       = new List<CraftList>(); // 조합식들 담아놓을 컨테이너
    private List<CraftingData> _craftingDataList = new List<CraftingData>(); // 조합식들 데이터를 담아놓을 컨테이너

    // 카테고리 하위에 여러가지 조합식들이 있다
    // 조합식은 어떤 아이템이 어떤 재료를 필요로 하는지가 기록되어 있고
    // 여기에 저장해놓음
    private HashSet<string> _resourceSet = new HashSet<string>();




    private List<Recipe> _recipes = new List<Recipe>();




    public void Start()
    {
        _categorySelectImage = transform.GetChild(1).gameObject;
        _scrollView          = transform.GetChild(2).gameObject;

        // Tool 카테고리가 기본으로 열려있는 상태
        if (gameObject.name == "Tool")
        {
            _categorySelectImage. SetActive(true);
            _scrollView.SetActive(true);
        }
    }


    // 카테고리 스위치
    public void ToggleCraftCategory(string name)
    {
        _categorySelectImage.SetActive(gameObject.name == name);
        _scrollView.SetActive(gameObject.name == name);
    }

    public void AddData(CraftingData data)
    {
        _recipes.Add(ParseRecipe(data));


        _craftingDataList.Add(data);
    }

    private Recipe ParseRecipe(CraftingData data)
    {
        Dictionary<string, int> necessaryMaterials = new Dictionary<string, int>();

        string[] recipes = data.Recipe.Split('+');

        foreach (string item in recipes)
        {
            string[] materialsAndCount = item.Split('_');

            necessaryMaterials.Add(materialsAndCount[0], int.Parse(materialsAndCount[1]));
        }

        Recipe recipe;
        {
            recipe.name = data.Name;
            recipe.necessaryMaterials = necessaryMaterials;
        }
        return recipe;
    }

    public void CreateCraftList(GameObject craftListPrefab, GameObject craftItemSlotPrefab)
    {
        Transform craftListParent = _scrollView.transform.GetChild(0).GetChild(0).GetChild(0);

        for(int i = 0; i < _craftingDataList.Count; ++i)
        {
            CraftList craftList = Instantiate(craftListPrefab, craftListParent).GetComponent<CraftList>();
            
            Queue<string> resourceQueue = new Queue<string>();

            craftList.AddCraftItemSlot(craftList.transform.GetChild(0).transform, craftItemSlotPrefab, _craftingDataList[i], out resourceQueue);

            int queueSize = resourceQueue.Count;

            for(int j = 0; j < queueSize; ++j)
            {
                string resource = resourceQueue.Dequeue();

                if(_resourceSet.Contains(resource) == false)
                {
                    _resourceSet.Add(resource);
                }
            }

            _craftLists.Add(craftList);
        }
    }

    public void ResouceCounting(Dictionary<string, int> inventoryDict)
    {
        Dictionary<string , int> categorizedResourceDict = new Dictionary<string , int>();

        foreach(var resource in _resourceSet)
        {
            if(inventoryDict.ContainsKey(resource) == true)
            {
                categorizedResourceDict[resource] = inventoryDict[resource];
            }
        }

        foreach(var categorizedResource in categorizedResourceDict)
        {

        }
    }


    
}

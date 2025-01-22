using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_CraftCategory : MonoBehaviour
{
    private GameObject _scrollView; // 조합식들을 담는 Parent
    private GameObject _highLight;  // 클릭 됐는지 확인하기 위한 오브젝트
    
    private List<UI_CraftList> _craftLists       = new List<UI_CraftList>(); // 조합식들 담아놓을 컨테이너
    private List<CraftingData> _craftingDataList = new List<CraftingData>(); // 조합식들 데이터를 담아놓을 컨테이너

    public void Start()
    {
        _highLight  = transform.GetChild(0).gameObject;
        _scrollView = transform.GetChild(2).gameObject;

        // Tool 카테고리가 기본으로 열려있는 상태
        if (gameObject.name == "Tool")
        {
            _highLight. SetActive(true);
            _scrollView.SetActive(true);
        }
    }


    // 카테고리 스위치
    public void ToggleCraftCategory(string name)
    {
        _highLight .SetActive(gameObject.name == name);
        _scrollView.SetActive(gameObject.name == name);
    }

    public void AddData(CraftingData data)
    {
        _craftingDataList.Add(data);
    }

    public void CreateCraftList(GameObject craftListPrefab, GameObject craftItemSlotPrefab)
    {
        Transform craftListParent = _scrollView.transform.GetChild(0).GetChild(0).GetChild(0);

        for(int i = 0; i < _craftingDataList.Count; ++i)
        {
            UI_CraftList craftList = Instantiate(craftListPrefab, craftListParent).GetComponent<UI_CraftList>();

            craftList.AddCraftItemSlot(craftList.transform, craftItemSlotPrefab, _craftingDataList[i]);

            _craftLists.Add(craftList);
        }
    }
}

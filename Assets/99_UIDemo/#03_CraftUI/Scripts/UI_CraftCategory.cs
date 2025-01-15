using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_CraftCategory : MonoBehaviour
{
    private GameObject _scrollView;
    private GameObject _highLight;

    private bool _isActive = false;

    private List<UI_CraftList>    _craftLists       = new List<UI_CraftList>();
    private List<CraftingData> _craftingDataList = new List<CraftingData>();

    public void Start()
    {
        _highLight  = transform.Find("Highlight").gameObject;
        _scrollView = transform.Find("ScrollViewArea").gameObject;

        if (gameObject.name == "Tool")
        {
            Active();
        }
    }

    void Update()
    {
    }

    public void Active()
    {
        _isActive = true;
        _highLight.SetActive(true);
        _scrollView.SetActive(true);
    }
    public void Deactive()
    {
        _isActive = false;
        _highLight.SetActive(false);
        _scrollView.SetActive(false);
    }

    public void AddData(CraftingData data)
    {
        _craftingDataList.Add(data);
    }
    public void CreateCraftList(GameObject craftListPrefab, GameObject craftItemSlotPrefab)
    {
        Transform craftListParent = _scrollView.transform.Find("ScrollView").Find("Viewport").Find("Content");

        for(int i = 0; i < _craftingDataList.Count; ++i)
        {
            UI_CraftList craftList = Instantiate(craftListPrefab, craftListParent).GetComponent<UI_CraftList>();

            craftList.AddCraftItemSlot(craftList.transform, craftItemSlotPrefab, _craftingDataList[i]);

            _craftLists.Add(craftList);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class UI_CraftTab : MonoBehaviour
{
    private GameObject _scrollView;
    private GameObject _craftList;

    private RectTransform _rectTransform;
    private Vector2       _defaultScale;

    private List<CraftingData> _craftingDataList= new List<CraftingData>();

    private bool _isActive = false;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defaultScale  = _rectTransform.sizeDelta;

        if(gameObject.name == "Tool")
        {
            Active();
        }
    }

    void Update()
    {
        if(_isActive == true)
        {
            _scrollView.SetActive(true);
            _craftList.SetActive(true);
        }
        else
        {
            _scrollView.SetActive(false);
            _craftList.SetActive(false);
        }
    }

    public void Active()
    {
        _isActive = true;

        Vector2 newSize = _rectTransform.sizeDelta;
        newSize.y = _defaultScale.y - 20f;

        _rectTransform.sizeDelta = newSize;
    }
    public void Deactive()
    {
        _isActive = false;

        _rectTransform.sizeDelta = _defaultScale;
    }

    public void AddData(CraftingData data)
    {
        _craftingDataList.Add(data);
    }
    public void CreateCraftList(Transform scrollViewArea, GameObject scrollViewPrefab, GameObject craftListPrefab, GameObject craftItemSlotPrefab)
    {
        _scrollView = Instantiate(scrollViewPrefab, scrollViewArea);

        Transform craftListParent = _scrollView.transform.Find("Viewport").Find("Content");

        for(int i = 0; i < _craftingDataList.Count; ++i)
        {
            _craftList = Instantiate(craftListPrefab, craftListParent);

            for(int j = 0; j < 2 *_craftingDataList[i].NumOfMaterial + 1; ++j)
            {
                GameObject slot = Instantiate(craftItemSlotPrefab, _craftList.transform);
            }
        }
    }
}

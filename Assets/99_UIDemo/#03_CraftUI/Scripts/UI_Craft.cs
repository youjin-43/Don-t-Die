using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] Transform  ScrollViewArea;
    [SerializeField] GameObject ScrollViewPrefab;
    [SerializeField] GameObject CraftListPrefab;
    [SerializeField] GameObject CraftItemSlotPrefab;

    private Dictionary<string, UI_CraftTab> _tabs = new Dictionary<string, UI_CraftTab>();

    void Start()
    {
        foreach (Transform sibling in transform)
        {
            UI_CraftTab tab = sibling.GetComponent<UI_CraftTab>();

            if (tab != null)
            {
                tab.transform.GetComponent<Button>().onClick.AddListener(() => TabClicked(tab.name));

                _tabs.Add(tab.name, tab);
            }
        }
        foreach (var data in DataManager.Instance.CraftingData)
        {
            if(_tabs.TryGetValue(data.Value.Category, out UI_CraftTab tab))
            {
                tab.AddData(data.Value);
            }
        }
        foreach (var tab in _tabs)
        {
            tab.Value.CreateCraftList(ScrollViewArea, ScrollViewPrefab, CraftListPrefab, CraftItemSlotPrefab);
        }
    }

    public void TabClicked(string tabName)
    {
        foreach(var tab in _tabs)
        {
            if(tab.Key == tabName)
            {
                tab.Value.Active();
            }
            else
            {
                tab.Value.Deactive();
            }
        }
    }
}

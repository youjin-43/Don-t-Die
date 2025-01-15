using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] GameObject CraftListPrefab;
    [SerializeField] GameObject CraftListItemSlotPrefab;

    public SerializedDictionary<string, UI_CraftCategory> Categories = new SerializedDictionary<string, UI_CraftCategory>();

    void Start()
    {
        foreach (var data in DataManager.Instance.CraftingData)
        {
            if(Categories.TryGetValue(data.Value.Category, out UI_CraftCategory category))
            {
                category.AddData(data.Value);
            }
        }
        foreach (var category in Categories)
        {
            category.Value.transform.GetComponent<Button>().onClick.AddListener(() => TabClicked(category.Value.name));

            category.Value.CreateCraftList(CraftListPrefab, CraftListItemSlotPrefab);
        }
    }

    public void TabClicked(string tabName)
    {
        foreach(var tab in Categories)
        {
            if(tab.Value.name == tabName)
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

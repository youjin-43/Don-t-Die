using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class UI_Craft : MonoBehaviour
{
    private static UI_Craft instance;
    public  static UI_Craft Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

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
            category.Value.Start();

            category.Value.transform.GetComponent<Button>().onClick.AddListener(() => TabClicked(category.Value.name));

            category.Value.CreateCraftList(CraftListPrefab, CraftListItemSlotPrefab);
        }

        ToggleCraftingUI();
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

    public void ToggleCraftingUI()
    {
        if(gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}

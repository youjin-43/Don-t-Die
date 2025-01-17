using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Collections;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager : MonoBehaviour
{
    #region SINGLETON
    private static DataManager instance;
    public  static DataManager Instance
    {
        get
        {
            return instance;
        }
    }

    void SingletonInitialize()
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
    #endregion

    // Json => Data
    public Dictionary<string, CraftingData> CraftingData { get; private set; } = new Dictionary<string, CraftingData>();

    // Addressable
    public Dictionary<string, Sprite>   IconImageData = new Dictionary<string, Sprite>();
    public Dictionary<string, ItemData> ItemData      = new Dictionary<string, ItemData>();

    async void Awake()
    {
        SingletonInitialize();

        // Load From Json
        CraftingData = LoadJson<CraftingDataLoader, string, CraftingData>("CraftingData").MakeDict();

        // Addressable
        var asyncOperation_1 = Addressables.LoadAssetsAsync<Sprite>("IconImage", OnImageLoaded);
        var asyncOperation_2 = Addressables.LoadAssetsAsync<ItemData>("ItemData", OnItemDataLoaded);

        asyncOperation_1.WaitForCompletion();
        asyncOperation_2.WaitForCompletion();
    }

    private T LoadJson<T, Key, Value>(string fileName) where T : ILoader<Key, Value>
    {
        string fullPath = PathManager.Instance.JsonFilePath(fileName);

        string json = File.ReadAllText(fullPath);

        return JsonConvert.DeserializeObject<T>(json);
    }

    // Addressable
    void OnImageLoaded(Sprite sprite)
    {
        IconImageData.Add(sprite.name, sprite);
    }

    // Addressable
    void OnItemDataLoaded(ItemData data)
    {
        ItemData.Add(data.Name, data);
    }
}
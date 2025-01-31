using UnityEngine;
using Newtonsoft.Json;
using ExcelDataReader;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Unity.Collections;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
    public Dictionary<string, CraftingData>    CraftingData        { get; private set; } = new Dictionary<string, CraftingData>();
    public Dictionary<string, AchievementData> AchievementData     { get; private set; } = new Dictionary<string, AchievementData>();
    public Dictionary<string, AchievementData> UserAchievementData { get; private set; } = new Dictionary<string, AchievementData>();

    // Addressable
    public Dictionary<string, Sprite>             IconImageData   = new Dictionary<string, Sprite>();
    public Dictionary<string, ItemData>           ItemData        = new Dictionary<string, ItemData>();
    public Dictionary<string, NatureResourceData> NatureResources = new Dictionary<string, NatureResourceData>();
    public Dictionary<string, Biome>              BiomeDatas      = new Dictionary<string, Biome>();

    void Awake()
    {
        SingletonInitialize();

        // Addressable
        var iconImageLoadOperationHandle = Addressables.LoadAssetsAsync<Sprite>  ("IconImage", OnImageLoaded   );
        var itemDataLoadOperationHandle  = Addressables.LoadAssetsAsync<ItemData>("ItemData", OnItemDataLoaded);

        iconImageLoadOperationHandle.WaitForCompletion();
        itemDataLoadOperationHandle .WaitForCompletion();


        var natureResourceHandle = Addressables.LoadAssetsAsync<NatureResourceData>("NatureResources");
        natureResourceHandle.Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var data in handle.Result)
                {
                    if (!NatureResources.ContainsKey(data.name))
                    {
                        NatureResources.Add(data.name, data);
                    }
                }
            }
            else
            {
                DebugController.Log("NatureResources Load Failed.");
            }
        };

        var biomeDataHandle = Addressables.LoadAssetsAsync<Biome>("BiomeDatas");
        biomeDataHandle.Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var biome in handle.Result)
                {
                    if (!BiomeDatas.ContainsKey(biome.name))
                    {
                        BiomeDatas.Add(biome.name, biome);
                    }
                }
            }
            else
            {
                DebugController.Log("BiomeDatas Load Failed.");
            }
        };

        // Load From Json
        CraftingData        = LoadJson<CraftingDataLoader, string, CraftingData>("CraftingData").MakeDict();
        AchievementData     = LoadJson<AchievementDataLoader, string, AchievementData>("AchievementData").MakeDict();
        UserAchievementData = LoadJson<AchievementDataLoader, string, AchievementData>("UserAchievementData").MakeDict();
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
    void OnItemDataLoaded(ItemData itemData)
    {
        ItemData.Add(itemData.Name, itemData);
    }



    public void SetAchievement(string code)
    {
        UserAchievementData.Add(code, AchievementData[code]);

        //string json = JsonUtility.ToJson(UserAchievementData, true);
        string json = JsonConvert.SerializeObject(new { Items = ToList(UserAchievementData) }, Formatting.Indented);

        File.WriteAllText(PathManager.Instance.JsonFilePath("UserAchievementData"), json);
    }

    private List<AchievementData> ToList(Dictionary<string ,AchievementData> dict)
    {
        List<AchievementData> list = new List<AchievementData>();

        foreach(var kvp in dict)
        {
            list.Add(kvp.Value);
        }

        return list;
    }
}
using UnityEngine;
using Newtonsoft.Json;
using ExcelDataReader;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public  static DataManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Json => Data
    public Dictionary<string, CraftingData> CraftingData { get; private set; } = new Dictionary<string, CraftingData>();

    public Dictionary<string, Sprite> IconImageData = new Dictionary<string, Sprite>();

    void Awake()
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

        // Addressable
        Addressables.LoadAssetsAsync<Sprite>("IconImage", OnImageLoaded);


        // Load From Json
        CraftingData = LoadJson<CraftingDataLoader, string, CraftingData>("CraftingData").MakeDict();
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
}
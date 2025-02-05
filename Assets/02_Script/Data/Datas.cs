using System;
using System.Collections.Generic;
using UnityEngine;

#region CraftingData
[Serializable]
public class CraftingData
{
    public string Name;
    public string NameKr;
    public string Category;
    public int    NumOfMaterial;
    public bool   NeedCraftingTable;
    public string Recipe;
}

[Serializable]
public class CraftingDataLoader : ILoader<string, CraftingData>
{
    public List<CraftingData> Items = new List<CraftingData>();

    public Dictionary<string, CraftingData> MakeDict()
    {
        Dictionary<string, CraftingData> dict = new Dictionary<string, CraftingData>();

        foreach(CraftingData data in Items)
        {
            dict.Add(data.Name, data);
        }
        
        return dict;
    }
}
#endregion


#region AchievementData
[Serializable]
public class AchievementData
{
    public string Code;
    public string Content;
}

[Serializable]
public class AchievementDataLoader : ILoader<string, AchievementData>
{
    public List<AchievementData> Items = new List<AchievementData>();

    public Dictionary<string, AchievementData> MakeDict()
    {
        Dictionary<string, AchievementData> dict = new Dictionary<string, AchievementData>();

        foreach(AchievementData data in Items)
        {
            dict.Add(data.Code, data);
        }

        return dict;
    }
}
#endregion
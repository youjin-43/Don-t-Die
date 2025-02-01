using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using Unity.Plastic.Newtonsoft.Json;
using ExcelDataReader;
using System.Data;

public class DataTransformer : EditorWindow
{
#if UNITY_EDITOR
    [MenuItem("★Tools★/ParseExcel %#K")]
    public static void ParseExcelDataToJson()
    {
        ParseExcelDataToJson<CraftingDataLoader, CraftingData>("CraftingData");
        ParseExcelDataToJson<AchievementDataLoader, AchievementData>("AchievementData");

        Debug.Log("DataTransformer Completed");
    }

    [MenuItem("★Tools★/CreateDefaultUserAchieveJson")]
    public static void CreateDefaultUserAchieveJson()
    {
        Dictionary<string, AchievementData> UserAchievementData  = new Dictionary<string, AchievementData>();

        string json = JsonUtility.ToJson(UserAchievementData, true);

        File.WriteAllText(PathManager.Instance.JsonFilePath("UserAchievementData"), json);
    
        Debug.Log("CreateDefaultUserAchieveJson Completed");
    }

    private static void ParseExcelDataToJson<Loader, LoaderData>(string fileName) where Loader : new() where LoaderData : new()
    {
        string fullPath = PathManager.Instance.ExcelFilePath(fileName);

        if (File.Exists(fullPath) == false)
        {
            Debug.LogError("FilePath Error");
            return;
        }

        FileStream stream = File.Open(fullPath, FileMode.Open, FileAccess.Read);

        IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

        DataSet  result = reader.AsDataSet();
        DataTable table = result.Tables[0];

        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

        List<string> headers = new List<string>();

        for (int col = 0; col < table.Columns.Count; col++)
        {
            headers.Add(table.Rows[0][col].ToString());
        }
        for (int row = 1; row < table.Rows.Count; row++)
        {
            Dictionary<string, object> rowData = new Dictionary<string, object>();

            for (int col = 0; col < table.Columns.Count; col++)
            {
                rowData[headers[col]] = table.Rows[row][col].ToString();
            }
            data.Add(rowData);
        }

        reader.Close();

        string json = JsonConvert.SerializeObject(new { Items = data }, Formatting.Indented);

        File.WriteAllText(PathManager.Instance.JsonFilePath(fileName), json);
    }
#endif
}
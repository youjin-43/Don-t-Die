using UnityEngine;

public class PathManager
{
    private static PathManager instance;
    public  static PathManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PathManager();
            }

            return instance;
        }
    }

    private string _excelFilePath = $"{Application.dataPath}/Data/";
    private string _jsonFilePath  = $"{Application.dataPath}/Data/";

    public string ExcelFilePath(string fileName)
    {
        return $"{_excelFilePath}/{fileName}.xlsx";
    }
    public string JsonFilePath(string fileName)
    {
        return $"{_jsonFilePath}/{fileName}.json";
    }
}

using UnityEngine;

public class PathManager
{
    #region SINGLETON
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
    #endregion

    private string _excelFilePath = $"{Application.dataPath}/Data";
    private string _jsonFilePath  = $"{Application.dataPath}/Data";

    public string ExcelFilePath(string fileName)
    {
        return $"{_excelFilePath}/{fileName}.xlsx";
    }

    public string JsonFilePath(string fileName)
    {
        return $"{_jsonFilePath}/{fileName}.json";
    }
}

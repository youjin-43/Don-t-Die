using Mono.Cecil.Cil;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    #region SINGLETON
    private static AchievementManager instance;
    public  static AchievementManager Instance
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

    // 업적 슬롯 프리펩
    [SerializeField] GameObject AchievementSlotPrefab;

    // 업적 슬롯 컨테이너
    private Dictionary<string, AchievementSlot> _achievementSlotDict = new Dictionary<string, AchievementSlot>();

    void Awake()
    {
        SingletonInitialize();
    }

    void Start()
    {
        int achievementCount = DataManager.Instance.AchievementData.Count;
        Transform content = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).transform;

        // 우선 총 업적 갯수만큼 슬롯을 생성
        for (int i = 1; i <= achievementCount; ++i)
        {
            string code = i.ToString("D3");

            AchievementSlot achievementSlot = Instantiate(AchievementSlotPrefab, content).GetComponent<AchievementSlot>();

            achievementSlot.SetCode(code);

            _achievementSlotDict.Add(code, achievementSlot);
        }
        
        // 유저가 달성 한 업적과 비교해서 업적 슬롯을 업데이트
        var userAchievementData = DataManager.Instance.UserAchievementData;

        foreach(var data in userAchievementData)
        {
            _achievementSlotDict[data.Key].RefreshAchievement(data.Key, data.Value.Content);
        }

        ToggleAchievementUI();
    }

    public void RefreshAchievement(string code)
    {
        _achievementSlotDict[code].RefreshAchievement(code, DataManager.Instance.AchievementData[code].Content);
    }

    public void ToggleAchievementUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SetAchievement(string code)
    {
        DataManager.Instance.SetAchievement(code);

        RefreshAchievement(code);
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum Season
{
    Fall,
    Winter
}

public class TimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;

    [SerializeField] AnimationCurve[] timeCurves;

    [SerializeField] float timeScale = 180f; // 하루가 8분이 되려면 realtime보다 180배 빨라야 함
    [SerializeField] float startAtTime = 28800f; // 8am
    [SerializeField] Light2D globalLight;

    List<TimeAgent> timeAgents;

    Season currentSeason;
    int days;
    float time;

    float growTimer;
    float growthInterval = 60f; // 게임 시간 기준으로 1분

    [SerializeField] List<int> daysPerSeason;

    // Debug용
    [SerializeField] TMP_Text timeDisplayer;

    private void Awake()
    {
        timeAgents = new List<TimeAgent>();
    }

    void Start()
    {
        currentSeason = Season.Fall;
        growTimer = 0;
        time = startAtTime;
    }

    float Hours
    {
        get { return time / 3600f; }
    }

    float Minutes
    {
        get { return time % 3600f / 60f; }
    }

    void Update()
    {
        time += Time.deltaTime * timeScale;
        growTimer += Time.deltaTime * timeScale;
        DisplayTime();
        ControlLight();

        if (time > secondsInDay)
        {
            NextDay();
        }

        if (growTimer >= growthInterval)
        {
            SignalToTimeAgents();
            growTimer = 0;
        }
    }

    /// <summary>
    /// 시간의 영향을 받는 agent들에게 신호를 보냄
    /// </summary>
    void SignalToTimeAgents()
    {
        foreach (TimeAgent agent in timeAgents)
        {
            agent.UpdateTimer();
        }
    }

    /// <summary>
    /// 이 함수가 호출되면 agent가 시간의 영향을 받게 된다.
    /// </summary>
    /// <param name="agent"></param>
    public void Subscribe(TimeAgent agent)
    {
        DebugController.Log($"{agent.name} subscribes time controller.");
        timeAgents.Add(agent);
    }

    /// <summary>
    /// 이 함수가 호출되면 agent가 시간의 영향을 받지 않게 된다.
    /// </summary>
    /// <param name="agent"></param>
    public void Unsubscribe(TimeAgent agent)
    {
        timeAgents.Remove(agent);
    }

    void DisplayTime()
    {
        int h = (int)Hours;
        int m = (int)Minutes;
        timeDisplayer.text = $"{h.ToString("00")} : {m.ToString("00")}";
    }

    /// <summary>
    /// 시간과 계절에 맞춰서 광원의 밝기를 조절한다.
    /// </summary>
    void ControlLight()
    {
        float v = timeCurves[(int)currentSeason].Evaluate(Hours);
        globalLight.intensity = v;
    }

    void NextDay()
    {
        time = 0;
        days++;

        if (days >= daysPerSeason[(int)currentSeason])
        {
            NextSeason();
        }
    }

    void NextSeason()
    {
        days = 0;
        currentSeason = (Season)((int)((currentSeason + 1)) % System.Enum.GetValues(typeof(Season)).Length);
    }
}

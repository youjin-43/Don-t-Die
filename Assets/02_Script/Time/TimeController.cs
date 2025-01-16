using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;

    [SerializeField] AnimationCurve[] timeCurves;

    [SerializeField] float timeScale = 180f; // 하루가 8분이 되려면 realtime보다 180배 빨라야 함
    [SerializeField] float startAtTime = 28800f; // 8am
    [SerializeField] Light2D globalLight;

    List<TimeAgent> timeAgents;
    int season;
    int days;
    float time;

    float growTimer;
    float growthInterval = 60f; // 게임 시간 기준 1분

    //Debug
    [SerializeField] TMP_Text timeDisplayer;

    private void Awake()
    {
        timeAgents = new List<TimeAgent>();
    }

    void Start()
    {
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

    void SignalToTimeAgents()
    {
        foreach (TimeAgent agent in timeAgents)
        {
            agent.UpdateTimer();
        }
    }

    public void Subscribe(TimeAgent agent)
    {
        DebugController.Log($"{agent.name} subscribes time controller.");
        timeAgents.Add(agent);
    }

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

    void ControlLight()
    {
        float v = timeCurves[season].Evaluate(Hours);
        globalLight.intensity = v;
    }

    void NextDay()
    {
        time = 0;
        days++;
    }
}

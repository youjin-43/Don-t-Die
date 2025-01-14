using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightController : MonoBehaviour
{
    const float secondsInDay = 86400f;

    [SerializeField] AnimationCurve[] timeCurves;

    [SerializeField] float timeScale = 180f; // ÇÏ·ç 8ºÐ
    [SerializeField] float startAtTime = 28800f; // 8am
    [SerializeField] Light2D globalLight;

    List<TimeAgent> agents;
    int season;
    int days;
    float time;

    //Debug
    [SerializeField] TMP_Text timeDisplayer;

    private void Awake()
    {
        agents = new List<TimeAgent>();
    }

    void Start()
    {
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
        DisplayTime();
        ControlLight();

        if (time > secondsInDay)
        {
            NextDay();
        }
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

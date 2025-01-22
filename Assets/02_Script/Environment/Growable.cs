using System.Net;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public class Growable : TimeAgent
{
    [SerializeField] GrowableResourceData data;
    SpriteRenderer spriteRenderer;
    int growStage;

    public int GrowStage
    {
        get { return growStage; }
        set { growStage = Mathf.Clamp(value, 0, data.GrowthStageTime.Count - 1); }
    }

    /// <summary>
    /// 최대로 성장했는가
    /// </summary>
    bool isAllGrown 
    {
        get {
            if (data == null) { return false; }
            return timer >= data.TimeToAllGrown; 
        }
    }

    /// <summary>
    /// 수확할 수 있을 만큼 자랐는지 확인
    /// </summary>
    public bool canBeHarvested => timer >= data.MinTimeToHarvest;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 맵 Clear 된 상태로 실행해야 에러 안 남
        EnvironmentManager.Instance.GetComponent<TimeController>().Subscribe(this);
        UpdateSprite();
    }

    private void OnEnable()
    {
        if (EnvironmentManager.Instance.TryGetComponent(out TimeController timeController))
        {
            timeController.Subscribe(this);
        }
        ResetTimer();
    }

    private void OnDisable()
    {
        if (isQuitting) { return; }
        if (EnvironmentManager.Instance.TryGetComponent(out TimeController timeController))
        {
            timeController.Unsubscribe(this);
        }
    }

    // 게임뷰에서 씬뷰로 넘어올 때 에러 방지
    bool isQuitting = false;
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    public override void UpdateTimer()
    {
        if (isAllGrown) return;

        timer++;

        if (timer > data.GrowthStageTime[growStage])
        {
            growStage++;
            UpdateSprite();
        }
    }

    void UpdateSprite()
    {
        if (spriteRenderer == null)
        {
            return;
        }
        spriteRenderer.sprite = data.Sprites[growStage];
    }

    public void ResetTimer()
    {
        growStage = 0;
        timer = 0;
        UpdateSprite();
    }
}

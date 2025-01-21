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

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 맵 Clear 된 상태로 실행해야 에러 안 남
        EnvironmentManager.Instance.GetComponent<TimeController>().Subscribe(this);
        UpdateSprite();
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

    private void OnDestroy()
    {
        if (isQuitting) { return; }
        if (EnvironmentManager.Instance.TryGetComponent(out TimeController timeController))
        {
            timeController.Unsubscribe(this);
        }
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
        spriteRenderer.sprite = data.Sprites[growStage];
    }
}

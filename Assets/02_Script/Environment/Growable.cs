using System.Net;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public class Growable : TimeAgent
{
    [SerializeField] GrowableResourceData data;
    SpriteRenderer spriteRenderer;
    int growStage;

    bool isAllGrown // 최대로 성장했나?
    {
        get {
            if (data == null) { return false; }
            return timer >= data.TimeToAllGrown; 
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 임시!!!!!! 그리고 맵 Clear 된 상태로 실행해야 에러 안 남
        transform.parent.parent.GetComponent<TimeController>().Subscribe(this);
    }

    private void Update()
    {
        
    }

    public override void UpdateTimer()
    {
        if (isAllGrown) return;

        timer++;

        if (timer > data.GrowthStageTime[growStage])
        {
            growStage++;
            spriteRenderer.sprite = data.Sprites[growStage];
        }
    }
}

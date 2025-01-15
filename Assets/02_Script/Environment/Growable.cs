using System.Net;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public class Growable : TimeAgent
{
    [SerializeField] GrowableResourceData data;
    SpriteRenderer spriteRenderer;
    int growTimer;
    int growStage;
    bool isAllGrown // 최대로 성장했나?
    {
        get {
            if (data == null) { return false; }
            return growTimer >= data.TimeToAllGrown; 
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        onTimeTick += Tick;
        Init();
    }

    void Tick()
    {
        DebugController.Log("Growable Tick");
        if (isAllGrown) return;

        growTimer++;

        if (growTimer > data.GrowthStageTime[growStage])
        {
            DebugController.Log("Change Sprite");
            growStage++;
            spriteRenderer.sprite = data.Sprites[growStage];
        }
    }
}

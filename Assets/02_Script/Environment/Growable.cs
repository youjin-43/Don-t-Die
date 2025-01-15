using UnityEngine;

public class Growable : TimeAgent
{
    [SerializeField] GrowableResourceData data;
    SpriteRenderer spriteRenderer;
    int growTimer;
    int growStage;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        onTimeTick += Tick;
    }

    void Tick()
    {
        growTimer++;

        if (growTimer > data.GrowthStageTime[growStage])
        {
            growStage++;
            spriteRenderer.sprite = data.Sprites[growStage];
        }
    }
}

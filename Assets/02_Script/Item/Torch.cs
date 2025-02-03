using UnityEngine;

public class Torch : TimeAgent
{
    [SerializeField] float maxDuration;         // 게임 시간 기준 몇 분 버틸 수 있는지.
    public float currentDuration;

    public override void UpdateTimer()
    {
        currentDuration -= 1f;
        if (currentDuration <= float.Epsilon)
        {
            TurnOff();
        }
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }

    public void TurnOn()
    {
        gameObject.SetActive(true);
    }
}

using UnityEngine;

public class Torch : TimeAgent
{
    public override void UpdateTimer()
    {
        if (EquipmentManager.Instance.GetCurrentTool() == null || EquipmentManager.Instance.GetCurrentTool().Type != ToolType.Torch) return;
        EquipmentManager.Instance.ReduceToolDurability(out bool destroyed);
        if (destroyed)
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

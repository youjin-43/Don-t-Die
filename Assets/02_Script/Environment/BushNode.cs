using UnityEngine;

public class BushNode : ResourceNode
{
    public override void Harvest()
    {
        if (!GetComponent<Growable>().canBeHarvested)
        {
            DebugController.Log("수확할 수 없습니다.");
            return;
        }
        SpreadItems();
        ResetGrowTimer();
    }


    void ResetGrowTimer()
    {
        GetComponent<Growable>().ResetTimer();
    }
}

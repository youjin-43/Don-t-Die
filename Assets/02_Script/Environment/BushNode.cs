using UnityEngine;

public class BushNode : ResourceNode
{
    public override void Harvest()
    {
        if (!GetComponent<Growable>().canBeHarvested)
        {
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

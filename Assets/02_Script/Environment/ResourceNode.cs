using System;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    NatureResourceData natureResourceData;

    public void SetData(NatureResourceData renewableResourceData)
    {
        this.natureResourceData = renewableResourceData;
    }

    protected void Harvest()
    {
        gameObject.SetActive(false);
    }
}

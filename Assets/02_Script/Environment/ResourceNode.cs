using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    RenewableResourceData renewableResourceData;

    public void SetData(RenewableResourceData renewableResourceData)
    {
        this.renewableResourceData = renewableResourceData;
    }
}

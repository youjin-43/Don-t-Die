using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    NatureResourceData renewableResourceData;

    public void SetData(NatureResourceData renewableResourceData)
    {
        this.renewableResourceData = renewableResourceData;
    }
}

using System;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] protected NatureResourceData natureResourceData;
    public NatureResourceData Data {  get { return natureResourceData; } }

    protected virtual void Init()
    {

    }

    public virtual void Harvest()
    {
        // 수확이 되면 objectMap에서 정보를 지우고 비활성화한다. 
        // Object Pooling을 써서 비활성화/활성화할지 아예 destroy instantiate를 해버릴지 고민중 ㄱ-
        EnvironmentManager.Instance.objectMap.ClearTiles(new Vector2Int((int)transform.position.x, (int)transform.position.y), natureResourceData.Width, natureResourceData.Height);
        EnvironmentManager.Instance.natureResources.Remove(transform.position);
        PoolManager.Instance.Push(gameObject);

        foreach(var item in natureResourceData.dropItems)
        {
            // spread Items
        }
    }
}

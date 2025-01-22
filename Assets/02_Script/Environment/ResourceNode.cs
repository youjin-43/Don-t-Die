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
        // 기본 로직은 맵에서 사라지고 아이템을 드랍하는 것이다.
        RemoveFromMap();
        SpreadItems();
    }

    /// <summary>
    /// Map 위에서 오브젝트를 없앤 후 풀에 반환한다.
    /// </summary>
    protected void RemoveFromMap()
    {
        EnvironmentManager.Instance.objectMap.ClearTiles(new Vector2Int((int)transform.position.x, (int)transform.position.y), natureResourceData.Width, natureResourceData.Height);
        EnvironmentManager.Instance.natureResources.Remove(transform.position);
        PoolManager.Instance.Push(gameObject);
    }

    protected void SpreadItems()
    {
        foreach (var item in natureResourceData.dropItems)
        {
            
        }
    }
}

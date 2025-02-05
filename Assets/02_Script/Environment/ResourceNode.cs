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
            int count = UnityEngine.Random.Range(item.minAmount, item.maxAmount + 1);

            while (count > 0)
            {
                Item go = PoolManager.Instance.InstantiateItem(item.data);

                // 긴데 별거 없습니다.. 플레이어 반대 방향으로 뿌리겠다는 뜻
                Vector3 dir = transform.position + 
                    (transform.position - GameManager.Instance.GetPlayerPos() 
                    + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
                
                go.Spread(transform.position, dir, UnityEngine.Random.Range(2.5f, 3f));
                count--;
            }
        }
    }
}

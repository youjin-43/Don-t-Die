using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] List<DropItem> dropItems;

    public void Harvest()
    {
        SpreadItems();
        RemoveFromMap();
    }

    /// <summary>
    /// Map 위에서 오브젝트를 없앤 후 풀에 반환한다.
    /// </summary>
    protected void RemoveFromMap()
    {
        EnvironmentManager.Instance.objectMap.ClearTiles(new Vector2Int((int)transform.position.x, (int)transform.position.y), 1, 1);
        Destroy(gameObject);
    }

    protected void SpreadItems()
    {
        foreach (var item in dropItems)
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

using UnityEngine;

public class TreeNode : DamageableResourceNode
{
    [SerializeField] GameObject trunkPrefab;

    public override void Harvest()
    {
        base.Harvest();
        InstantiateTrunk();
    }

    /// <summary>
    /// 밑동을 생성하고 자원 딕셔너리에 추가한다.
    /// </summary>
    void InstantiateTrunk()
    {
        GameObject go = PoolManager.Instance.InstantiatePoolObject(trunkPrefab, rootParent: EnvironmentManager.Instance.VoronoiMapGenerator.objectParent.transform);
        go.transform.position = transform.position;
        EnvironmentManager.Instance.natureResources.Add(transform.position, new ResourceObject
        {
            position = new Vector2Int((int)transform.position.x, (int)transform.position.y),
            dataName = go.GetComponent<ResourceNode>().Data.name,
            isDamageable = true,
            gameObject = go
        });
    }
}
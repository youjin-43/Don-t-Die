using UnityEngine;

public class TreeNode : DamageableResourceNode
{
    [SerializeField] GameObject trunkPrefab;
    public override void Harvest()
    {
        // 나무를 자원 딕셔너리에서 지우고 풀에 반환
        EnvironmentManager.Instance.natureResources.Remove(transform.position);
        PoolManager.Instance.Push(gameObject);

        // 밑동 생성하고 자원 딕셔너리에 추가
        GameObject go = PoolManager.Instance.InstantiatePoolObject(trunkPrefab, rootParent:EnvironmentManager.Instance.VoronoiMapGenerator.objectParent.transform);
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
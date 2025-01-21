using UnityEngine;

public class TreeNode : DamageableResourceNode
{
    [SerializeField] GameObject crown;
    [SerializeField] GameObject trunkPrefab;
    public override void Harvest()
    {
        // 나무를 자원 딕셔너리에서 지우고 비활성화
        EnvironmentManager.Instance.natureResources.Remove(transform.position);
        gameObject.SetActive(false);

        // 밑동 생성하고 자원 딕셔너리에 추가
        GameObject go = Instantiate(trunkPrefab,transform.position, Quaternion.identity, EnvironmentManager.Instance.VoronoiMapGenerator.objectParent.transform);
        EnvironmentManager.Instance.natureResources.Add(transform.position, new ResourceObject
        {
            position = new Vector2Int((int)transform.position.x, (int)transform.position.y),
            dataName = go.GetComponent<ResourceNode>().Data.name,
            isDamageable = true,
            gameObject = go
        });
    }
}

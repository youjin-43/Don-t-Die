using System;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    VoronoiMapGenerator mapGenerator;                       // Map Manager를 따로 만들지 Generator를 매니저처럼 쓸지 고민 중...
    [SerializeField] NatureResourceData natureResourceData;

    protected virtual void Init()
    {
        // 임시임. 나중에 GameManager에서 받아오는 식으로 수정.
        mapGenerator = transform.parent.parent.GetComponent<VoronoiMapGenerator>();
    }

    private void Start()
    {
        Init();
    }

    protected void Harvest()
    {
        // 수확이 되면 objectMap에서 정보를 지우고 비활성화한다. 
        // Object Pooling을 써서 비활성화/활성화할지 아예 destroy instantiate를 해버릴지 고민중 ㄱ-
        mapGenerator.objectMap.ClearTiles(new Vector2Int((int)transform.position.x, (int)transform.position.y), natureResourceData.Width, natureResourceData.Height);
        gameObject.SetActive(false);
    }
}

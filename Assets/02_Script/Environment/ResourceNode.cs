using System;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] VoronoiMapGenerator mapGenerator;
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
        mapGenerator.objectMap.ClearTiles(new Vector2Int((int)transform.position.x, (int)transform.position.y), natureResourceData.Width, natureResourceData.Height);
        gameObject.SetActive(false);
    }
}

using UnityEngine;

public class Campfire : MonoBehaviour
{
    public float maxDurability;
    public float currentDurability;

    float timeScale = 180f;

    void Start()
    {
        currentDurability = maxDurability;
    }

    
    void Update()
    {
        currentDurability -= Time.deltaTime * timeScale;

        if (currentDurability <= 0)
        {
            Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            EnvironmentManager.Instance.objectMap.ClearTiles(pos, 1, 1);
            Destroy(gameObject);
        }
    }
}

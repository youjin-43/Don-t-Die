using UnityEngine;
using UnityEngine.Rendering.Universal; // Light2D 사용 

public class Campfire : MonoBehaviour
{
    [Header("Durability")]
    [SerializeField] float maxDurability = 15f;
    public float currentDurability;

    [Header("Light")]
    [SerializeField] Light2D fireLight; // 불빛 조절용 Light 컴포넌트 - 인스펙터에서 할당
    [SerializeField] Animator animator; // 인스펙터에서 할당
    [SerializeField] float intensity;

    [SerializeField] GameObject fireObj;

    //float timeScale = 180f;

    void Start()
    {
        currentDurability = maxDurability;
    }

    void Update()
    {
        if(currentDurability > 0)
        {
            //currentDurability -= Time.deltaTime * timeScale;
            currentDurability -= Time.deltaTime;
            UpdateFireLight();

            if (currentDurability <= 0)
            {
                fireLight.intensity = 0;
                Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
                EnvironmentManager.Instance.objectMap.ClearTiles(pos, 1, 1);
                Destroy(gameObject);
            }
        }

    }

    public void AddDurability(float amount)
    {
        currentDurability += amount;
    }

    void UpdateFireLight()
    {
        if (fireLight != null)
        {
            intensity = Mathf.Lerp(0, 1, currentDurability / maxDurability); // 0~1 사이 값 조절
            fireLight.intensity = intensity;
            animator.SetFloat("intensity", intensity);
        }
    }
}

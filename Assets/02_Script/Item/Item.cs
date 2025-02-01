using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] public ItemData ItemData;
    [SerializeField] int currentDurability;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void SetItemData(ItemData itemData)
    {
        ItemData = itemData;
        if (ItemData.Image == null)
        {
            DebugController.Log($"{itemData.name}의 이미지가 설정되지 않았습니다.");
            return;
        }
        GetComponent<SpriteRenderer>().sprite = itemData.Image;
    }

    public void Spread(Vector2 start, Vector2 target, float speedMultiflier = 2f)
    {
        StartCoroutine(SpreadRoutine(start, target, speedMultiflier));
    }

    IEnumerator SpreadRoutine(Vector2 start, Vector2 target, float speedMultiflier = 2f)
    {
        float timer = 0f;
        float maxHeight = Random.Range(0.5f, 0.8f);

        while (timer < 1f)
        {
            timer += Time.deltaTime * speedMultiflier;

            float curveTime = timer / 1f;
            float height = curve.Evaluate(curveTime);

            height = Mathf.Lerp(0f, maxHeight, height);

            transform.position = Vector2.Lerp(start, target, curveTime) + new Vector2(0f, height);

            yield return null;
        }
    }


    float initialForce = 2f;
    float damping = 1f;
    float frequency = 8f;
    float time = 1.2f;

    Vector3 startPosition;
    float xDir;
    float xPower;

    public void DropEffect()
    {
        StartCoroutine(DropEffectCoroutine());

        startPosition = GameManager.Instance.GetPlayerPos();

        // RandomDir
        xDir = Random.Range(-1, 1);
        xPower = Random.Range(1f, 2f);


        if (xDir < 0f)
        {
            xDir = -1;
        }
        else
        {
            xDir = 1;
        }

    }

    IEnumerator DropEffectCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            // Y = A*e^(-kt)*|sin(Bt)|
            float height = initialForce * Mathf.Exp(-damping * elapsedTime) * Mathf.Abs(Mathf.Sin(frequency * elapsedTime));

            transform.position = startPosition + new Vector3(elapsedTime * xDir * xPower, height, 0);
            
            yield return null;
        }
    }
}
using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] public ItemData ItemData;

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

    public void Spread(Vector2 start, Vector2 target, float duration = 0.5f)
    {
        StartCoroutine(SpreadRoutine(start, target, duration));
    }

    IEnumerator SpreadRoutine(Vector2 start, Vector2 target, float duration = 0.5f)
    {
        float timer = 0f;
        float maxHeight = Random.Range(0.5f, 0.8f);

        while (timer < 1f)
        {
            timer += Time.deltaTime;

            float curveTime = timer / duration;
            float height = curve.Evaluate(curveTime);

            height = Mathf.Lerp(0f, maxHeight, height);

            transform.position = Vector2.Lerp(start, target, curveTime) + new Vector2(0f, height);

            yield return null;
        }
    }
}
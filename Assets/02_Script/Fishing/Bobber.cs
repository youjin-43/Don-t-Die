using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] List<ItemData> fishDatas;               // 낚일 수 있는 물고기들
    public PlayerFishingAction player;

    public void Throw(Vector3 start, Vector3 target, bool deactivate = false, bool getFish = false)
    {
        StopAllCoroutines();
        StartCoroutine(ThrowRoutine(start, target, deactivate, getFish));
    }

    IEnumerator ThrowRoutine(Vector2 start, Vector2 target, bool deactivate = false, bool getFish = false)
    {
        Item fish = null;
        float timer = 0f;
        float maxHeight = Random.Range(0.5f, 0.8f);

        if (getFish)
        {
            fish = InstantiateFish();
        }

        while (timer < 0.5f)
        {
            timer += Time.deltaTime;

            float curveTime = timer / 0.5f;
            float height = curve.Evaluate(curveTime);

            height = Mathf.Lerp(0f, maxHeight, height);

            transform.position = Vector2.Lerp(start, target, curveTime) + new Vector2(0f, height);

            if (getFish)
            {
                fish.transform.position = Vector2.Lerp(start, target, curveTime) + new Vector2(0f, height);
            }

            yield return null;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f, LayerMask.GetMask("Water"));
        if (colliders.Length == 0)
        {
            player.CatchBobber();
        }

        gameObject.SetActive(!deactivate);
    }

    public Item InstantiateFish()
    {
        int randIdx = Random.Range(0, fishDatas.Count);
        Item fish = PoolManager.Instance.InstantiateItem(fishDatas[randIdx]);
        fish.transform.position = transform.position;

        return fish;
    }
}

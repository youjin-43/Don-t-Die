using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    public PlayerFishingAction player;

    public void Throw(Vector3 start, Vector3 target, bool deactivate = false)
    {
        StartCoroutine(ThrowRoutine(start, target, deactivate));
    }

    IEnumerator ThrowRoutine(Vector2 start, Vector2 target, bool deactivate = false, float duration = 0.5f)
    {
        float timer = 0f;
        float maxHeight = Random.Range(0.5f, 0.8f);

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float curveTime = timer / duration;
            float height = curve.Evaluate(curveTime);

            height = Mathf.Lerp(0f, maxHeight, height);

            transform.position = Vector2.Lerp(start, target, curveTime) + new Vector2(0f, height);

            yield return null;
        }

        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f, LayerMask.GetMask("Water"));
        //if (colliders.Length == 0)
        //{
        //    player.CatchBobber();
        //}

        gameObject.SetActive(!deactivate);
    }
}

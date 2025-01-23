using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetLineRenderer(Vector3 tipPosition)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, tipPosition);
    }

    public void Throw(Vector3 start, Vector3 target)
    {
        StartCoroutine(ThrowRoutine(start, target));
    }

    IEnumerator ThrowRoutine(Vector2 start, Vector2 target, float duration = 0.5f)
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

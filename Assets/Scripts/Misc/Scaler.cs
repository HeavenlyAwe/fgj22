using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(2.0f, 2.0f, 2.0f);
    public Vector3 startingScale = new Vector3(1.0f, 1.0f, 1.0f);
    public float duration = 1.0f;
    public bool destroyWhenFinished = true;

    public AnimationCurve animationCurve;

    IEnumerator ScaleUpAndDown(Transform transform, Vector3 upScale, float duration)
    {
        Vector3 initialScale = transform.localScale;

        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            float curvePercent = animationCurve.Evaluate(time/duration);
            transform.localScale = Vector3.LerpUnclamped(initialScale, upScale, curvePercent);

            //float progress = Mathf.PingPong(time, duration) / duration;
            //transform.localScale = Vector3.Lerp(initialScale, upScale, progress);
            yield return null;
        }

        transform.localScale = initialScale;

        if (destroyWhenFinished)
        {
            Destroy(transform.gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = startingScale;
        StartCoroutine(ScaleUpAndDown(transform, targetScale, duration));
    }
}

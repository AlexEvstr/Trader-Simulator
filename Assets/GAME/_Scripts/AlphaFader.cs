using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaFader : MonoBehaviour
{
    public List<Image> points;
    private float totalDuration = 3f;
    private float fadeTime;

    void Start()
    {
        fadeTime = totalDuration / points.Count;
        SetAlpha(0.1f);
    }

    public void StartFading()
    {
        StopAllCoroutines();
        StartCoroutine(FadePoints());
    }

    private void SetAlpha(float alpha)
    {
        foreach (var point in points)
        {
            Color color = point.color;
            color.a = alpha;
            point.color = color;
        }
    }

    private IEnumerator FadePoints()
    {
        foreach (var point in points)
        {
            float elapsedTime = 0f;
            Color startColor = point.color;
            Color endColor = startColor;
            endColor.a = 1f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                point.color = Color.Lerp(startColor, endColor, elapsedTime / fadeTime);
                yield return null;
            }

            point.color = endColor;
        }

        float elapsedTimeReset = 0f;
        while (elapsedTimeReset < fadeTime)
        {
            elapsedTimeReset += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0.1f, elapsedTimeReset / fadeTime);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0.1f);
    }
}
using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeDuration = 0.5f;

    IEnumerator Fade(float targetTransparency)
    {
        float start = canvasGroup.alpha;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, targetTransparency, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetTransparency;
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1));
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0));
    }
}
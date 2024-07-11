using System.Collections;
using UnityEngine;

public class ObjectBFader : MonoBehaviour
{
    public float fadeDuration = 2.0f;
    private bool isFading = false;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void StartFadingOut()
    {
        if (!isFading && gameObject.activeSelf) 
        {
            StartCoroutine(FadeOut());
        }
    }

    public void StartFadingIn()
    {
        if (!isFading && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetMaterialAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetMaterialAlpha(0);
        gameObject.SetActive(false);
        isFading = false;
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            SetMaterialAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetMaterialAlpha(1);
        isFading = false;
    }

    private void SetMaterialAlpha(float alpha)
    {
        Color color = rend.material.color;
        color.a = alpha;
        rend.material.color = color;
    }
    
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFade : MonoBehaviour
{

    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeImage = GetComponent<Image>();

        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        // Começa o fade-in automaticamente
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        float a = fadeImage.color.a;

        while (a > 0f)
        {
            a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = new Color(0,0,0,a);
    }

    public void FadeOutAndRestart()
    {
        Debug.Log("[ScreenFade] FadeOut START");
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        float a = fadeImage.color.a;

        while (a > 1f)
        {
            a += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0,0,0,a);
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

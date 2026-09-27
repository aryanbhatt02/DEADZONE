using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{    
    public static SceneFader instance;

    public CanvasGroup canvasGroup;
    public float speed = 5f;
    void Awake()
    {        
        instance = this;
        canvasGroup.alpha = 1f;
    }

    void Start()
    {
        StartCoroutine(FadeIn());        
    }

    IEnumerator FadeIn()
    {
        while(canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= speed *Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
    public IEnumerator StartNewGameFadeOut()
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += speed * Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("Playing");
    }

    public IEnumerator MainMenuFadeOut()
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += speed * Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator ExitGameFadeOut()
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += speed * Time.deltaTime;
            yield return null;
        }
        Application.Quit();
    }
}

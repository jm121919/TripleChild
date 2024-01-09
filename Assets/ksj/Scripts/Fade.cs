using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image panel;
    public Image Image;
    private bool isCall;
    Coroutine fadeInCou;
    Coroutine fadeOutCou;
    private void Start()
    {
        isCall = false;
    }
    private void Update()
    {
        if (GameManager.Instance.getFade)
        {
            FadeOn();
            GameManager.Instance.getFade = false;
        }
    }
    void FadeOn()
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        DontDestroyOnLoad(gameObject);
        Debug.Log("버튼클릭");

        panel.gameObject.SetActive(true);

        if (scene.name == "MainGame")
        {
            Image.gameObject.SetActive(true);
        }

        PauseGame();
        fadeInCou = StartCoroutine(FadeInCo());
    }
    void PauseGame()
    {
        Time.timeScale = 0;
    }
    void ResumeGame()
    {
        Time.timeScale = 1;
        StopCoroutine(fadeInCou);
        StopCoroutine(fadeOutCou);
    }
    IEnumerator FadeInCo()
    {
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            panel.color = new Color(0, 0, 0, fadeCount);
        }
        Image.gameObject.SetActive(false);
        SceneManager.LoadScene(GameManager.Instance.fadeSceneName);
        fadeOutCou = StartCoroutine(FadeOutCo());
    }
    IEnumerator FadeOutCo()
    {
        float fadeCount = 1;
        yield return new WaitForSecondsRealtime(1f);
        while (fadeCount > 0)
        {
            fadeCount -= Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            panel.color = new Color(0, 0, 0, fadeCount);
        }
        panel.gameObject.SetActive(false);//
        GameManager.Instance.fadeSceneName = "MainGame";
        ResumeGame();
        
    }
}

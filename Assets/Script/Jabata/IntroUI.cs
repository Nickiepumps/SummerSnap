using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel;
    public Image pageImage;
    public CanvasGroup continueGroup;

    [Serializable]
    public class PageData
    {
        public Sprite image;
        [TextArea(2, 5)]
        public string[] elements;
    }

    [Header("Pages Data")]
    public PageData[] pages;

    private int currentPage = 0;
    private bool isFadingOut = false;

    private void Start()
    {
        panel.SetActive(true);
        ShowPage(0);
    }

    private void Update()
    {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.Space) && !isFadingOut)
        {
            NextPage();
        }
    }

    private void ShowPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pages.Length) return;

        currentPage = pageIndex;
        pageImage.sprite = pages[pageIndex].image;
    }
    private void NextPage()
    {
        if (currentPage + 1 < pages.Length)
        {
            ShowPage(currentPage + 1);
        }
        else
        {
            StartCoroutine(FadeOutAndLoad("Village"));
        }
    }
    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        isFadingOut = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        float duration = 1.5f;
        float t = 0f;

        Color startColorImage = pageImage.color;
        float startAlphaContinue = continueGroup != null ? continueGroup.alpha : 1f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);

            pageImage.color = new Color(startColorImage.r, startColorImage.g, startColorImage.b, alpha);

            if (continueGroup != null)
                continueGroup.alpha = Mathf.Lerp(startAlphaContinue, 0f, t / duration);

            yield return null;
        }
        panel.SetActive(false);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }
}

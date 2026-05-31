using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel;
    public Image pageImage;
    public TextMeshProUGUI storyText;
    public CanvasGroup continueGroup; // ⬅ ใช้ CanvasGroup ของ ContinueText

    [System.Serializable]
    public class PageData
    {
        public Sprite image;
        [TextArea(2, 5)]
        public string[] elements;
    }

    [Header("Pages Data")]
    public PageData[] pages;

    private int currentPage = 0;
    private int currentElement = 0;
    private bool isFadingOut = false;

    void Start()
    {
        panel.SetActive(true);
        ShowPage(0);
    }

    void Update()
    {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.Space) && !isFadingOut)
        {
            NextElement();
        }
    }

    void ShowPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pages.Length) return;

        currentPage = pageIndex;
        currentElement = 0;

        pageImage.sprite = pages[pageIndex].image;
        storyText.text = "";
        ShowElement();
    }

    void ShowElement()
    {
        if (currentElement < pages[currentPage].elements.Length)
        {
            if (storyText.text == "")
                storyText.text = pages[currentPage].elements[currentElement];
            else
                storyText.text += "\n" + pages[currentPage].elements[currentElement];
        }
    }

    void NextElement()
    {
        currentElement++;

        if (currentElement < pages[currentPage].elements.Length)
        {
            ShowElement();
        }
        else
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
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        isFadingOut = true;

        // โหลด Scene ถัดไปแบบ Additive ก่อน
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Fade UI ปัจจุบันออก
        float duration = 1.5f;
        float t = 0f;

        Color startColorImage = pageImage.color;
        Color startColorText = storyText.color;
        float startAlphaContinue = continueGroup != null ? continueGroup.alpha : 1f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);

            pageImage.color = new Color(startColorImage.r, startColorImage.g, startColorImage.b, alpha);
            storyText.color = new Color(startColorText.r, startColorText.g, startColorText.b, alpha);

            if (continueGroup != null)
                continueGroup.alpha = Mathf.Lerp(startAlphaContinue, 0f, t / duration);

            yield return null;
        }

        // ปิด Panel Intro
        panel.SetActive(false);

        // Unload Scene เก่าออก
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }
}

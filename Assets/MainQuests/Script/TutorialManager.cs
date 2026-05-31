using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;      // Panel Tutorial
    public GameObject gameplayObjects;    // Object เกมหลักทั้งหมด
    public AudioSource audioSource;       // AudioSource สำหรับเสียงคลิก
    public AudioClip clickSound;          // เสียงคลิกเริ่มเกม

    private CanvasGroup canvasGroup;
    private bool tutorialActive = true;

    void Awake()
    {
        // ปิด gameplay ตั้งแต่เริ่ม ไม่ให้โผล่
        if (gameplayObjects != null)
            gameplayObjects.SetActive(false);

        // เปิด tutorial panel
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            canvasGroup = tutorialPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = tutorialPanel.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
        }
    }

    void Start()
    {
        Time.timeScale = 0f; // หยุดเกมชั่วคราว
        StartCoroutine(FadeInPanel());
    }

    void Update()
    {
        if (tutorialActive && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(FadeOutAndStartGame());
        }
    }

    IEnumerator FadeInPanel()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 2f;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
    }

    IEnumerator FadeOutAndStartGame()
    {
        tutorialActive = false;

        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 2f;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        tutorialPanel.SetActive(false);

        if (gameplayObjects != null)
            gameplayObjects.SetActive(true);

        Time.timeScale = 1f; // เริ่มเกม
    }
}

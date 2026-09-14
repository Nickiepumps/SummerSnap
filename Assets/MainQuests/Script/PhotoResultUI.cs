using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class StarThreshold
{
    public int scoreFor1Star = 20;
    public int scoreFor2Stars = 35;
    public int scoreFor3Stars = 60;
}

public class PhotoResultUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject resultPanelComplete;   // ชี้ไปที่ "ResultPanelComplete" (Panel แม่)
    public GameObject popupPanel;            // ชี้ไปที่ "Complete" (Panel ที่จะ Animate)

    [Header("Result Panel Buttons")]
    [SerializeField] private Button buttonCompleteResultOk;
    [SerializeField] private Button buttonCompleteResultTryAgain;
    [SerializeField] private Button buttonFailedResultOk;
    [SerializeField] private Button buttonFailedResultTryAgain;

    [Header("Scoring")]
    public int mainQuestLevel = 1;

    [Header("Stars")]
    public Image[] starImages;
    public Sprite starComplete;
    public Sprite starFail;

    [Header("Condition Icons")]
    public Image timeIcon;
    public Sprite timeComplete;
    public Sprite timeFail;

    public Image npcIcon;
    public Sprite npcComplete;
    public Sprite npcFail;

    public Image eyeIcon;
    public Sprite eyeComplete;
    public Sprite eyeFail;

    public Image fogIcon;
    public Sprite fogComplete;
    public Sprite fogFail;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip starSound;

    [Header("Star Thresholds")]
    public StarThreshold starThreshold;

    [Header("Next Scene")]
    public string nextSceneName = "MainMenu";

    private CanvasGroup popupCanvasGroup;
    private RectTransform popupRect;
    private Vector3[] starOriginalScales;

    private Sprite photoSprite;
    void Start()
    {
        // Result panel buttons initialize
        buttonCompleteResultOk.onClick.AddListener(OnOK);
        buttonFailedResultOk.onClick.AddListener(OnOK);
        buttonCompleteResultTryAgain.onClick.AddListener(OnPlayAgain);
        buttonFailedResultTryAgain.onClick.AddListener(OnPlayAgain);

        // ปิดทั้ง Panel แม่ก่อน
        resultPanelComplete.SetActive(false);

        // เพิ่ม CanvasGroup ถ้ายังไม่มี
        popupCanvasGroup = popupPanel.GetComponent<CanvasGroup>();
        if (popupCanvasGroup == null)
            popupCanvasGroup = popupPanel.AddComponent<CanvasGroup>();

        popupRect = popupPanel.GetComponent<RectTransform>();

        // เก็บขนาดดั้งเดิมของดาว
        starOriginalScales = new Vector3[starImages.Length];
        for (int i = 0; i < starImages.Length; i++)
        {
            starOriginalScales[i] = starImages[i].transform.localScale;
        }
    }

    public void ShowResult(int score, bool timeOk, bool npcOk, bool blinkOk, bool fogOk)
    {
        StartCoroutine(ShowPopupAnimated(score, timeOk, npcOk, blinkOk, fogOk));
    }

    private IEnumerator ShowPopupAnimated(int score, bool timeOk, bool npcOk, bool blinkOk, bool fogOk)
    {
        yield return new WaitForSecondsRealtime(0.3f);

        // เปิด Panel แม่ทั้งหมด (รวม BG_black)
        resultPanelComplete.SetActive(true);

        // เตรียมค่าเริ่มต้นสำหรับ Animate Panel Complete
        popupCanvasGroup.alpha = 0;
        popupRect.localScale = Vector3.one * 0.8f;

        // ตั้งค่าภาพไอคอนตามผลลัพธ์
        timeIcon.sprite = timeOk ? timeComplete : timeFail;
        npcIcon.sprite = npcOk ? npcComplete : npcFail;
        eyeIcon.sprite = blinkOk ? eyeComplete : eyeFail;
        fogIcon.sprite = fogOk ? fogComplete : fogFail;

        // Fade + Scale Panel Complete
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 2f;
            popupCanvasGroup.alpha = Mathf.Lerp(0, 1, t);
            popupRect.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, t);
            yield return null;
        }

        // คำนวณจำนวนดาว
        int starCount = PhotoStarCalculator.CalculateStars(score, starThreshold);

        // Map stars -> level score per level
        int levelScore = 0;
        switch (mainQuestLevel)
        {
            case 1: levelScore = (starCount == 3 ? 20 : starCount == 2 ? 15 : starCount == 1 ? 10 : 0); break;
            case 2: levelScore = (starCount == 3 ? 35 : starCount == 2 ? 25 : starCount == 1 ? 15 : 0); break;
            case 3: levelScore = (starCount == 3 ? 45 : starCount == 2 ? 30 : starCount == 1 ? 20 : 0); break;
            case 4: levelScore = (starCount == 3 ? 50 : starCount == 2 ? 40 : starCount == 1 ? 30 : 0); break;
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddMainQuestScore(mainQuestLevel, levelScore);
        }
        // รีเซ็ตดาวทั้งหมดเป็น fail
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = starFail;
            starImages[i].transform.localScale = starOriginalScales[i];
        }

        // แสดงดาว complete ทีละดวง
        for (int i = 0; i < starCount; i++)
        {
            starImages[i].sprite = starComplete;

            if (audioSource != null && starSound != null)
                audioSource.PlayOneShot(starSound);

            Vector3 targetScale = starOriginalScales[i];
            starImages[i].transform.localScale = Vector3.zero;

            float s = 0f;
            while (s < 1f)
            {
                s += Time.unscaledDeltaTime * 5f;
                float scale = Mathf.Sin(s * Mathf.PI * 0.5f); // ease out
                starImages[i].transform.localScale = targetScale * scale;
                yield return null;
            }

            starImages[i].transform.localScale = targetScale;
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    public void OnConfirmButton()
    {
        StartCoroutine(HidePopupAnimated());
    }

    private IEnumerator HidePopupAnimated()
    {
        float t = 0f;
        Vector3 startScale = popupRect.localScale;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 2f;
            popupCanvasGroup.alpha = Mathf.Lerp(1, 0, t);
            popupRect.localScale = Vector3.Lerp(startScale, Vector3.one * 0.8f, t);
            yield return null;
        }
        resultPanelComplete.SetActive(false);  // ปิดทั้งก้อน
    }

    public void OnPlayAgain()
    {
        Time.timeScale = 1f;

        ClearPhoto();

        if (AudioManager.Instance != null)
        {
            // ✅ ให้เล่น QuestBGM อีกครั้ง หลังจากรีโหลด
            SceneBGM sceneBGM = FindObjectOfType<SceneBGM>();
            if (sceneBGM != null && sceneBGM.isQuestBGM && sceneBGM.bgm != null)
            {
                AudioManager.Instance.PlayTemporaryBGMImmediate(sceneBGM.bgm);
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddPhoto(Sprite photo)
    {
        photoSprite = photo;
    }
    private void ClearPhoto()
    {
        photoSprite = null;
    }
    public void OnOK()
    {
        Time.timeScale = 1f;

        if (PhoneAlbumUI_Work.Instance != null)
        {
            PhoneAlbumUI_Work.Instance.AddPhoto(photoSprite);
        }
        if (MainQuestPhotoStorage.Instance != null)
        {
            MainQuestPhotoStorage.Instance.AddPhoto(photoSprite);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RestoreLastBGM();
        }

        GameObject player = GameManager.Instance != null ? GameManager.Instance.GetPlayer() : null;
        if (player != null)
        {
            PlayerPositionManager.SavePlayerPosition(player.transform.position);
        }

        GameManager.Instance.TogglePhotoshootStatus(false);
        SceneManager.LoadScene(nextSceneName);
    }
}

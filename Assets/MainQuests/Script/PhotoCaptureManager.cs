using System.Collections;
using UnityEngine;

public class PhotoCaptureManager : MonoBehaviour
{
    [SerializeField] private PhotoResultUI photoResultUI;

    public int baseScore = 12;

    public AudioClip shutterSound;
    private AudioSource audioSource;

    private bool hasCaptured = false;
    private bool isGameOver = false; // ป้องกันการถ่ายรูปหลังเกมจบ

    [Header("UI References")]
    public RectTransform frameUI;
    public GameObject cameraFrameUI;
    public GameObject timeSliderUI;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (isGameOver) return; // เกมจบแล้ว ห้ามถ่ายเพิ่ม

        if (Input.GetKeyDown(KeyCode.Space) && !hasCaptured)
        {
            StartCoroutine(CapturePhoto());
            hasCaptured = true;
        }
    }

    IEnumerator CapturePhoto()
    {
        // เล่นเสียงชัตเตอร์
        if (shutterSound != null)
            audioSource.PlayOneShot(shutterSound);

        // ปิด UI ก่อนถ่าย
        if (cameraFrameUI != null) cameraFrameUI.SetActive(false);
        if (timeSliderUI != null) timeSliderUI.SetActive(false);

        yield return new WaitForEndOfFrame();

        // จับภาพตามขนาด frameUI
        Vector3[] corners = new Vector3[4];
        frameUI.GetWorldCorners(corners);

        float x = corners[0].x;
        float y = corners[0].y;
        float width = corners[2].x - corners[0].x;
        float height = corners[2].y - corners[0].y;

        Texture2D screenTex = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
        screenTex.ReadPixels(new Rect(x, y, width, height), 0, 0);
        screenTex.Apply();

        Sprite photoSprite = Sprite.Create(screenTex, new Rect(0, 0, screenTex.width, screenTex.height), new Vector2(0.5f, 0.5f));

        photoResultUI.AddPhoto(photoSprite);

        Time.timeScale = 0f;

        // --- คำนวณคะแนน ---
        int npcPenalty = FindObjectOfType<NPCScoreEvaluator>().EvaluateTotalNPCPenalty();

        int blinkPenalty = 0;
        var influencer = FindObjectOfType<InfluencerBlink>();
        if (influencer != null && influencer.IsEyesClosed())
            blinkPenalty = 3;

        int fogPenalty = 0;
        var fogController = FindObjectOfType<FogController>();
        if (fogController != null && fogController.fogImage.activeSelf)
            fogPenalty = 7;

        int finalScore = baseScore - (npcPenalty + blinkPenalty + fogPenalty);

        bool npcOk = npcPenalty == 0;
        bool blinkOk = blinkPenalty == 0;
        bool fogOk = fogPenalty == 0;
        bool timeOk = true;

        // --- หาดาวจาก ResultUI ---
        var resultUI = FindObjectOfType<PhotoResultUI>();
        int stars = 0;
        if (resultUI != null)
        {
            stars = PhotoStarCalculator.CalculateStars(finalScore, resultUI.starThreshold);
        }

        // --- Preview ---
        var previewUI = FindObjectOfType<PhotoPreviewUI>();
        if (previewUI != null)
        {
            previewUI.ShowPreview(photoSprite, stars);
            yield return new WaitForSecondsRealtime(2.3f);
        }

        // ⭐ แสดงผลลัพธ์
        if (resultUI != null)
        {
            resultUI.ShowResult(finalScore, timeOk, npcOk, blinkOk, fogOk);
        }
    }

    // เรียกตอนเวลาหมด
    public void SetGameOver()
    {
        isGameOver = true;
    }
}

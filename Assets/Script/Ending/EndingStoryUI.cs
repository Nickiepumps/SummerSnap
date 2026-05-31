using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[System.Serializable]
public class StoryLine
{
    [TextArea(2, 5)]
    public string text;       // ข้อความ
    public Sprite image;      // รูปประกอบ (optional)
}

public class EndingStoryUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI storyText;
    public Image storyImage;       // UI Image สำหรับรูปประกอบ
    public GameObject panel;

    [Header("Story Content")]
    public StoryLine[] storyLines;

    private int currentIndex = 0;
    private Action onStoryFinished;

    void Start()
    {
        if (panel != null) panel.SetActive(false);
        if (storyImage != null) storyImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (panel != null && panel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                ShowNextLine();
            }
        }
    }

    // เริ่มเรื่อง
    public void StartStory(Action onFinished = null)
    {
        if (storyLines == null || storyLines.Length == 0)
        {
            Debug.LogWarning($"[EndingStoryUI] ไม่มีข้อความใน {gameObject.name}");
            return;
        }

        currentIndex = 0;
        panel.SetActive(true);
        storyText.text = storyLines[currentIndex].text;
        ShowImage(storyLines[currentIndex].image);

        onStoryFinished = onFinished;
    }

    // แสดงบรรทัดต่อไป
    void ShowNextLine()
    {
        currentIndex++;

        if (currentIndex < storyLines.Length)
        {
            storyText.text = storyLines[currentIndex].text;
            ShowImage(storyLines[currentIndex].image);
        }
        else
        {
            panel.SetActive(false);
            onStoryFinished?.Invoke();
        }
    }

    // แสดงหรือซ่อนรูปทันที
    void ShowImage(Sprite newImage)
    {
        if (storyImage == null) return;

        if (newImage != null)
        {
            storyImage.sprite = newImage;
            storyImage.gameObject.SetActive(true);
        }
        else
        {
            storyImage.gameObject.SetActive(false);
        }
    }
}

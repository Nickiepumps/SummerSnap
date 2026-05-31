using UnityEngine;
using TMPro;

public class FogButton : MonoBehaviour
{
    public TMP_Text countText;
    public AudioClip clickSound; // 🎵 เพิ่มเสียงตอนคลิก
    private AudioSource audioSource;

    private int remainingClicks;
    private FogController controller;

    public void Setup(int clickCount, FogController fogController)
    {
        remainingClicks = clickCount;
        controller = fogController;
        countText.text = remainingClicks.ToString();

        // เตรียม AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    public void OnClick()
    {
        // 🔊 ใช้ AudioSource แบบยิงทิ้ง
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, 1f);
        }

        remainingClicks--;
        if (remainingClicks <= 0)
        {
            controller.RemoveButton(gameObject);
        }
        else
        {
            countText.text = remainingClicks.ToString();
        }
    }
}

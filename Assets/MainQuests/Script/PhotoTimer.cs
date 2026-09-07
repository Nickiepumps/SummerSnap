using UnityEngine;
using UnityEngine.UI;

public class PhotoTimer : MonoBehaviour
{
    public float totalTime = 15f; // เวลาทั้งหมดในแต่ละ Stage
    private float remainingTime;
    [SerializeField] private PhotoCaptureManager photoCaptureManager;

    public Slider timeSlider; // UI slider แสดงเวลา
    public Image fillImage; // Image ของ Fill Area เพื่อนำมาเปลี่ยนสี
    public Color greenColor = new Color32(0x8f, 0xdb, 0x60, 0xff);  // #8fdb60
    public Color yellowColor = new Color32(0xef, 0xcf, 0x4f, 0xff); // #efcf4f
    public Color redColor = new Color32(0xaa, 0x2a, 0x35, 0xff);    // #aa2a35

    public GameObject gameOverPanel; // UI Game Over (ปิดไว้ใน Editor แล้วเปิดเมื่อหมดเวลา)
    
    private bool isRunning = true;

    void Start()
    {
        remainingTime = totalTime;
        timeSlider.maxValue = totalTime;
        timeSlider.value = totalTime;
        UpdateColor();
    }

    void Update()
    {
        if (!isRunning) return;

        remainingTime -= Time.deltaTime;
        timeSlider.value = remainingTime;

        UpdateColor();

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            isRunning = false;
            GameOver();
        }
    }

    void UpdateColor()
    {
        float percent = remainingTime / totalTime;

        if (percent > 0.6f)
            fillImage.color = greenColor;
        else if (percent > 0.3f)
            fillImage.color = yellowColor;
        else
            fillImage.color = redColor;
    }

    void GameOver()
    {
       
        Time.timeScale = 0f; // หยุดเวลาเกมทั้งเกม

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        
        // แจ้งให้ PhotoCaptureManager รู้ว่าเกมจบแล้ว
        if (photoCaptureManager != null)
        {
            photoCaptureManager.SetGameOver();
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void StartTimer(float newTime)
    {
        totalTime = newTime;
        remainingTime = totalTime;
        isRunning = true;
        timeSlider.maxValue = totalTime;
        timeSlider.value = totalTime;
        UpdateColor();
    }
}

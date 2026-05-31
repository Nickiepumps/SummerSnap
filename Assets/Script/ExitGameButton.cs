using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    public void ExitGame()
    {
        // สำหรับออกจากเกมจริง ๆ ตอน Build (Windows, Mac, etc.)
        Application.Quit();

        // ส่วนนี้ไว้ทดสอบใน Editor (จะเห็นแค่ข้อความใน Console)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

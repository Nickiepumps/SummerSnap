using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    // ใส่ชื่อ Scene ที่ต้องการเปลี่ยนไป (ต้องสะกดตรงกับใน Build Settings)
    public string sceneName;

    // เรียกฟังก์ชันนี้ตอนกดปุ่มหรือ Trigger อะไรก็ได้
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty! Please assign a scene name in the inspector.");
        }
    }
}

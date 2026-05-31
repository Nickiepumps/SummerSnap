using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // กันซ้ำ
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // ทำให้ไม่หายเวลาเปลี่ยน Scene
    }
}

using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    private static PersistentUI instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);   // ✅ ทั้งก้อน PersistentUI (รวม Canvas_PHONE, PhoneAlbumUI) จะอยู่ตลอดเกม
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

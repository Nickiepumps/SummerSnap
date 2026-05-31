using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class WarningManager : MonoBehaviour
{
    public static WarningManager Instance;

    [Header("UI Reference")]
    public Image warningImage;   // ลาก Image ที่เป็น Warning ใส่ตรงนี้

    private float warningTimer = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // จัด Canvas ให้ลอยบนสุด
            var canvas = GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 9999;

            if (warningImage != null)
                warningImage.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (warningTimer > 0)
        {
            warningTimer -= Time.unscaledDeltaTime;
            if (warningTimer <= 0 && warningImage != null)
            {
                warningImage.gameObject.SetActive(false);
            }
        }
    }

    public void ShowWarning(float duration = 2f)
    {
        if (warningImage == null) return;

        if (!warningImage.gameObject.activeSelf)
        {
            warningImage.gameObject.SetActive(true);
            warningImage.transform.SetAsLastSibling();
        }

        warningTimer = duration;
    }
}

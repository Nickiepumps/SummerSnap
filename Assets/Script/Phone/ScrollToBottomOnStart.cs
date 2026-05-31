using UnityEngine;
using UnityEngine.UI;

public class ScrollToBottomOnStart : MonoBehaviour
{
    private ScrollRect scrollRect;

    void Awake()
    {
        // หาคอมโพเนนต์ ScrollRect ในตัวเองหรือลูก
        scrollRect = GetComponent<ScrollRect>();
        if (scrollRect == null)
            scrollRect = GetComponentInChildren<ScrollRect>();
    }

    void Start()
    {
        if (scrollRect == null)
        {
            Debug.LogError("ScrollRect not found!");
            return;
        }

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }
}

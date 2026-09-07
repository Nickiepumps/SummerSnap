using UnityEngine;

public class PhotoQuestObject : MonoBehaviour
{
    [SerializeField] private PhotoPreviewSideQuests photoPreviewSideQuests;

    [Header("ตั้ง ID ให้แต่ละ Object ไม่ซ้ำกัน")]
    public string questId;
    public Sprite photoSprite;

    private bool clicked = false;
    private bool isTaken = false;

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.HasTaken(questId))
        {
            isTaken = true;
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance != null && GameManager.Instance.HasTaken(questId))
        {
            Debug.Log($"[PhotoQuestObject] {questId} เคยถ่ายไปแล้ว ห้ามถ่ายซ้ำ");
            return;
        }

        if (isTaken) return;
        if (clicked) return;

        clicked = true;

        if (photoSprite == null)
        {
            Debug.LogWarning($"{name} ไม่มี Sprite!");
            clicked = false; // reset กรณี fail
            return;
        }

        if (photoPreviewSideQuests != null)
        {
            photoPreviewSideQuests.ShowButtons(photoSprite, questId);
        }
    }

    public void MarkTaken()
    {
        isTaken = true;
        clicked = true;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MarkAsTaken(questId);
        }
    }

    // เรียกจาก PhotoPreviewSideQuests ตอนกดยกเลิก
    public void ResetClicked()
    {
        if (!isTaken) clicked = false;
    }
}

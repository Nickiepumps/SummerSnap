using UnityEngine;

public class PhotoQuestObject : MonoBehaviour
{
    [Header("ตั้ง ID ให้แต่ละ Object ไม่ซ้ำกัน")]
    public string questId;
    public Sprite photoSprite;

    private bool clicked = false;
    private bool isTaken = false;

    void Start()
    {
        // เช็คว่าถ่ายไปแล้วใน GameManager → ถ่ายไม่ได้อีก
        if (GameManager.Instance != null && GameManager.Instance.HasTaken(questId))
        {
            isTaken = true;
        }
    }

    void OnMouseDown()
    {
        // ถ่ายไปแล้วจริง ๆ ห้ามกด
        if (GameManager.Instance != null && GameManager.Instance.HasTaken(questId))
        {
            Debug.Log($"[PhotoQuestObject] {questId} เคยถ่ายไปแล้ว ห้ามถ่ายซ้ำ");
            return;
        }

        if (isTaken) return;   // ถ่ายเสร็จแล้วห้ามกด
        if (clicked) return;   // ป้องกัน spam กดรัว ๆ ระหว่าง popup ยังไม่ปิด

        clicked = true;

        if (photoSprite == null)
        {
            Debug.LogWarning($"{name} ไม่มี Sprite!");
            clicked = false; // reset กรณี fail
            return;
        }

        if (PhotoPreviewSideQuests.Instance != null)
        {
            PhotoPreviewSideQuests.Instance.ShowButtons(photoSprite, questId);
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

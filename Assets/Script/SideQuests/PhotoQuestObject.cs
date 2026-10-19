using UnityEngine;

public class PhotoQuestObject : MonoBehaviour
{
    [SerializeField] private PhotoPreviewSideQuests photoPreviewSideQuests;

    [Header("ตั้ง ID ให้แต่ละ Object ไม่ซ้ำกัน")]
    public string questId;
    public Sprite photoSprite;
    public Sprite photoFrameSprite;
    [SerializeField] private SOQuestPhotographDetail photographDetailSO;

    public bool clicked = false;
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
        if (GameManager.Instance != null && GameManager.Instance.HasTaken(questId)) return;
        if (isTaken) return;
        if (clicked) return;

        if (photoSprite == null)
        {
            clicked = false;
            return;
        }
        if (photoPreviewSideQuests != null)
        {
            clicked = true;
            photoPreviewSideQuests.ShowButtons(photoSprite, questId, this);
            photoPreviewSideQuests.currentClickedSideQuestSO = photographDetailSO;
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
    public void ResetClicked()
    {
        if (!isTaken) clicked = false;
    }
}

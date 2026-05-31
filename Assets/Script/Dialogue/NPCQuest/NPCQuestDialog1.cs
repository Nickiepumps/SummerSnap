using UnityEngine;

public class NPCQuestDialog1 : MonoBehaviour
{
    [Header("Dialogs")]
    public DialogueLine1[] beforeQuest;   // ยังไม่ได้รับเควส
    public DialogueLine1[] duringQuest;   // รับเควสแล้วแต่ยังไม่ครบ
    public DialogueLine1[] afterQuest;    // เก็บครบแล้ว ส่งเควสสำเร็จ

    void OnMouseDown()
    {
        NPCDialogManager1 manager = FindObjectOfType<NPCDialogManager1>();
        if (manager == null) return;

        if (!QuestManager1.Instance.IsFlagQuestActive() && !QuestManager1.Instance.IsFlagQuestCompleted())
        {
            // เริ่มคุยครั้งแรก → รับเควส
            manager.StartDialog(beforeQuest);
            QuestManager1.Instance.StartFlagQuest();
        }
        else if (QuestManager1.Instance.IsFlagQuestActive() && !QuestManager1.Instance.HasCollectedAllFlags())
        {
            // อยู่ระหว่างเควส
            manager.StartDialog(duringQuest);
        }
        else if (QuestManager1.Instance.HasCollectedAllFlags() && !QuestManager1.Instance.IsFlagQuestCompleted())
        {
            // เก็บครบ → มาส่งเควส
            manager.StartDialog(afterQuest);
            QuestManager1.Instance.CompleteFlagQuest();
        }
    }
}

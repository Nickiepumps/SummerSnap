using UnityEngine;

public class Flag1 : MonoBehaviour
{
    public string flagID; // ตั้งค่าใน Inspector ให้ไม่ซ้ำกัน

    private void OnMouseDown()
    {
        if (QuestManager1.Instance.IsFlagQuestActive())
        {
            QuestManager1.Instance.CollectFlag(flagID);
            Destroy(gameObject); // เก็บแล้วลบออกถาวร
        }
    }
}

using UnityEngine;

public class TriggerDialogEnd : MonoBehaviour
{
    public DialogManager dialogManager;
    public DialogData dialogData;

    [Header("Unique ID for this dialog")]
    public string dialogId;

    [Header("UI ที่จะแสดงหลังจบบทสนทนา")]
    public GameObject endDialogUI;

    private bool hasPlayed = false;
    private bool isCooldown = false;
    private float cooldownTime = 1f; // ⏳ หน่วงเวลา 1 วิ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasPlayed && !isCooldown)
        {
            if (dialogManager != null && dialogData != null)
            {
                Debug.Log($"[TriggerDialogEnd] Player ชน Trigger: {dialogId}");

                // บอก GameManager ว่า dialog นี้กำลังจะเล่น
                if (GameManager.Instance != null)
                    GameManager.Instance.SetCurrentDialogId(dialogId);

                // ตั้ง dialog และเริ่มเล่น
                dialogManager.SetDialog(dialogData);
                dialogManager.StartDialog();

                // ผูก callback → เมื่อจบบทสนทนา
                dialogManager.onDialogEnd = OnDialogEnd;

                hasPlayed = true;
            }
            else
            {
                Debug.LogWarning("[TriggerDialogEnd] dialogManager หรือ dialogData ยังไม่ได้เซ็ต!");
            }
        }
    }

    private void OnDialogEnd()
    {
        Debug.Log("[TriggerDialogEnd] จบบทสนทนาแล้ว!");

        if (endDialogUI != null)
        {
            endDialogUI.SetActive(true);
            Debug.Log("[TriggerDialogEnd] เปิด UI เรียบร้อย!");
        }
        else
        {
            Debug.LogWarning("[TriggerDialogEnd] ไม่มี endDialogUI ถูก assign!");
        }
    }

    // ✅ เรียกจากปุ่ม Cancel
    public void ResetTrigger()
    {
        hasPlayed = false;
        StartCoroutine(CooldownCoroutine());
    }

    private System.Collections.IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
}

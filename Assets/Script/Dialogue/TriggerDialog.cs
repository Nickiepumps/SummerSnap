using UnityEngine;

public class TriggerDialog : MonoBehaviour
{
    public DialogManager dialogManager;
    public DialogData dialogData;
    public bool usePowder = false;

    [Header("Unique ID for this dialog")]
    public string dialogId;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance != null && GameManager.Instance.HasTriggeredDialog(dialogId))
            {
                // เคยเล่นแล้ว → ออกจากฟังก์ชัน
                return;
            }

            if (dialogManager != null && dialogData != null)
            {
                GameManager.Instance.SetCurrentDialogId(dialogId);
                dialogManager.SetDialog(dialogData);
                dialogManager.StartDialog();

                if (usePowder)
                    GameManager.Instance.SetCurrentDialogPowdered(true);

                // บันทึกว่า Dialog ตัวนี้เล่นแล้ว
                GameManager.Instance.MarkDialogTriggered(dialogId);
            }
        }
    }
}

using System.Collections;
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
    private float cooldownTime = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasPlayed && !isCooldown)
        {
            if (dialogManager != null && dialogData != null)
            {
                Debug.Log($"[TriggerDialogEnd] Player ชน Trigger: {dialogId}");

                if (GameManager.Instance != null)
                    GameManager.Instance.SetCurrentDialogId(dialogId);

                dialogManager.SetDialog(dialogData);
                dialogManager.StartDialog();

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
        if (endDialogUI != null)
        {
            endDialogUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("[TriggerDialogEnd] ไม่มี endDialogUI ถูก assign!");
        }
    }
    public void ResetTrigger()
    {
        hasPlayed = false;
        StartCoroutine(CooldownCoroutine());
    }
    private IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
}

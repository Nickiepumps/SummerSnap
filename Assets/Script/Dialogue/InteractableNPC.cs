using UnityEngine;

public class InteractableNPC : MonoBehaviour
{
    public DialogManager dialogManager;
    public DialogData dialogData;
    public bool usePowder = false;
    public string dialogId;

    private bool hasInteracted = false;

    void OnMouseDown()
    {
        if (hasInteracted) return;

        if (GameManager.Instance.HasTriggeredDialog(dialogId))
            return; // เคยเล่นแล้ว

        if (dialogManager != null && dialogData != null)
        {
            GameManager.Instance.SetCurrentDialogId(dialogId);
            dialogManager.SetDialog(dialogData);
            dialogManager.StartDialog();

            if (usePowder)
                GameManager.Instance.SetCurrentDialogPowdered(true);

            GameManager.Instance.MarkDialogTriggered(dialogId);
            hasInteracted = true;
        }
    }
}

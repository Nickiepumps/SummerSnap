using UnityEngine;

public class NPCInteractTrigger : MonoBehaviour
{
    [Header("UI Icon")]
    public GameObject fIcon;

    [Header("Dialogue")]
    public DialogueLine[] lines;

    private bool isPlayerNear = false;
    private NPCDialogManager dialogManager;

    void Start()
    {
        dialogManager = FindObjectOfType<NPCDialogManager>();

        if (fIcon != null)
            fIcon.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            UpdateFIcon();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            UpdateFIcon();
        }
    }

    void Update()
    {
        if (isPlayerNear && dialogManager != null && !dialogManager.dialogPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F) && PhoneUIManager.Instance.isOpen == false)
            {
                dialogManager.StartDialog(lines);
                UpdateFIcon();
            }
        }
        UpdateFIcon();
    }

    void UpdateFIcon()
    {
        if (fIcon != null)
        {
            bool showF = isPlayerNear && (dialogManager == null || !dialogManager.dialogPanel.activeSelf);
            if (fIcon.activeSelf != showF)
                fIcon.SetActive(showF);
        }
    }
}

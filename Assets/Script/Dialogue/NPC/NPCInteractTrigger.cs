using UnityEngine;

public class NPCInteractTrigger : MonoBehaviour
{
    [Header("UI Icon")]
    public GameObject fIcon;           // รูปปุ่ม F (Sprite ของคุณ)

    [Header("Dialogue")]
    public DialogueLine[] lines;       // บทสนทนาของ NPC ตัวนี้

    private bool isPlayerNear = false;
    private NPCDialogManager dialogManager;

    void Start()
    {
        // หา DialogManager อัตโนมัติ
        dialogManager = FindObjectOfType<NPCDialogManager>();

        if (fIcon != null)
            fIcon.SetActive(false); // เริ่มเกมซ่อนไว้
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
            // กด F → เริ่ม Dialog
            if (Input.GetKeyDown(KeyCode.F) && PhoneUIManager.Instance.isOpen == false)
            {
                dialogManager.StartDialog(lines);
                UpdateFIcon(); // ซ่อน F ตอน Dialog เริ่ม
            }
        }

        // เช็คตลอดเวลา ถ้า Dialog จบและ Player ยังอยู่ → F โผล่อีกครั้ง
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

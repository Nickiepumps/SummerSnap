using UnityEngine;
using UnityEngine.UI;

public class InBuilding : MonoBehaviour
{
    [Header("ตั้ง ID สำหรับประตูนี้ (ห้ามซ้ำ)")]
    public string buildingId;

    [Header("Dialog")]
    public DialogManager dialogManager;
    public DialogData dialogData;
    public bool usePowder = false;

    [Header("UI")]
    public GameObject confirmPanel;   // Panel ที่มีปุ่ม เข้า / ยกเลิก
    public Button enterButton;
    public Button cancelButton;

    private bool clicked = false;     // ป้องกันกดรัว
    private bool isEntered = false;   // เข้าไปแล้ว (ครั้งเดียว)

    void Start()
    {
        // ถ้าเคยเข้าแล้วใน GameManager → ปิดการทำงาน
        if (GameManager.Instance != null && GameManager.Instance.HasTriggeredDialog(buildingId))
        {
            isEntered = true;
        }

        if (confirmPanel != null)
            confirmPanel.SetActive(false);

        if (enterButton != null)
            enterButton.onClick.AddListener(OnEnter);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancel);
    }

    void OnMouseDown()
    {
        // ถ้าเข้าไปแล้ว → ห้ามกดซ้ำ
        if (GameManager.Instance != null && GameManager.Instance.HasTriggeredDialog(buildingId))
        {
            Debug.Log($"[InBuilding] {buildingId} เคยเข้าไปแล้ว ห้ามซ้ำ");
            return;
        }

        if (isEntered) return;   // กันซ้ำ
        if (clicked) return;     // ป้องกันกดรัว ๆ ระหว่าง popup ยังเปิดอยู่

        clicked = true;

        // โชว์ UI ปุ่ม
        if (confirmPanel != null)
            confirmPanel.SetActive(true);
    }

    void OnEnter()
    {
        if (dialogManager != null && dialogData != null)
        {
            GameManager.Instance.SetCurrentDialogId(buildingId);
            dialogManager.SetDialog(dialogData);
            dialogManager.StartDialog();

            if (usePowder)
                GameManager.Instance.SetCurrentDialogPowdered(true);

            GameManager.Instance.MarkDialogTriggered(buildingId);
            isEntered = true;   // บันทึกว่าเข้าไปแล้ว
        }

        if (confirmPanel != null)
            confirmPanel.SetActive(false);

        clicked = true; // กันกดซ้ำหลังจากเข้าแล้ว
    }

    void OnCancel()
    {
        if (confirmPanel != null)
            confirmPanel.SetActive(false);

        // reset ให้กดใหม่ได้ในครั้งต่อไป
        if (!isEntered)
            clicked = false;
    }
}

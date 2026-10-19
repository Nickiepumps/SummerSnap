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
    public GameObject confirmPanel;
    public Button enterButton;
    public Button cancelButton;

    private bool clicked = false;
    private bool isEntered = false;

    private PlayerController playerController;
    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();

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
    private void OnMouseDown()
    {
        if (playerController.canMove == false || playerController.isOnDialogue == true || playerController.isOnSideQuest == true) return;
        if (GameManager.Instance != null && GameManager.Instance.HasTriggeredDialog(buildingId)) return;
        if (isEntered) return;
        if (clicked) return;

        clicked = true;
        
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(true);
            playerController.canMove = false;
            playerController.isOnSideQuest = true;
        }
    }
    private void OnEnter()
    {
        if (dialogManager != null && dialogData != null)
        {
            GameManager.Instance.SetCurrentDialogId(buildingId);
            dialogManager.SetDialog(dialogData);
            dialogManager.StartDialog();

            if (usePowder)
                GameManager.Instance.SetCurrentDialogPowdered(true);

            GameManager.Instance.MarkDialogTriggered(buildingId);
            isEntered = true;
        }

        if (confirmPanel != null)
            confirmPanel.SetActive(false);

        clicked = true;
    }
    private void OnCancel()
    {
        playerController.canMove = true;
        playerController.isOnSideQuest = false;

        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
        if (!isEntered)
        {
            clicked = false;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using Cysharp.Threading.Tasks;
using PrimeTween;
public class PlayerControlTutorialManager : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private PhotoPreviewSideQuests photoPreviewSideQuests;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private PhoneAppController phoneAppController;

    [SerializeField] private CinemachineCamera cinemachinePlayerCam;
    [SerializeField] private CinemachineCamera cinemachineSidequestCam;

    [SerializeField] private Transform sidequestFocusPos;

    [SerializeField] private GameObject movementTutorial;
    [SerializeField] private GameObject sidequestTutorial;
    [SerializeField] private CanvasGroup canvasPhoneToggleTutorial;

    [SerializeField] private GameObject ryuPhoneTutorial;
    [SerializeField] private GameObject ryuPhoneTriggerTutorial;
    [SerializeField] private GameObject phoneCallListPanel;
    [SerializeField] private BoxCollider2D mainQuestCollider;

    [SerializeField] private GameObject sideQuestTriggerCollider;
    [SerializeField] private BoxCollider2D sideQuestObjectCollider;
    [SerializeField] private PhotoQuestObject sideQuestObject;

    [SerializeField] private CanvasGroup[] sidequestInstructionArr;
    [SerializeField] private CanvasGroup[] ryuPhoneInstructionArr;
    private PlayerData tutorialData => GameManager.Instance.playerData;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        if(tutorialData.hasClearMovementTutorial == true)
        {
            movementTutorial.SetActive(false);
        }
        else
        {
            movementTutorial.SetActive(true);
            TriggerMovementTutorial().Forget();
        }
        if (tutorialData.hasClearSideQuestTutorial == true)
        {
            sideQuestTriggerCollider.SetActive(false);
        }
        else
        {
            sideQuestObjectCollider.enabled = false;
        }
        if (tutorialData.hasClearRyuPhoneCallTutorial == true)
        {
            ryuPhoneTriggerTutorial.SetActive(false);
        }
        else
        {
            mainQuestCollider.enabled = false;
        }

        playerController = GameManager.Instance.GetPlayerController();
        
        cinemachineSidequestCam.Target.TrackingTarget = sidequestFocusPos;
        cinemachinePlayerCam.Priority = 20;
        cinemachineSidequestCam.Priority = 10;

        foreach (CanvasGroup instruction in sidequestInstructionArr)
        {
            instruction.alpha = 0;
        }
        foreach (CanvasGroup instruction in ryuPhoneInstructionArr)
        {
            instruction.alpha = 0;
            instruction.blocksRaycasts = false;
        }
    }
    private async UniTaskVoid TriggerMovementTutorial()
    {
        await UniTask.WaitUntil(() => playerController.isWalking == true);
        movementTutorial.SetActive(false);
        tutorialData.hasClearMovementTutorial = true;
    }
    public void TriggerSidequestTutorial()
    {
        playerController.gameObject.SetActive(false);
        NPCDisplayManager.instance.ToggleNPCDisplay(false);
        PhoneUIManager.Instance.enabled = false;

        cinemachinePlayerCam.Priority = 10;
        cinemachineSidequestCam.Priority = 20;
        cinemachinePlayerCam.enabled = false;
        cinemachineSidequestCam.gameObject.SetActive(true);
        cinemachineSidequestCam.enabled = true;

        sidequestTutorial.SetActive(true);
        SidequestInstructionSequence().Forget();
    }
    public void TriggerRyuPhoneTutorial()
    {
        playerController.canMove = false;
        PersistentUI.instance.TogglePhoneUI(false);
        ryuPhoneTutorial.SetActive(true);
        RyuPhoneInstructionSequence().Forget();
    }
    private async UniTaskVoid RyuPhoneInstructionSequence()
    {
        for(int i = 0; i < ryuPhoneInstructionArr.Length; i++)
        {
            if (i == 0)
            {
                ryuPhoneInstructionArr[i].alpha = 1;
                ryuPhoneInstructionArr[i].blocksRaycasts = true;
                await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }
            else if (i == 1)
            {
                ryuPhoneInstructionArr[i - 1].alpha = 0;
                ryuPhoneInstructionArr[i].alpha = 1;
                ryuPhoneInstructionArr[i - 1].blocksRaycasts = false;
                ryuPhoneInstructionArr[i].blocksRaycasts = true;
                PersistentUI.instance.TogglePhoneUI(true);
                await UniTask.WaitUntil(() => PhoneUIManager.Instance.isOpen == true);
                PersistentUI.instance.TogglePhoneUI(false);
            }
            else if (i == 2)
            {
                ryuPhoneInstructionArr[i - 1].alpha = 0;
                ryuPhoneInstructionArr[i].alpha = 1;
                ryuPhoneInstructionArr[i - 1].blocksRaycasts = false;
                ryuPhoneInstructionArr[i].blocksRaycasts = true;
                await UniTask.WaitUntil(() => phoneCallListPanel.activeSelf == true);
            }
            else if (i == 3)
            {
                ryuPhoneInstructionArr[i - 1].alpha = 0;
                ryuPhoneInstructionArr[i].alpha = 1;
                ryuPhoneInstructionArr[i - 1].blocksRaycasts = false;
                ryuPhoneInstructionArr[i].blocksRaycasts = true;
                await UniTask.WaitUntil(() => phoneAppController.isCallingRyu == true);
                ryuPhoneInstructionArr[i].alpha = 0;
                ryuPhoneInstructionArr[i].blocksRaycasts = false;
                mainQuestCollider.enabled = true;
            }
        }
        tutorialData.hasClearRyuPhoneCallTutorial = true;
        playerController.canMove = true;
    }
    private async UniTaskVoid SidequestInstructionSequence()
    {
        for(int i = 0; i < sidequestInstructionArr.Length; i++)
        {
            if(i == 0)
            {
                sidequestInstructionArr[i].alpha = 1;
                await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }
            else if(i == 1)
            {
                sidequestInstructionArr[i - 1].alpha = 0;
                sidequestInstructionArr[i].alpha = 1;
                sideQuestObjectCollider.enabled = true;
                await UniTask.WaitUntil(() => sideQuestObject.clicked == true);
                cinemachinePlayerCam.Priority = 20;
                cinemachineSidequestCam.Priority = 10;
                cinemachinePlayerCam.enabled = true;
                cinemachineSidequestCam.enabled = false;
            }
            else
            {
                sidequestInstructionArr[i - 1].alpha = 0;
                sidequestInstructionArr[i].alpha = 1;
                playerController.gameObject.SetActive(true);
                await UniTask.WaitUntil(() => photoPreviewSideQuests.isTakingPhoto == true);
                sidequestInstructionArr[i].alpha = 0;
                await UniTask.WaitUntil(() => photoPreviewSideQuests.isTakingPhoto == false);
                PhoneUIManager.Instance.enabled = true;
                NPCDisplayManager.instance.ToggleNPCDisplay(true);
                sidequestTutorial.SetActive(false);
                PhoneToggleTutorial().Forget();
            }
        }
        tutorialData.hasClearSideQuestTutorial = true;
    }
    private async UniTaskVoid PhoneToggleTutorial()
    {
        canvasPhoneToggleTutorial.gameObject.SetActive(true);
        await UniTask.Delay(5000);
        await Tween.Alpha(canvasPhoneToggleTutorial, 0f, 0.3f, Easing.Standard(Ease.OutCubic));
        canvasPhoneToggleTutorial.gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingUIController : MonoBehaviour
{
    [Header("References")]
    public GameObject endingUI;                 // UI ที่มีปุ่ม OK / Cancel
    public EndingStoryUI blockEndingUI;         // Canvas เนื้อเรื่อง (Ending 1)
    public EndingStoryUI acquaintanceEndingUI;  // Canvas เนื้อเรื่อง (Ending 2)
    public EndingStoryUI bestFriendEndingUI;    // Canvas เนื้อเรื่อง (Ending 3)
    public DialogManager dialogManager;         // ใช้กับฉากจบความสัมพันธ์
    public DialogData relationshipEndingData;   // ข้อมูล Dialog สำหรับ Relationship Ending ❤️

    [Header("Scene Names")]
    public string fakeLoadingScene = "LoadingToMainmenu";
    public string homeScene = "Home";
    public string goodEndingVideoScene = "GoodEndCutscene";

    private void Start()
    {
        if (endingUI != null)
            endingUI.SetActive(false);
    }

    public void ShowEndingUI()
    {
        if (endingUI != null)
            endingUI.SetActive(true);
    }
    public void OnCancelPressed(TriggerDialogEnd trigger)
    {
        if (endingUI != null)
            endingUI.SetActive(false);

        if (trigger != null)
            trigger.ResetTrigger();
    }
    public void OnOkPressed()
    {
        PersistentUI.instance.TogglePhoneUI(false);

        if (endingUI != null)
            endingUI.SetActive(false);

        int totalScore = GameManager.Instance.GetTotalScore();
        
        int goodDialogs = 0;
        if (GameManager.Instance.scoreDekpang >= 10) goodDialogs++;
        if (GameManager.Instance.scoreDog >= 10) goodDialogs++;
        if (GameManager.Instance.scoreInTemple >= 10) goodDialogs++;

        Debug.Log($"[EndingUIController] ✅ กดปุ่ม OK → TotalScore={totalScore}, GoodDialogs={goodDialogs}");

        
        if (GameManager.Instance.scoreLevel1 == 0 &&
            GameManager.Instance.scoreLevel2 == 0 &&
            GameManager.Instance.scoreLevel3 == 0 &&
            GameManager.Instance.scoreLevel4 == 0)
        {
            if (blockEndingUI != null)
            {
                blockEndingUI.StartStory(() =>
                {
                    SceneManager.LoadScene(fakeLoadingScene);
                });
            }
            else Debug.LogWarning("⚠ blockEndingUI ยังไม่ได้เซ็ตใน Inspector!");
        }
        else if (totalScore < 70)
        {
            if (acquaintanceEndingUI != null)
            {
                acquaintanceEndingUI.StartStory(() =>
                {
                    SceneManager.LoadScene(fakeLoadingScene);
                });
            }
            else Debug.LogWarning("⚠ acquaintanceEndingUI ยังไม่ได้เซ็ตใน Inspector!");
        }
        else if ((totalScore >= 70 && totalScore < 120) || (totalScore >= 120 && goodDialogs < 2))
        {
            if (bestFriendEndingUI != null)
            {
                bestFriendEndingUI.StartStory(() =>
                {
                    SceneManager.LoadScene(fakeLoadingScene);
                });
            }
            else Debug.LogWarning("⚠ bestFriendEndingUI ยังไม่ได้เซ็ตใน Inspector!");
        }
        else if (totalScore >= 120 && goodDialogs >= 2)
        {
            if (dialogManager != null && relationshipEndingData != null)
            {
                dialogManager.SetDialog(relationshipEndingData);
                dialogManager.StartDialog();

                dialogManager.onDialogEnd = () =>
                {
                    SceneManager.LoadScene(goodEndingVideoScene);
                };
            }
            else Debug.LogWarning("⚠ Relationship Ending ยังไม่ได้เซ็ต dialogManager หรือ relationshipEndingData!");
        }
        else
        {
            Debug.LogWarning("⚠ ไม่เข้า Ending ไหนเลย! เช็คเงื่อนไขคะแนนอีกที");
        }
    }

}

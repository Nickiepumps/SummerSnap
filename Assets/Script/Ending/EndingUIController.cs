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
    public string fakeLoadingScene = "FakeLoading";
    public string homeScene = "Home";
    public string goodEndingVideoScene = "GoodEndVideo";

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

    // ✅ ปุ่ม Cancel
    public void OnCancelPressed(TriggerDialogEnd trigger)
    {
        if (endingUI != null)
            endingUI.SetActive(false);

        if (trigger != null)
            trigger.ResetTrigger();
    }

    // ✅ ปุ่ม OK
    // ✅ ปุ่ม OK
    public void OnOkPressed()
    {
        if (endingUI != null)
            endingUI.SetActive(false);

        // เช็คคะแนนรวมทั้งหมด
        int totalScore = GameManager.Instance.GetTotalScore();

        // คำนวณจำนวน dialog ดี
        int goodDialogs = 0;
        if (GameManager.Instance.scoreDekpang >= 10) goodDialogs++;
        if (GameManager.Instance.scoreDog >= 10) goodDialogs++;
        if (GameManager.Instance.scoreInTemple >= 10) goodDialogs++;

        Debug.Log($"[EndingUIController] ✅ กดปุ่ม OK → TotalScore={totalScore}, GoodDialogs={goodDialogs}");

        // ---- เงื่อนไข Ending ----
        if (GameManager.Instance.scoreLevel1 == 0 &&
            GameManager.Instance.scoreLevel2 == 0 &&
            GameManager.Instance.scoreLevel3 == 0 &&
            GameManager.Instance.scoreLevel4 == 0)
        {
            Debug.Log("👉 เข้าสู่ Ending 1: Block");
            if (blockEndingUI != null)
            {
                blockEndingUI.StartStory(() =>
                {
                    Debug.Log("👉 จบ Ending 1 → โหลด Scene: " + fakeLoadingScene);
                    SceneManager.LoadScene(fakeLoadingScene);
                });
            }
            else Debug.LogWarning("⚠ blockEndingUI ยังไม่ได้เซ็ตใน Inspector!");
        }
        else if (totalScore < 70)
        {
            Debug.Log("👉 เข้าสู่ Ending 2: แค่คนรู้จัก");
            if (acquaintanceEndingUI != null)
            {
                acquaintanceEndingUI.StartStory(() =>
                {
                    Debug.Log("👉 จบ Ending 2 → โหลด Scene: " + fakeLoadingScene);
                    SceneManager.LoadScene(fakeLoadingScene);
                });
            }
            else Debug.LogWarning("⚠ acquaintanceEndingUI ยังไม่ได้เซ็ตใน Inspector!");
        }
        else if ((totalScore >= 70 && totalScore < 120) || (totalScore >= 120 && goodDialogs < 2))
        {
            Debug.Log("👉 เข้าสู่ Ending 3: เพื่อนรัก");
            if (bestFriendEndingUI != null)
            {
                bestFriendEndingUI.StartStory(() =>
                {
                    Debug.Log("👉 จบ Ending 3 → โหลด Scene: " + fakeLoadingScene);
                    SceneManager.LoadScene(fakeLoadingScene);
                });
            }
            else Debug.LogWarning("⚠ bestFriendEndingUI ยังไม่ได้เซ็ตใน Inspector!");
        }
        else if (totalScore >= 120 && goodDialogs >= 2)
        {
            Debug.Log("👉 เข้าสู่ Ending 4: เลื่อนความสัมพันธ์ ❤️");
            if (dialogManager != null && relationshipEndingData != null)
            {
                dialogManager.SetDialog(relationshipEndingData);
                dialogManager.StartDialog();

                dialogManager.onDialogEnd = () =>
                {
                    Debug.Log("👉 จบ Ending 4 → โหลด Scene: " + goodEndingVideoScene);
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

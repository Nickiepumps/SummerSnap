using UnityEngine;

public class RelationshipEndingStarter : MonoBehaviour
{
    [Header("References")]
    public DialogManager dialogManager;
    public DialogData relationshipDialogData; // DialogData ของ Ending ความสัมพันธ์

    [Header("Scene Settings")]
    public string nextScene = "GoodEndVideo"; // ไปฉากวิดีโอ Good Ending

    private bool hasStarted = false;

    void Start()
    {
        // เริ่มฉาก → ให้ DialogManager เล่นอัตโนมัติเลย
        if (!hasStarted && dialogManager != null && relationshipDialogData != null)
        {
            hasStarted = true;

            dialogManager.SetDialog(relationshipDialogData);
            dialogManager.StartDialog();

            // callback ตอนจบบทสนทนา
            dialogManager.onDialogEnd = OnDialogEnd;
        }
    }

    private void OnDialogEnd()
    {
        Debug.Log("Relationship Ending: จบบทสนทนา → โหลดฉากวิดีโอ");
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
    }
}

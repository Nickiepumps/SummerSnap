using UnityEngine;
using UnityEngine.SceneManagement;

public class RelationshipEndingStarter : MonoBehaviour
{
    [Header("References")]
    public DialogManager dialogManager;
    public DialogData relationshipDialogData;

    [Header("Scene Settings")]
    public string nextScene = "GoodEndVideo";

    private bool hasStarted = false;

    void Start()
    {
        if (!hasStarted && dialogManager != null && relationshipDialogData != null)
        {
            hasStarted = true;
            dialogManager.SetDialog(relationshipDialogData);
            dialogManager.StartDialog();
            dialogManager.onDialogEnd = OnDialogEnd;
        }
    }
    private void OnDialogEnd()
    {
        SceneManager.LoadScene(nextScene);
    }
}

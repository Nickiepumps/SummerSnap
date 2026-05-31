using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogData", menuName = "Dialog/DialogData")]
public class DialogData : ScriptableObject
{
    public DialogManager.DialogLine[] lines; // เก็บบทสนทนาของ Trigger
}
